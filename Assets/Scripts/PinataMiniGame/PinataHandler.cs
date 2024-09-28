using System;
using System.Collections;
using Infrastructure;
using UnityEngine;

namespace PinataMiniGame
{
	public class PinataHandler : MonoBehaviour
	{
		[SerializeField] private ObjectInputHandler _inputHandler;
		[SerializeField] private Animator _animator;

		[Header("Return Settings")] 
		[SerializeField] private float _returnSpeed = 0.8f; 
		[Header("Progress Settings")] 
		[SerializeField] private float _chargeDelay = 0.3f; 
		[SerializeField] private float[] _phases;

		[Header("Explosion Settings")] 
		[SerializeField] private PinataPiece[] _pieceExplosionInfos;
	
		private Vector3 _startScale;
		private Quaternion _startRotation;
		private float _lastRot;
		private bool _canReturnToDefault;
		private float _chargeCount;
		private int _hitSfxIndex;
		private int _phaseIndex;
		private AudioClip _lastClip;
		private bool _canClick = true;

		private readonly int _idleState = Animator.StringToHash("Idle");
		private readonly int _hitState = Animator.StringToHash("Hit");
		private readonly int _explodeState = Animator.StringToHash("Explosion");

		public event Action<float> OnHit;
		public event Action<int> OnPhasePass;
		public event Action OnExplosion;

		private void Awake()
		{
			_inputHandler.onClick.Add(OnClickPinata);
			_startRotation = transform.rotation;
			_startScale = transform.localScale;
		}
		
		
		public void Reset()
		{
			foreach (var piece in _pieceExplosionInfos)
				piece.Reset();
			
			transform.localScale = _startScale;
			transform.rotation = _startRotation;
		}
		
		public void StartGame()
		{
			_canClick = true;
		}

		private void OnEnable()
		{
			_phaseIndex = 0;
			_animator.Play(_idleState);
			StartCoroutine(ReturnToDefaultState());
			StartCoroutine(DelayUntilReturn());
			_canClick = true;
		}

		private void OnClickPinata(ObjectInputHandler _)
		{
			if (!_canClick)
				return;
			HandleProgress();
			OnHit?.Invoke(_chargeCount);
		}

		private void HandleProgress()
		{
			_chargeCount += _chargeDelay;
			if (_phaseIndex >= _phases.Length)
			{
				Explosion();
				return;
			}
			if (_chargeCount < _phases[_phaseIndex]) 
				return;
			OnPhasePass?.Invoke(_hitSfxIndex);
			_phaseIndex++;
		}

		private void Explosion()
		{
			if(!_canClick)
				return;
			OnExplosion?.Invoke();
			_canClick = false;
		}
		
	
		private IEnumerator DelayUntilReturn() //can be with Unitask/Task
		{
			while (true)
			{
				yield return null;
				if(!_canClick)
					continue;
				if (_chargeCount <= 0)
				{
					_canReturnToDefault = true;
					continue;
				}
				_chargeCount -= Time.deltaTime;
				_canReturnToDefault = false;
			}
		}
	
		private IEnumerator ReturnToDefaultState() //can be with Unitask/Task
		{
			while (true)
			{
				yield return null;
				if(!_canClick)
					continue;
				var returnSpeed = _returnSpeed;
				if (!_canReturnToDefault)
					returnSpeed *= 0.1f;
				transform.localScale = Vector3.Lerp(transform.localScale, _startScale, returnSpeed * Time.deltaTime);
				transform.rotation = Quaternion.Lerp(transform.rotation, _startRotation, returnSpeed * Time.deltaTime);
			}
		}

		
	}
}
