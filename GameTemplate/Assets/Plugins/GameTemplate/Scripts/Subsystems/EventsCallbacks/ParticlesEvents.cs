using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesEvents : MonoBehaviour
{
	public VoidParticlesEvent onParticleTrigger;
	public GameObjectParticlesEvent onParticleCollision;
	public VoidParticlesEvent onParticleSystemStopped;

	[SerializeField] ParticleSystem particles;

#if UNITY_EDITOR
	private void OnValidate() {
		Start();
	}
#endif

	private void Start() {
		if (!particles)
			particles = GetComponent<ParticleSystem>();
	}

	private void OnParticleTrigger() {
		onParticleTrigger?.Invoke(particles);
	}

	private void OnParticleCollision(GameObject other) {
		onParticleCollision?.Invoke(particles, other);

	}

	private void OnParticleSystemStopped() {
		onParticleSystemStopped?.Invoke(particles);

	}
}
