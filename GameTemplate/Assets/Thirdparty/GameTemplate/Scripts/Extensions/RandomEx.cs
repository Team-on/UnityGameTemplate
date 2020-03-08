using System.Collections.Generic;

static class RandomEx {
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
		return array[UnityEngine.Random.Range(0, array.Count)];
	}

	public static T Random<T>(this T[] array) {
		return array[UnityEngine.Random.Range(0, array.Length)];
	}
}
