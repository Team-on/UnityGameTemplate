using System;
using UnityEngine;
using Object = UnityEngine.Object;

public static class PlayerLoopQuitChecker {
    public static event Action GameQuitCallback;

    [RuntimeInitializeOnLoadMethod]
    static void Initialize() {
        var quitCheckContainer = new GameObject("[Quit Checker]") {
            hideFlags = HideFlags.HideAndDontSave
        };
        quitCheckContainer.AddComponent<QuitCheckerRunner>();
        Object.DontDestroyOnLoad(quitCheckContainer);
    }

    private class QuitCheckerRunner : MonoBehaviour {
        private void OnApplicationQuit() {
            GameQuitCallback?.Invoke();
        }
    }

}