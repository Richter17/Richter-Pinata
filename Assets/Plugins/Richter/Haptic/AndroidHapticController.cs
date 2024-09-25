#if UNITY_ANDROID
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.Richter.Haptic
{
	public class AndroidHapticController : IHaptic
	{
		private readonly AndroidJavaObject _plugin;
		public AndroidHapticController()
		{
			var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			var unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			_plugin = new AndroidJavaObject("com.garage.hapticfeedback.HapticController");

			if (_plugin == null)
			{
				Debug.LogError("Failed to get the plugin");
				return;
			}

			_plugin.CallStatic("receiveUnityActivity", unityActivity);
		}

		public void Vibrate(VibrationType vibrationType)
		{
			float amp;
			float duration;
			switch (vibrationType)
			{
				case VibrationType.Short:
				default:
					amp = HapticController.LIGHT; duration = 0.1f;
					break;
				case VibrationType.Medium:
					amp = HapticController.MEDIUM; duration = 0.2f;
					break;
				case VibrationType.Strong:
					amp = HapticController.STRONG; duration = 0.2f;
					break;
				case VibrationType.Long:
					amp = HapticController.STRONG; duration = 1f;
					break;
			}
			Vibrate(amp, duration);
		}

		public void Vibrate(float amplitude, float duration)
		{
			_plugin?.Call(nameof(Vibrate), ConvertRangeToAmplitude(amplitude), ConvertSecondsToMilliseconds(duration), -1);
		}

		//doesn't seem to work, check on this later
		public void Vibrate(int predefined, float scale)
		{
			if (predefined is > -1 and < 9)
			{
				_plugin?.Call(nameof(Vibrate), predefined, Mathf.Clamp01(scale));
			}
		}

		public void Vibrate(List<(float amplitude, float duration)> vibrateSequence, int repeat = -1)
		{
			var amps = new int[vibrateSequence.Count];
			var mss = new long[vibrateSequence.Count];
			for (int i = 0; i < vibrateSequence.Count; i++)
			{
				amps[i] = ConvertRangeToAmplitude(vibrateSequence[i].amplitude);
				mss[i] = ConvertSecondsToMilliseconds(vibrateSequence[i].duration);
			}
			
			_plugin?.Call(nameof(Vibrate), amps, mss, repeat);
		}

		private int ConvertRangeToAmplitude(float val) => Mathf.CeilToInt(Mathf.Clamp01(val) * 255);
		private long ConvertSecondsToMilliseconds(float val) => Mathf.RoundToInt(val * 1000);

		public void Stop()
		{
			_plugin?.Call(nameof(Stop));
		}
	}
}
#endif