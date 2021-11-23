using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UpdateManager {
    public enum UpdateMode { BucketA, BucketB, Always }

    static readonly HashSet<IBatchUpdate> _slicedUpdateBehavioursBucketA = new HashSet<IBatchUpdate>();
    static readonly HashSet<IBatchUpdate> _slicedUpdateBehavioursBucketB = new HashSet<IBatchUpdate>();
    static bool _isCurrentBucketA;

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize() {
        PlayerLoopInterface.InsertSystemBefore(typeof(UpdateManager), UpdateSystem, typeof(UnityEngine.PlayerLoop.Update.ScriptRunBehaviourUpdate));
    }

    private static void UpdateSystem() {
        var targetUpdateFunctions = _isCurrentBucketA ? _slicedUpdateBehavioursBucketA : _slicedUpdateBehavioursBucketB;
        foreach (var slicedUpdateBehaviour in targetUpdateFunctions) {
            slicedUpdateBehaviour.BatchUpdate();
        }
        _isCurrentBucketA = !_isCurrentBucketA;
    }

    #region Register
    public static void RegisterSlicedUpdate(IBatchUpdate slicedUpdateBehaviour, UpdateMode updateMode) {
        if (updateMode == UpdateMode.Always) {
            _slicedUpdateBehavioursBucketA.Add(slicedUpdateBehaviour);
            _slicedUpdateBehavioursBucketB.Add(slicedUpdateBehaviour);
        }
        else {
            var targetUpdateFunctions = updateMode == UpdateMode.BucketA ? _slicedUpdateBehavioursBucketA : _slicedUpdateBehavioursBucketB;
            targetUpdateFunctions.Add(slicedUpdateBehaviour);
        }
    }

    public static void DeregisterSlicedUpdate(IBatchUpdate slicedUpdateBehaviour) {
        _slicedUpdateBehavioursBucketA.Remove(slicedUpdateBehaviour);
        _slicedUpdateBehavioursBucketB.Remove(slicedUpdateBehaviour);
    }
    #endregion
}
