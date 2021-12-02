using System.Collections.Generic;
using UnityEngine;

public static class RandomEx {
	public static bool GetEventWithChance(int percent) {
		int number = UnityEngine.Random.Range(1, 101);
		return number <= percent;
	}

	public static void Shuffle<T>(this List<T> array) {
		int n = array.Count;
		while (n > 1) {
			int k = UnityEngine.Random.Range(0, --n);
			T temp = array[n];
			array[n] = array[k];
			array[k] = temp;
		}
	}

	public static void Shuffle<T>(this T[] array) {
		int n = array.Length;
		while (n > 1) {
			int k = UnityEngine.Random.Range(0, --n);
			T temp = array[n];
			array[n] = array[k];
			array[k] = temp;
		}
	}

	public static T Random<T>(this List<T> array) {
		if (array.Count == 0)
			return default(T);
		return array[UnityEngine.Random.Range(0, array.Count)];
	}

	public static T Random<T>(this T[] array) {
		if (array.Length == 0)
			return default(T);
		return array[UnityEngine.Random.Range(0, array.Length)];
	}

	public static Vector2 PointInCircle() {
		float angle = UnityEngine.Random.Range(0, 360);
		float radius = UnityEngine.Random.Range(Mathf.Epsilon, 1f);
		Vector2 randomPos = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));

		return randomPos;
	}

	public static Vector3 PointInSphere() {
		float u = UnityEngine.Random.Range(0f, 1f);
		float v = UnityEngine.Random.Range(0f, 1f);
		float radius = UnityEngine.Random.Range(Mathf.Epsilon, 1f);

		float theta = 2 * Mathf.PI * u;
		float phi = Mathf.Acos(2 * v - 1);

		float sinPhi = Mathf.Sin(phi);
		float cosPhi = Mathf.Cos(phi);
		float sinTheta = Mathf.Sin(theta);
		float cosTheta = Mathf.Cos(theta);

		float x = (radius * sinPhi * cosTheta);
		float y = (radius * sinPhi * sinTheta);
		float z = (radius * cosPhi);

		return new Vector3(x, y, z);
	}

	public static Vector2 PointInCircleNormalized() {
		return PointInCircle().normalized;
	}

	public static Vector3 PointInSphereNormalized() {
		return PointInSphere().normalized;
	}
}
