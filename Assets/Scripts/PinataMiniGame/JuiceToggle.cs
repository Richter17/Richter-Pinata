using System;
using UnityEngine;

namespace PinataMiniGame
{
	public enum JuiceType
	{
		Normal,
		Hard,
		Crazy
	}
	public class JuiceToggle : MonoBehaviour
	{
		[SerializeField] private JuiceType _juiceType;
		public static Action<JuiceType> OnSelectJuiceType;
		
		public void SetState(bool isActive)
		{
			if(!isActive)
				return;
			OnSelectJuiceType?.Invoke(_juiceType);
		}
	}
}