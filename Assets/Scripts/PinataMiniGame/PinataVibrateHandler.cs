using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugins.Richter.Haptic;
using UnityEngine;

namespace PinataMiniGame
{
	public class PinataVibrateHandler : PinataListener
	{
		[SerializeField] private VibrationType _hitVibrationType;
		[SerializeField] private List<VibrateStep> _explodingSequence = new()
		{
			new (0.5f, 0.2f),
			new (0.0f, 0.01f),
			new (0.5f, 0.2f),
			new (0.0f, 0.01f),
			new (1f, 0.75f),
		};
		protected override void OnExplosion()
		{
			VibrateExplosion();
		}

		protected override void OnPhasePass(int index)
		{
			
		}

		protected override void OnHit(float charge)
		{
			VibrateHit();
		}

		protected override void OnReset()
		{
			
		}

		private void VibrateHit()
		{
			HapticController.Instance.Vibrate(_hitVibrationType);
		}

		private async Task VibrateExplosion()
		{
			await Task.Delay(TimeSpan.FromSeconds(1));
			HapticController.Instance.Vibrate(_explodingSequence);
		}
	}
}