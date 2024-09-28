using System;
using UnityEngine;

namespace PinataMiniGame
{
	public class PinataPiece : MonoBehaviour
	{
		[SerializeField] private Rigidbody2D _rigid;
		[SerializeField] private Vector2 _direction;
		[SerializeField] private float _directionForce;
		[SerializeField] private float _rotationForce;

		private Vector3 _startPosition;
		private Quaternion _rotation;
		private bool _kicked;
		private void OnDrawGizmosSelected()
		{
			if(!_rigid)
				return;
			var dir = _rigid.transform.position + (Vector3)_direction * _directionForce;
			Gizmos.DrawLine(_rigid.transform.position, dir);
		}
		
		private void OnEnable()
		{
			_startPosition = transform.position;
			_rotation = transform.rotation;
			_rigid.AddForce(_direction * _directionForce, ForceMode2D.Impulse);
			_rigid.AddTorque(_rotationForce, ForceMode2D.Impulse);
			_kicked = true;
		}

		public void Reset()
		{
			if(!_kicked)
				return;
			transform.position = _startPosition;
			transform.rotation = _rotation;
			_rigid.velocity = Vector3.zero;
		}
	}
}