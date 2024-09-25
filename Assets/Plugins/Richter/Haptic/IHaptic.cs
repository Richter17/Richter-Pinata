using System.Collections.Generic;

namespace Plugins.Richter.Haptic
{
	public interface IHaptic
	{
		void Vibrate(VibrationType vibrationType);
		void Vibrate(float amplitude, float duration);
		void Vibrate(int predefined, float scale = 1);
		void Vibrate(List<(float amplitude, float duration)> amplitudes, int repeat = -1);
		void Stop();
	}

	public enum VibrationType
	{
		Short,
		Medium,
		Strong,
		Long
	}
}