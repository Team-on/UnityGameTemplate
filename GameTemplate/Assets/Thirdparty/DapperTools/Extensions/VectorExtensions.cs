using UnityEngine;

public static class VectorExtensions {
	/// <summary>
	/// Return a copy of this vector with an altered x and/or y and/or z component
	/// </summary>
	/// <param name="v"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	/// <returns></returns>
	public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null) {
		return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
	}

	/// <summary>
	/// Return a copy of this vector with an altered x and/or y component
	/// </summary>
	/// <param name="v"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public static Vector2 With(this Vector2 original, float? x = null, float? y = null) {
		return new Vector2(x ?? original.x, y ?? original.y);
	}

	/// <summary>
	/// Return this vector with only its x and y components
	/// </summary>
	/// <param name="v"></param>
	/// <returns></returns>
	public static Vector2 ToVector2(this Vector3 v) {
		// Create a Vector2 from the Vector3 but ignore its z component
		return new Vector2(v.x, v.y);
	}

	/// <summary>
	/// Return a copy of this vector with an altered x component
	/// </summary>
	/// <param name="v"></param>
	/// <param name="x"></param>
	/// <returns></returns>
	public static Vector2 ChangeX(this Vector2 v, float x) {
		return new Vector2(x, v.y);
	}

	/// <summary>
	/// Return a copy of this vector with an altered y component
	/// </summary>
	/// <param name="v"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public static Vector2 ChangeY(this Vector2 v, float y) {
		return new Vector2(v.x, y);
	}

	/// <summary>
	/// Return a copy of this vector with an altered x component
	/// </summary>
	/// <param name="v"></param>
	/// <param name="x"></param>
	/// <returns></returns>
	public static Vector3 ChangeX(this Vector3 v, float x) {
		return new Vector3(x, v.y, v.z);
	}

	/// <summary>
	/// Return a copy of this vector with an altered y component
	/// </summary>
	/// <param name="v"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public static Vector3 ChangeY(this Vector3 v, float y) {
		return new Vector3(v.x, y, v.z);
	}

	/// <summary>
	/// Return a copy of this vector with an altered z component
	/// </summary>
	/// <param name="v"></param>
	/// <param name="z"></param>
	/// <returns></returns>
	public static Vector3 ChangeZ(this Vector3 v, float z) {
		return new Vector3(v.x, v.y, z);
	}

	/// <summary>
	/// Return a Vector3 with this vector's components as well as the supplied z component
	/// </summary>
	/// <param name="v"></param>
	/// <param name="z"></param>
	/// <returns></returns>
	public static Vector3 ChangeZ(this Vector2 v, float z) {
		return new Vector3(v.x, v.y, z);
	}
}
