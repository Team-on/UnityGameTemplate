using System.IO;
using UnityEngine;

namespace ProjectWindowDetail.Details
{
	/// <summary>
	/// Draws the file path of assets into the project window.
	/// </summary>
	public class PathDetail : ProjectWindowDetailBase
	{
		public PathDetail()
		{
			Name = "Path";
			ColumnWidth = 400;
		}

		public override string GetLabel(string guid, string assetPath, Object asset)
		{
			return Path.GetDirectoryName(assetPath);
		}
	}
}
