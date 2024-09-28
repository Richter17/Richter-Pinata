using System.Collections;
using Infrastructure;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PinataMiniGame
{
	public class PinataEffectsHandler : PinataListener
	{
		[SerializeField] private float _radius = 1;
		[SerializeField] private ParticleSystem _pinataSparkles;
		[SerializeField] private ParticleSystem _pinataSparklesBig;
		[SerializeField] private int _sparksOnExplosions = 3;


		private ObjectPool<ParticleSystem> _particlesPool;
		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(transform.position, _radius);
		}

		public override void Init(PinataHandler pinataHandler)
		{
			base.Init(pinataHandler);
			_particlesPool = new(p
					=> p.gameObject.SetActive(true),
				p => p.gameObject.SetActive(false),
				() => Instantiate(_pinataSparkles, transform));
		}

		protected override void OnExplosion()
		{
			for (int i = 0; i < _sparksOnExplosions; i++)
			{
				ExplodeBig();
			}
		}

		protected override void OnHit(float charge)
		{
			var sparks = _particlesPool.Get();
			SetEffectPosition(sparks.transform);
			StartCoroutine(ReturnToPoolWhenFinished(sparks));
		}
		
		protected override void OnPhasePass(int obj)
		{
			ExplodeBig();
		}

		protected override void OnReset()
		{
			
		}

		private IEnumerator ReturnToPoolWhenFinished(ParticleSystem ps)
		{
			yield return new WaitWhile(() => ps.isPlaying);
			_particlesPool.Release(ps);
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