using UnityEngine;
using Object = UnityEngine.Object;

namespace ProjectWindowDetail.Details
{
	/// <summary>
	/// Draws the wrap mode of texture assets into the project window.
	/// </summary>
	public class TextureWrapModeDetail : ProjectWindowDetailBase
	{
		private static string[] _wrapModeStrings = new[] {"Rpt", "Clp", "Mrr", "MrO"};

		public TextureWrapModeDetail()
		{
			Name = "Texture Wrap Mode";
			ColumnWidth = 70;
		}

		public override string GetLabel(string guid, string assetPath, Object asset)
		{
			var texture = asset as Texture;

			var wu = Mathf.Clamp((int)texture.wrapModeU, 0, 3);
			var wv = Mathf.Clamp((int)texture.wrapModeV, 0, 3);

			if (wu == wv)
			{
				return _wrapModeStrings[wu];
			}

			return string.Format("{0}|{1}", _wrapModeStrings[wu], _wrapModeStrings[wv]);
		}
	}
}
