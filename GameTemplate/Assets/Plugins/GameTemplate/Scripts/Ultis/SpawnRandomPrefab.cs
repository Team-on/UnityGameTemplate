using UnityEngine;

public class SpawnRandomPrefab : MonoBehaviour {
	[SerializeField] GameObject[] prafabs;

	void Start() {
		if (prafabs != null && prafabs.Length != 0) {
			Instantiate(prafabs.Random(), transform.position, Quaternion.identity, transform);
		}
		Destroy(this);
	}
}
