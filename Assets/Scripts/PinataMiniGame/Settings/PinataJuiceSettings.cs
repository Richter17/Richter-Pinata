using UnityEngine;

namespace PinataMiniGame.Settings
{
	[CreateAssetMenu(fileName = "PinataJuiceSettings", menuName = "Pinata Juice Settings", order = 0)]
	public class PinataJuiceSettings : ScriptableObject
	{
		[Header("Progress Settings")] 
		[SerializeField] private float _chargeDelay = 0.3f; 
		[SerializeField] private float[] _phases;
		
		[Header("Hit settings")]
		[SerializeField] private float _punchRotScale = 10;
		[SerializeField] private float _punchScale = 0.5f;
		[SerializeField] private float _punchDuration = 0.2f;
		[SerializeField] private int _punchVibration = 2;
	}
}