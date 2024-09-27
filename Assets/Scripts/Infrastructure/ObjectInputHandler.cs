using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure
{
	public class ObjectInputHandler : MonoBehaviour
	{
		[SerializeField] private float _minimumDistanceForDrag = 10;

		[SerializeField] private bool _debug;
		//instance relevance 
		public List<Action<ObjectInputHandler>> onHold = new();
		public List<Action<ObjectInputHandler>> onClick = new();
		public List<Action<ObjectInputHandler>> onDragStarted = new();
		public List<Action<ObjectInputHandler>> onDragFinished = new();
		//static logic
		public static Action<ObjectInputHandler> OnHeld;
		public static Action<ObjectInputHandler> OnClicked;
		public static Action<ObjectInputHandler> OnDragStarted; 
		public static Action<(ObjectInputHandler from, ObjectInputHandler to)> OnDragCompleted; 
		
		public bool IsLocked { get; private set; }

		private const float HoldDuration = 0.6f, HoldWithoutRelease = 0.7f, ClickDuration = 0.3f;
		private bool _isMouseDown;
		private bool _isDragging;
		private float _mouseDownTime;
		private Vector3 _startPos;
		private static ObjectInputHandler _startedObject, _targetObject;
		private static ObjectInputHandler _currentSelectedObject;
		private bool DownButNotDragging => _isMouseDown && !_isDragging;

		private void OnMouseDown()
		{
			if(IsLocked)
				return;
			_isMouseDown = true;
			_mouseDownTime = Time.time;
			_startPos = Input.mousePosition;
			_currentSelectedObject = this;
			if(_debug) Debug.Log($"{gameObject.name} mouse down");
		}

		private void OnMouseDrag()
		{
			if(IsLocked)
				return;
			if(!DownButNotDragging)
				return;
			if (Vector3.Distance(_startPos, Input.mousePosition) > _minimumDistanceForDrag)
				OnMouseExit();
		}

		private void OnMouseExit()
		{
			if (_targetObject == this)
			{
				_targetObject = null;
			}
			if(IsLocked)
				return;
			if (!DownButNotDragging) 
				return;
			onDragStarted.Invoke(this);
			OnDragStarted?.Invoke(this);
			_isDragging = true;
			_startedObject = this;
			if(_debug) Debug.Log($"{gameObject.name} mouse drag start");
		}

		private void Update()
		{
			if (IsLocked || !_currentSelectedObject || !DownButNotDragging)
				return;
			var delta = Time.time - _mouseDownTime;
			if (!(delta >= HoldWithoutRelease)) 
				return;
			CallOnHold();
			_currentSelectedObject = null;
		}

		private void OnMouseUp()
		{
			if (IsLocked)
			{
				ResetState();
				return;
			}
			//in case the mouse didn't leave the same object
			if (DownButNotDragging)
			{
				var delta = Time.time - _mouseDownTime;
				switch (delta)
				{
					case <= ClickDuration:
						onClick.Invoke(this);
						OnClicked?.Invoke(this);
						if(_debug) Debug.Log($"{gameObject.name} mouse click");
						break;
					case > HoldDuration:
						CallOnHold();
						break;
				}
			}

			//in case it's dragging to another location
			if (_isDragging)
			{
				_targetObject = _startedObject != _targetObject ? _targetObject : null;
				onDragFinished.Invoke(_targetObject);
				OnDragCompleted?.Invoke(new(_startedObject, _targetObject));
				if(_debug) Debug.Log($"{gameObject.name} mouse drag end");
			}

			ResetState();
		}

		private void ResetState()
		{
			_isMouseDown = false;
			_startedObject = _targetObject = null;
			_isDragging = false;
			_currentSelectedObject = null;
		}

		private void OnMouseEnter()
		{
			if(IsLocked)
				return;
			_targetObject = this;
		}

		private void CallOnHold()
		{
			onHold.Invoke(this);
			OnHeld?.Invoke(this);
			if(_debug) Debug.Log($"{gameObject.name} mouse hold");
		}
		
		public void Lock()
		{
			IsLocked = true;
		}

		public void Unlock()
		{
			IsLocked = false;
		}
	}

	public static class Helper
	{
		public static void Invoke(this List<Action<ObjectInputHandler>> list, ObjectInputHandler target)
		{
			var actions = new List<Action<ObjectInputHandler>>(list);
			foreach (var action in actions)
			{
				action?.Invoke(target);
			}
		}
	}
}