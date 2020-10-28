using UnityEngine;

/// <summary>
/// float 型の拡張メソッドを管理するクラス
/// </summary>
public static class Extensions
{
	/// <summary>
	/// 指定のオブジェクトが現在のオブジェクトと等しいかどうかを判断します
	/// </summary>
	public static bool SafeEquals
	(
		this float self,
		float obj,
		float threshold = 0.001f
	)
	{
		return Mathf.Abs(self - obj) <= threshold;
	}
}