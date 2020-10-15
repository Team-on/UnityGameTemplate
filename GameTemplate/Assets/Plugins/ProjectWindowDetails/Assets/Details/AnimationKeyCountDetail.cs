using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProjectWindowDetail.Details
{
	/// <summary>
	/// Draws the number of key of animation clips into the project window.
	/// </summary>
	public class AnimationKeyCountDetail : ProjectWindowDetailBase
	{
		public AnimationKeyCountDetail()
		{
			Name = "Animation Key Count";
			ColumnWidth = 50;
			Alignment = TextAlignment.Right;
		}

		public override string GetLabel(string guid, string assetPath, Object asset)
		{
			var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath);
			if (clip != null)
			{
				EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);
				var numKeys = 0;
				foreach (EditorCurveBinding binding in bindings)
				{
					AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
					numKeys += curve.length;
				}

				return string.Format("{0:D}", numKeys);
			}

			return string.Empty;
		}
	}
}
