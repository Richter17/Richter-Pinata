using UnityEngine;

namespace AssetManagement
{
	public interface IAssetsService
	{
		T GetAsset<T>(string path) where T : Object; //can be async

		T GetAssetById<T>(string id) where T : Object; //can be async

		void UnloadAsset(string path);
		void UnloadAssetById(string id);
	}
}
