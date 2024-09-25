using System.Collections.Generic;
using UnityEngine;

namespace Plugins.Richter.Haptic
{
	public class EmptyHapticController : IHaptic
	{
		public void Vibrate(VibrationType vibrationType)
		{
			Debug.Log($"Vibrate {vibrationType}");
		}

		public void Vibrate(float amplitude, float duration)
		{
			Log($"Vibrate amp: {amplitude} for: {duration}");
		}

		public void Vibrate(int predefined, float scale = 1)
		{
			Log($"Vibrate primitive {predefined} with scale: {1}");
		}

		public void Vibrate(List<(float amplitude, float duration)> amplitudes, int repeat = -1)
		{
			Log($"Vibrate unique pattern");
		}

		public void Stop()
		{
			Log($"Vibrate stop");
		}

		private void Log(string message, string caller = nameof(HapticController))
		{
			if(Application.isEditor)
				Debug.Log($"[{caller}] {message}");
		}
	}
}