using System;
using System.Collections.Generic;

namespace Plugins.Richter.Haptic
{
	public interface IHaptic
	{
		void Vibrate(VibrationType vibrationType);
		void Vibrate(float amplitude, float duration);
		void Vibrate(int predefined, float scale = 1);
		void Vibrate(List<VibrateStep> sequence, int repeat = -1);
		void Stop();
	}

	[Serializable]
	public struct VibrateStep
	{
		public float Amplitude;
		public float Duration;

		public VibrateStep(float amplitude, float duration)
		{
			Amplitude = amplitude;
			Duration = duration;
		}
	}

	public enum VibrationType
	{
		Short,
		Medium,
		Strong,
		Long
	}
}