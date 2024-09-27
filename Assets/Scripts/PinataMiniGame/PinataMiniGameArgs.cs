using System.Collections.Generic;

namespace PinataMiniGame
{
	public class PinataMiniGameArgs
	{
		public string AssetBundleUrl;
		public List<Reward> Rewards;
	}

	public class Reward
	{
		public string Type;
		public string AssetBundleUrl;
		public int Value;
	}
}