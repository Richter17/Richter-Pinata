using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Infrastructure;
using UnityEngine;
using Random = UnityEngine.Random;

public class PinataHandler : MonoBehaviour
{

	[SerializeField] private ObjectInputHandler _inputHandler;
	[SerializeField] private Animator _animator;

	[Header("Return Settings")] 
	[SerializeField] private float _chargeDelay = 0.3f; 
	[SerializeField] private float _returnSpeed = 0.8f; 
	[Header("Hit settings")]
	[SerializeField] private float _punchRotScale = 10;
	[SerializeField] private float _punchScale = 0.5f;
	[SerializeField] private float _punchDuration = 0.2f;
	[SerializeField] private int _punchVibration = 2;
	[Header("Sounds")] 
	[SerializeField] private float _hitSound = 0.5f;
	[SerializeField] private List<AudioClip> _hitsPinataSfx;
	[SerializeField] private AudioClip[] _hitsCrackSfx;

	[Header("Progress Settings")] 
	[SerializeField] private float[] _phases;

	private Vector3 _startScale;
	private Quaternion _startRotation;
	private float _lastRot;
	private bool _canReturnToDefault;
	private float _chargeCount;
	private int _hitSfxIndex;
	private int _phaseIndex;
	private AudioClip _lastClip;

	private readonly int HitState = Animator.StringToHash("Hit");

	public Action<float> OnHit;
	public Action<int> OnPhasePass;
	

	private void Awake()
	{
		_inputHandler.onClick.Add(OnClickPinata);
		_startRotation = transform.rotation;
		_startScale = transform.localScale;
	}

	private void OnEnable()
	{
		_phaseIndex = 0;
		StartCoroutine(ReturnToDefaultState());
		StartCoroutine(DelayUntilReturn());
	}

	private void OnClickPinata(ObjectInputHandler _)
	{
		_chargeCount += _chargeDelay;
		Pop();
		Animate();
		PlayHitSound();
		HandlePopPhase();
		OnHit?.Invoke(_chargeCount);
	}

	private void HandlePopPhase()
	{
		if (_phaseIndex >= _phases.Length)
		{
			
			return;
		}
		if (_chargeCount < _phases[_phaseIndex]) 
			return;
		OnPhasePass?.Invoke(_hitSfxIndex);
		_hitSfxIndex++;
		_hitSfxIndex %= _hitsCrackSfx.Length;
		SoundManager.Instance.PlaySFX(_hitsCrackSfx[_hitSfxIndex]);
		_phaseIndex++;
	}

	private async Task Pop()
	{
		_punchRotScale *= -1;
		transform.DOPunchRotation(Vector3.forward * _punchRotScale, _punchDuration, _punchVibration);
		await transform.DOPunchScale(transform.localScale * _punchScale, _punchDuration, _punchVibration).AsyncWaitForCompletion();
		
	}

	private IEnumerator DelayUntilReturn() //can be with Unitask/Task
	{
		while (true)
		{
			yield return null;
			if (_chargeCount <= 0)
			{
				_canReturnToDefault = true;
				continue;
			}
			_chargeCount -= Time.deltaTime;
			_canReturnToDefault = false;
		}
	}

	private void Animate()
	{
		_animator.Play(HitState);
	}

	private void PlayHitSound()
	{
		var currentClip = _hitsPinataSfx[Random.Range(0, _hitsPinataSfx.Count)];
		SoundManager.Instance.PlaySFX(currentClip, _hitSound);
		//to make sure the same sound won't occur twice in a row
		if (_lastClip)
			_hitsPinataSfx.Add(_lastClip);
		_lastClip = currentClip;
		_hitsPinataSfx.Remove(currentClip);
	}

	private IEnumerator ReturnToDefaultState() //can be with Unitask/Task
	{
		while (true)
		{
			yield return null;
			var returnSpeed = _returnSpeed;
			if (!_canReturnToDefault)
				returnSpeed *= 0.1f;
			transform.localScale = Vector3.Lerp(transform.localScale, _startScale, returnSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, _startRotation, returnSpeed * Time.deltaTime);
		}
	}
}
