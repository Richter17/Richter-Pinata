using System;
using System.Collections.Generic;
using Infrastructure;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PinataMiniGame
{
	public class PinataSoundController : MonoBehaviour
	{
		[SerializeField] private PinataHandler _pinata;
		[SerializeField] private float _hitSound = 0.5f;
		[SerializeField] private List<AudioClip> _hitsPinataSfx;
		[SerializeField] private AudioClip[] _hitsCrackSfx;
		[SerializeField] private AudioClip[] _explosionsSfx;
		
		private AudioClip _lastClip;
		private int _hitSfxIndex;

		private void Awake()
		{
			_pinata.OnHit += PlayHitSound;
			_pinata.OnPhasePass += PlayPhaseSfx;
			_pinata.OnExplosion += PlayExplosionSfx;
		}

		private void PlayExplosionSfx()
		{
			foreach (var clip in _explosionsSfx)
			{
				SoundManager.Instance.PlaySFX(clip);
			}
		}

		private void PlayHitSound(float charge)
		{
			var currentClip = _hitsPinataSfx[Random.Range(0, _hitsPinataSfx.Count)];
			SoundManager.Instance.PlaySFX(currentClip, _hitSound);
			//to make sure the same sound won't occur twice in a row
			if (_lastClip)
				_hitsPinataSfx.Add(_lastClip);
			_lastClip = currentClip;
			_hitsPinataSfx.Remove(currentClip);
		}

		private void PlayPhaseSfx(int phase)
		{
			_hitSfxIndex++;
			_hitSfxIndex %= _hitsCrackSfx.Length;
			SoundManager.Instance.PlaySFX(_hitsCrackSfx[_hitSfxIndex]);
		}
	}
}