using System;
using UnityEngine;

namespace PinataMiniGame
{
	public abstract class PinataListener : MonoBehaviour
	{
		private PinataHandler _pinata;
		
		public virtual void Init(PinataHandler pinataHandler)
		{
			_pinata = pinataHandler;
			_pinata.OnHit += OnHit;
			_pinata.OnStart += OnStart;
			_pinata.OnReset += OnReset;
			_pinata.OnPhasePass += OnPhasePass;
			_pinata.OnExplosion += OnExplosion;
		}

		protected virtual void OnStart()
		{
			
		}

		private void OnDestroy()
		{
			_pinata.OnHit -= OnHit;
			_pinata.OnStart -= OnStart;
			_pinata.OnReset -= OnReset;
			_pinata.OnPhasePass -= OnPhasePass;
			_pinata.OnExplosion -= OnExplosion;
		}

		protected abstract void OnExplosion();

		protected abstract void OnPhasePass(int index);

		protected abstract void OnHit(float charge);
		
		protected abstract void OnReset();
	}
}