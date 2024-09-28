using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace PinataMiniGame
{
	public class PinataAnimatorHandler : PinataListener
	{
		[SerializeField] private Animator _animator;
		
		[Header("Hit settings")]
		[SerializeField] private float _punchRotScale = 10;
		[SerializeField] private float _punchScale = 0.5f;
		[SerializeField] private int _shakeRandomness = 90;
		[SerializeField] private float _punchDuration = 0.2f;
		[SerializeField] private int _punchVibration = 2;

		private readonly int _idleState = Animator.StringToHash("Idle");
		private readonly int _hitState = Animator.StringToHash("Hit");
		private readonly int _explodeState = Animator.StringToHash("Explosion");
		
		private void OnEnable()
		{
			_animator.Play(_idleState);
		}

		protected override void OnExplosion()
		{
			ExplodeAsync();
		}

		protected override void OnPhasePass(int index)
		{
			
		}

		protected override void OnHit(float charge)
		{
			Pop();
			HitAnimate();
		}

		protected override void OnReset()
		{
			
		}

		private void Pop()
		{
			_punchRotScale *= -1;
			transform.DOPunchRotation(Vector3.forward * _punchRotScale, _punchDuration, _punchVibration);
			transform.DOShakeScale(_punchDuration, _punchScale, _punchVibration, _shakeRandomness);
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