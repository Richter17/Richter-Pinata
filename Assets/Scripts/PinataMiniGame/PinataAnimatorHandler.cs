using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace PinataMiniGame
{
	public class PinataAnimatorHandler : MonoBehaviour
	{
		[SerializeField] private PinataHandler _main;
		[SerializeField] private Animator _animator;
		
		private readonly int _idleState = Animator.StringToHash("Idle");
		private readonly int _hitState = Animator.StringToHash("Hit");
		private readonly int _explodeState = Animator.StringToHash("Explosion");
		
		[Header("Hit settings")]
		[SerializeField] private float _punchRotScale = 10;
		[SerializeField] private float _punchScale = 0.5f;
		[SerializeField] private float _punchDuration = 0.2f;
		[SerializeField] private int _punchVibration = 2;

		private void Awake()
		{
			_main.OnHit += OnHitPinata;
			_main.OnExplosion += OnPinataExplode;
		}

		private void OnPinataExplode()
		{
			ExplodeAsync();
		}

		private void OnHitPinata(float obj)
		{
			Pop();
			HitAnimate();
		}

		private async Task Pop()
		{
			_punchRotScale *= -1;
			transform.DOPunchRotation(Vector3.forward * _punchRotScale, _punchDuration, _punchVibration);
			await transform.DOPunchScale(transform.localScale * _punchScale, _punchDuration, _punchVibration).AsyncWaitForCompletion();
		
		}
		
		private void HitAnimate()
		{
			_animator.Play(_hitState);
		}
		
		private async Task ExplodeAsync()
		{
			transform.DOShakePosition(1f, Vector3.one);
			await Task.Delay(TimeSpan.FromSeconds(0.9f));
			_animator.Play(_explodeState);
		}
	}
}