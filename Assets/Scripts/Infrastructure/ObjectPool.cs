using System;
using System.Collections.Generic;

namespace Infrastructure
{
	public class ObjectPool<T>
	{
		private readonly Stack<T> _stack = new Stack<T>();
		private readonly Action<T> _onGet;
		private readonly Action<T> _onRelease;
		private readonly Func<T> _onCreateNew;
		
		public int Count { get; private set; }

		public ObjectPool(Action<T> onGet, Action<T> onRelease, Func<T> onCreate)
		{
			_onGet = onGet;
			_onRelease = onRelease;
			if (onCreate == null)
				throw new Exception("onCreate can not be null");
			_onCreateNew = onCreate;
		}

		public T Get()
		{
			T element;
			if (_stack.Count == 0)
			{
				element = _onCreateNew();
				Count++;
			}
			else
			{
				element = _stack.Pop();
			}

			_onGet?.Invoke(element);
			return element;
		}

		public void Release(T element)
		{
			// if(_stack.Count > 0 && ReferenceEquals(_stack.Peek(), element))
			// 	GarageLogger.Error(this,"Internal error, Trying to destroy something that already released");
			_onRelease?.Invoke(element);
			_stack.Push(element);
		}
	}
}