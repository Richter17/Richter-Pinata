using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PinataMiniGame
{
	public class PinataEffectsHandler : MonoBehaviour
	{
		[SerializeField] private PinataHandler _main;
		[SerializeField] private float _radius = 1;
		[SerializeField] private ParticleSystem _pinataSparkles;
		[SerializeField] private ParticleSystem _pinataSparklesBig;
		[SerializeField] private int _sparksOnExplosions = 3;


		private ObjectPool<ParticleSystem> _particlesPool;
		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(transform.position, _radius);
		}

		private void Awake()
		{
			_main.OnHit += OnHit;
			_main.OnPhasePass += OnPhasePass;
			_main.OnExplosion += OnExplosion;
			_particlesPool = new(p
					=> p.gameObject.SetActive(true),
				p => p.gameObject.SetActive(false),
				() => Instantiate(_pinataSparkles, transform));
		}

		private void OnExplosion()
		{
			for (int i = 0; i < _sparksOnExplosions; i++)
			{
				ExplodeBig();
			}
		}

		private void OnHit(float charge)
		{
			var sparks = _particlesPool.Get();
			SetEffectPosition(sparks.transform);
			StartCoroutine(ReturnToPoolWhenFinished(sparks));
		}
		
		private IEnumerator ReturnToPoolWhenFinished(ParticleSystem ps)
		{
			yield return new WaitWhile(() => ps.isPlaying);
			_particlesPool.Release(ps);
		}

		private void OnPhasePass(int obj)
		{
			ExplodeBig();
		}

		private void ExplodeBig()
		{
			var sparks = Instantiate(_pinataSparklesBig, transform);
			SetEffectPosition(sparks.transform);
			StartCoroutine(DestroyWhenFinished(sparks));
		}
		
		private IEnumerator DestroyWhenFinished(ParticleSystem ps)
		{
			yield return new WaitWhile(() => ps.isPlaying);
			Destroy(ps.gameObject);
		}

		private void SetEffectPosition(Transform sparks)
		{
			var randPos = (Vector3)Random.insideUnitCircle * _radius;
			sparks.position = transform.position + randPos;
		}
	}
}