using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PinataMiniGame
{
	public class PinataSoundController : PinataListener
	{
		[SerializeField] private float _hitSound = 0.5f;
		[SerializeField] private float _delayInExplosionSfx = 0.5f;
		[SerializeField] private List<AudioClip> _hitsPinataSfx;
		[SerializeField] private AudioClip[] _hitsCrackSfx;
		[SerializeField] private AudioClip[] _explosionsSfx;
		
		private AudioClip _lastClip;
		private int _hitSfxIndex;

		protected override void OnExplosion()
		{
			PlayExplosionSfx();
		}

		protected override void OnPhasePass(int index)
		{
			PlayPhaseSfx();
		}

		protected override void OnHit(float charge)
		{
			PlayHitSound(charge);
		}

		protected override void OnReset()
		{
			
		}

		private async Task PlayExplosionSfx()
		{
			await Task.Delay(TimeSpan.FromSeconds(_delayInExplosionSfx));
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

		private void PlayPhaseSfx()
		{
			_hitSfxIndex++;
			_hitSfxIndex %= _hitsCrackSfx.Length;
			SoundManager.Instance.PlaySFX(_hitsCrackSfx[_hitSfxIndex]);
		}
	}
}