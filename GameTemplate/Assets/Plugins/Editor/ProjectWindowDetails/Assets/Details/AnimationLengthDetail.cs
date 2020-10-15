using UnityEditor;
using UnityEngine;

namespace ProjectWindowDetail.Details
{
	/// <summary>
	/// Draws the length of animation clips into the project window.
	/// </summary>
	public class AnimationLengthDetail : ProjectWindowDetailBase
	{
		public AnimationLengthDetail()
		{
			Name = "Animation Length";
			ColumnWidth = 70;
			Alignment = TextAlignment.Right;
		}

		public override string GetLabel(string guid, string assetPath, Object asset)
		{
			var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath);
			if (clip != null)
			{
				return string.Format("{0:F3}", clip.length);
			}

			return string.Empty;
		}
	}
}
