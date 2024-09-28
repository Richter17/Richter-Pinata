using UnityEngine;
using UnityEngine.Events;

namespace PinataMiniGame
{
	public class EmptyListener : PinataListener
	{
		public UnityEvent OnStartTriggered;
		public UnityEvent OnHitTriggered;
		public UnityEvent OnPassPhaseTriggered;
		public UnityEvent OnExplodeTriggered;

		protected override void OnStart()
		{
			OnStartTriggered.Invoke();
		}

		protected override void OnExplosion()
		{
			OnExplodeTriggered.Invoke();
		}

		protected override void OnPhasePass(int index)
		{
			OnPassPhaseTriggered.Invoke();
		}

		protected override void OnHit(float charge)
		{
			OnHitTriggered.Invoke();
		}

		protected override void OnReset()
		{
			
		}
	}
}