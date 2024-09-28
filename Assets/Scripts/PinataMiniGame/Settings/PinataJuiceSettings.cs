using System.Collections.Generic;
using UnityEngine;

namespace PinataMiniGame.Settings
{
	[CreateAssetMenu(fileName = "PinataJuiceSettings", menuName = "Pinata Juice Settings", order = 0)]
	public class PinataJuiceSettings : ScriptableObject
	{
		[field: SerializeField] public JuiceType Juice { get; private set; } = JuiceType.Normal;
		[field: Header("Progress Settings"), SerializeField] public float ChargeDelay { get; private set; } = 0.3f;
		[field: SerializeField] public float ReturnSpeed { get; private set; } = 0.8f;
		[field: SerializeField] public float TimeoutMultiplier { get; private set; } = 1;
		[field:SerializeField] public float[] Phases { get; private set; }
		
		[field:Header("Elements"), SerializeField] public PinataAnimatorHandler Visual { get; private set; }
		[field:SerializeField] public List<PinataListener> GameElements { get; private set; }
	}
}