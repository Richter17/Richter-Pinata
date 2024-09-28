using System.Collections.Generic;

namespace Plugins.Richter.Haptic
{
	public abstract class HapticController : IHaptic
	{
		public static IHaptic Instance { get; }
		public static bool IsEnabled { get; set; } = true;
		
		public const float LIGHT = 0.2f, MEDIUM = 0.5f, STRONG = 1f; 
		
		/// <summary>
		/// No haptic effect. Used to generate extended delays between primitives.
		/// </summary>
		public const int PRIMITIVE_NOOP = 0;
		/// <summary>
		/// This effect should produce a sharp, crisp click sensation.
		/// </summary>
		public const int PRIMITIVE_CLICK = 1;

		/// <summary>
		/// downwards movement with gravity 
		/// </summary>
		public const int PRIMITIVE_THUD = 2;
		/// <summary>
		/// A haptic effect that simulates spinning momentum.
		/// </summary>
		public const int PRIMITIVE_SPIN = 3;
		/// <summary>
		/// A haptic effect that simulates quick upward movement against gravity.
		/// </summary>
		public const int PRIMITIVE_QUICK_RISE = 4; 
		/// <summary>
		/// A haptic effect that simulates slow upward movement against gravity.
		/// </summary>
		public const int PRIMITIVE_SLOW_RISE = 5;
		/// <summary>
		/// A haptic effect that simulates quick downwards movement with gravity.
		/// </summary>
		public const int PRIMITIVE_QUICK_FALL = 6;
		/// <summary>
		/// light crisp sensation intended to be used repetitively for dynamic feedback.
		/// </summary>
		public const int PRIMITIVE_TICK = 7;
		/// <summary>
		/// low frequency light
		/// </summary>
		public const int PRIMITIVE_LOW_TICK = 8;
		
		static HapticController()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			Instance = new AndroidHapticController();
#elif UNIITY_IOS && !UNITY_EDITOR
			Instance = new EmptyHapticController();
#else
			Instance = new EmptyHapticController();
#endif
		}

		public void Vibrate(VibrationType vibrationType)
		{
			if(!IsEnabled)
				return;
			Instance.Vibrate(vibrationType);
		}

		public void Vibrate(float amplitude, float duration)
		{
			if(!IsEnabled)
				return;
			Instance.Vibrate(amplitude, duration);
		}

		public void Vibrate(int predefined, float scale = 1)
		{
			if(!IsEnabled)
				return;
			Instance.Vibrate(predefined, scale);
		}

		public void Vibrate(List<VibrateStep> sequence, int repeat = -1)
		{
			if(!IsEnabled)
				return;
			Instance.Vibrate(sequence, repeat);
		}

		public void Stop()
		{
			if(!IsEnabled)
				return;
			Instance.Stop();
		}
	}
}