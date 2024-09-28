using System;
using System.Collections.Generic;
using Plugins.Richter.Haptic;
using UnityEngine;

namespace PinataMiniGame
{
	public class PinataVibrateHandler : MonoBehaviour
	{
		[SerializeField] private PinataHandler _pinata;

		private void Awake()
		{
			_pinata.OnHit += OnPinataHit;
			_pinata.OnExplosion += OnPinataExplode;
		}

		private void OnPinataHit(float charge)
		{
			HapticController.Instance.Vibrate(VibrationType.Medium);
		}

		private void OnPinataExplode()
		{
			HapticController.Instance.Vibrate(new List<(float amplitude, float duration)>()
			{
				(0.5f, 0.2f),
				(0.0f, 0.01f),
				(0.5f, 0.2f),
				(0.0f, 0.01f),
				(1f, 0.75f),
			});
		}
	}
}