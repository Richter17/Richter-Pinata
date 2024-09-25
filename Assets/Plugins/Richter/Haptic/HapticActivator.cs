using UnityEngine;

namespace Plugins.Richter.Haptic
{
	public class HapticActivator : MonoBehaviour
	{
		[SerializeField] private float _amplitude = 1;
		[SerializeField] private float _duration = 1;

		public void ActivateVibrate(int vibrationType) => ActivateVibrate((VibrationType)vibrationType);
		
		public void ActivateVibrate(VibrationType vibrationType)
			=> HapticController.Instance.Vibrate(vibrationType);
		
		public void ActivatePresetVibrate() => HapticController.Instance.Vibrate(_amplitude, _duration);
	}
}