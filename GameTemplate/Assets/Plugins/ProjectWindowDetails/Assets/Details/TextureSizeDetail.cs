using UnityEngine;

namespace ProjectWindowDetail.Details
{
	/// <summary>
	/// Draws the width and height of texture assets into the project window.
	/// </summary>
	public class TextureSizeDetail : ProjectWindowDetailBase
	{
		public TextureSizeDetail()
		{
			Name = "Texture Size";
			ColumnWidth = 80;
			Alignment = TextAlignment.Right;
		}

		public override string GetLabel(string guid, string assetPath, Object asset)
		{
			var texture = asset as Texture;
			if (texture == null)
			{
				return string.Empty;
			}

			return string.Format("{0}x{1}", texture.width, texture.height);
		}
	}
}
