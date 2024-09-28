using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using PinataMiniGame.Settings;
using UnityEngine;

namespace PinataMiniGame
{
	public class PinataHandler : MonoBehaviour
	{
		[SerializeField] private ObjectInputHandler _inputHandler;
		
		private PinataJuiceSettings _settings;
		private Vector3 _startScale;
		private Quaternion _startRotation;
		private float _lastRot;
		private bool _canReturnToDefault;
		private float _chargeCount;
		private float _returnTimout;
		private int _hitSfxIndex;
		private int _phaseIndex;
		private AudioClip _lastClip;
		private bool _canClick = true;
		private Transform _visual;
		private readonly List<PinataListener> _listeners = new();

		public event Action<float> OnHit;
		public event Action<int> OnPhasePass;
		public event Action OnExplosion;
		public event Action OnStart;
		public event Action OnReset;
		
		private void Awake()
		{
			_inputHandler.onClick.Add(OnClickPinata);
			
		}
		
		public void Init(PinataJuiceSettings settings)
		{
			if(!settings)
				return;
			_settings = settings;
			InitVisual(settings.Visual);
			foreach (var prefab in settings.GameElements)
			{
				var listener = Instantiate(prefab, transform.parent);
				listener.Init(this);
				_listeners.Add(listener);
			}
		}

		private void InitVisual(PinataAnimatorHandler prefab)
		{
			var visual = Instantiate(prefab, transform.parent);
			visual.Init(this);
			_visual = visual.transform;
			_startRotation = _visual.rotation;
			_startScale = _visual.localScale;
		}
		
		
		public void Reset()
		{
			OnReset?.Invoke();
			_visual.localScale = _startScale;
			_visual.rotation = _startRotation;
			Destroy(_visual.gameObject);
			foreach (var listener in _listeners)
			{
				Destroy(listener.gameObject);
			}
			_listeners.Clear();
		}
		
		public void StartGame()
		{
			_canClick = true;
			OnStart?.Invoke();
		}

		private void OnEnable()
		{
			_chargeCount = 0;
			_phaseIndex = 0;
			StartCoroutine(ReturnToDefaultState());
			StartCoroutine(ProcessReturn());
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
			_chargeCount += _settings.ChargeDelay;
			if (_phaseIndex >= _settings.Phases.Length)
			{
				Explosion();
				return;
			}
			if (_chargeCount < _settings.Phases[_phaseIndex]) 
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
		
	
		private IEnumerator ProcessReturn() //can be with Unitask/Task
		{
			while (true)
			{
				yield return null;
				if(!_canClick)
					continue;
				if (_returnTimout <= 0)
				{
					_canReturnToDefault = true;
					continue;
				}
				_chargeCount -= Time.deltaTime;
				_returnTimout -= Time.deltaTime * _settings.TimeoutMultiplier;
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
				var returnSpeed = _settings.ReturnSpeed;
				if (!_canReturnToDefault)
					returnSpeed *= 0.1f;
				_visual.localScale = Vector3.Lerp(_visual.localScale, _startScale, returnSpeed * Time.deltaTime);
				_visual.rotation = Quaternion.Lerp(_visual.rotation, _startRotation, returnSpeed * Time.deltaTime);
			}
		}
	}
}
