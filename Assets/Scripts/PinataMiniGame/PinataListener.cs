using System;
using UnityEngine;

namespace PinataMiniGame
{
	public abstract class PinataListener : MonoBehaviour
	{
		[SerializeField] private PinataHandler _pinata;

		private void Awake()
		{
			_pinata.OnHit += OnHit;
			_pinata.OnPhasePass += OnPhasePass;
			_pinata.OnExplosion += OnExplosion;
		}

		protected abstract void OnExplosion();

		protected abstract void OnPhasePass(int index);

		protected abstract void OnHit(float charge);
	}
}