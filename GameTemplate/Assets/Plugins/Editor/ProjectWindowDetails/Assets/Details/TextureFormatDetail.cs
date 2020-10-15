using UnityEngine;
using Object = UnityEngine.Object;

namespace ProjectWindowDetail.Details
{
	/// <summary>
	/// Draws the format of texture assets into the project window.
	/// </summary>
	public class TextureFormatDetail : ProjectWindowDetailBase
	{
		public TextureFormatDetail()
		{
			Name = "Texture Format";
			ColumnWidth = 70;
		}

		public override string GetLabel(string guid, string assetPath, Object asset)
		{
			var texture = asset as Texture2D;
			if (texture != null)
			{
				return texture.format.ToString();
			}

			var renderTexture = asset as RenderTexture;
			if (renderTexture != null)
			{
				return renderTexture.format.ToString();
			}

			return string.Empty;
		}
	}
}
