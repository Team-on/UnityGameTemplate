using NaughtyAttributes;
using UnityEngine;

public class DropOnDestroy : MonoBehaviour {
	[Header("Drop")]
	[Space]
	[SerializeField] Transform[] positions = null;
	[SerializeField] GameObject[] drops = null;
	[SerializeField] byte[] chance = null;

	[Header("Drop forces")]
	[Space]
	[SerializeField] [MinMaxSlider(0, 10)] Vector2 dropForce = new Vector2(1, 3);

	[Header("This refs")]
	[Space]
	[SerializeField] Rigidbody2D rb = null;

#if UNITY_EDITOR
	private void OnValidate() {
		if (rb == null)
			rb = GetComponent<Rigidbody2D>();
	}
#endif

	// Called in Health.cs by BroadcastMessage("Die")
	void Die() {
		int usedPos = 0;
		positions.Shuffle();

		for (byte i = 0; i < drops.Length && i < chance.Length; ++i) {
			if (RandomEx.GetEventWithChance(chance[i])) {
				if (drops[i] == null) {
					Debug.LogError($"Can't spawn {drops[i]}, prefab is null");
					continue;
				}

				GameObject go = Instantiate(drops[i], positions[usedPos].position, Quaternion.Euler(0, 0, Random.Range(0, 360)), transform.parent);

				Rigidbody2D objectRb = go.GetComponent<Rigidbody2D>();
				if (objectRb) {
					objectRb.velocity += rb.velocity;
					objectRb.velocity += (Vector2)positions[usedPos].localPosition.normalized * dropForce.GetRandomValue();
					objectRb.angularVelocity += rb.angularVelocity;
				}

				++usedPos;
			}
		}
	}
}
