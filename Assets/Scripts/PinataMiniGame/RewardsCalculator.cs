using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PinataMiniGame
{
	public class RewardsCalculator : PinataListener
	{
		[SerializeField] private RewardItem _goldReward;
		[SerializeField] private RewardItem _itemReward;
		[SerializeField] private Transform _container;
		
		[SerializeField] private int _maxGold = 2000;
		[SerializeField] private int _minGold = 1000;
		[SerializeField] private int _minItems = 0;
		[SerializeField] private int _maxItems = 2;
		
		
		protected override void OnExplosion()
		{
			var goldReward = Random.Range(_minGold, _maxGold + 1);
			var items = Random.Range(_minItems, _maxItems + 1);
			var possibleItems = _itemReward.PossibleItems;
			var received = new string[items];
			for (int i = 0; i < items; i++)
			{
				if(possibleItems.Count == 0)
					break;
				var index = Random.Range(0, possibleItems.Count);
				received[i] = possibleItems[index];
				possibleItems.RemoveAt(index);
			}

			ShowRewards(goldReward, received);
		}

		private async Task ShowRewards(int goldReward, string[] received)
		{
			await Task.Delay(TimeSpan.FromSeconds(1));
			InitGoldReward(goldReward);
			foreach (var item in received)
				InitItemReward(item);
		}

		private void InitGoldReward(int value)
		{
			if (value <= 0)
				return;
			var reward = Instantiate(_goldReward, _container);
			reward.Init(value);
		}
		
		private void InitItemReward(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return;
			var reward = Instantiate(_itemReward, _container);
			reward.Init(value);
		}

		protected override void OnPhasePass(int index)
		{
			
		}

		protected override void OnHit(float charge)
		{
			
		}

		protected override void OnReset()
		{
			
		}
	}
}