using UnityEngine;

namespace ProjectWindowDetail.Details
{
	/// <summary>
	/// Draws the type of assets into the project window.
	/// </summary>
	public class AssetTypeDetail : ProjectWindowDetailBase
	{
		public AssetTypeDetail()
		{
			Name = "Asset Type";
			ColumnWidth = 100;
		}
		public override string GetLabel(string guid, string assetPath, Object asset)
		{
			return asset.GetType().Name;
		}

	}
}
