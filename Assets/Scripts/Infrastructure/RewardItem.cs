using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure
{
	public class RewardItem : MonoBehaviour
	{
		[SerializeField] private Image _icon;
		[SerializeField] private TMP_Text _value;
		[SerializeField] private List<ValueToImage> _valueToImages;
		[Serializable]
		private struct ValueToImage
		{
			public int Amount;
			public string Value;
			public Sprite Icon;
		}

		public List<string> PossibleItems => _valueToImages.Select(v => v.Value).ToList();

		public async Task Init(int value)
		{
			var child = transform.GetChild(0);
			child.localScale = Vector3.zero;
			
			var index= _valueToImages.FindIndex(v => v.Amount > value);
			if (index == -1)
				index = _valueToImages.Count - 1;
			_icon.sprite = _valueToImages[index].Icon;
			_value.text = string.Empty;
			await child.DOScale(Vector3.one, 1).SetEase(Ease.OutQuad).AsyncWaitForCompletion();
			StartCoroutine(ShowTextAnimation(value));
		}
		
		public void Init(string item)
		{
			var child = transform.GetChild(0);
			child.localScale = Vector3.zero;
			var index= _valueToImages.FindIndex(v => v.Value == item);
			if (index == -1)
			{
				Debug.LogError($"Item {item} is not supported");
				gameObject.SetActive(false);
				return;
			}

			_icon.sprite = _valueToImages[index].Icon;
			_value.text = item;
			child.DOScale(Vector3.one, 1).SetEase(Ease.OutQuad);
		}

		private IEnumerator ShowTextAnimation(int value)
		{
			var current = 0;
			var delta = Mathf.RoundToInt(value * Time.deltaTime);
			while (current < value)
			{
				current = Mathf.Min(current + delta, value);
				_value.text = current.ToString();
				yield return null;
			}
		}
	}
}