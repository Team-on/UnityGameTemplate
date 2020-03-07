using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class RandomEx {
	public static void Shuffle<T>(List<T> array) {
		int n = array.Count;
		while (n > 1) {
			int k = Random.Range(0, --n);
			T temp = array[n];
			array[n] = array[k];
			array[k] = temp;
		}
	}

	public static void Shuffle<T>(T[] array) {
		int n = array.Length;
		while (n > 1) {
			int k = Random.Range(0, --n);
			T temp = array[n];
			array[n] = array[k];
			array[k] = temp;
		}
	}

	public static T GetRandom<T>(List<T> array) {
		return array[Random.Range(0, array.Count)];
	}

	public static T GetRandom<T>(T[] array) {
		return array[Random.Range(0, array.Length)];
	}
}
