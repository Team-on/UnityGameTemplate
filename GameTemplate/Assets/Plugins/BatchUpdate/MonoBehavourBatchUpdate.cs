using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoBehaviourBatchUpdate : MonoBehaviour, IBatchUpdate
{
    public abstract void BatchUpdate();

	protected virtual void OnEnable() {
		UpdateManager.RegisterSlicedUpdate(this, UpdateManager.UpdateMode.Always);
	}

	protected virtual void OnDisable() {
		UpdateManager.DeregisterSlicedUpdate(this);
	}
}
