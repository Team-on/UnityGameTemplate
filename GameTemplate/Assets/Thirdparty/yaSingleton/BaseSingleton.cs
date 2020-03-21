using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using yaSingleton.Helpers;
using yaSingleton.Utility;

namespace yaSingleton {
    /// <summary>
    /// Base class for singletons. Contains method stubs and the Create method. Use this to create custom Singleton flavors.
    /// If you're looking to create a singleton, inherit Singleton or LazySingleton.
    /// </summary>
    public abstract class BaseSingleton : PreloadedScriptableObject {

        protected static SingletonUpdater Updater {
            get { return SingletonUpdater.Updater; }
        }
        
        internal abstract void CreateInstance();
        
        // Reduce the visibility of OnEnable; Inheritors should override Initialize instead.
        private new void OnEnable() {
            base.OnEnable();
#if UNITY_EDITOR
            if(!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) {
                return;
            }
#endif
            AllSingletons.Add(this);
        }
        
        // Reduce the visibility of OnDisable; Inheritors should override Deinitialize instead.
        private new void OnDisable() {
            base.OnDisable();
        }

        protected virtual void Initialize() {
            Updater.DestroyEvent += Deinitialize;

            Updater.FixedUpdateEvent += OnFixedUpdate;
            Updater.UpdateEvent += OnUpdate;
            Updater.LateUpdateEvent += OnLateUpdate;

            Updater.ApplicationFocusEvent += OnApplicationFocus;
            Updater.ApplicationPauseEvent += OnApplicationPause;
            Updater.ApplicationQuitEvent += OnApplicationQuit;

            Updater.DrawGizmosEvent += OnDrawGizmos;
            Updater.PostRenderEvent += OnPostRender;
            Updater.PreCullEvent += OnPreCull;
            Updater.PreRenderEvent += OnPreRender;
        }
        
        protected virtual void Deinitialize() { }
        
        #region UnityEvents

        public virtual void OnFixedUpdate() { }
        public virtual void OnUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnApplicationFocus() { }
        public virtual void OnApplicationPause() { }
        public virtual void OnApplicationQuit() { }
        public virtual void OnDrawGizmos() { }
        public virtual void OnPostRender() { }
        public virtual void OnPreCull() { }
        public virtual void OnPreRender() { }

        #endregion

        #region Coroutines

        /// <summary>
        ///   <para>Starts a coroutine.</para>
        /// </summary>
        /// <param name="routine">The coroutine</param>
        protected Coroutine StartCoroutine(IEnumerator routine) {
            return Updater.StartCoroutine(routine);
        }

        /// <summary>
        ///   <para>Starts a coroutine named methodName.</para>
        /// </summary>
        /// <param name="methodName">Name of coroutine.</param>
        protected Coroutine StartCoroutine(string methodName) {
            return Updater.StartCoroutine(methodName);
        }

        /// <summary>
        ///   <para>Stops the first coroutine named methodName, or the coroutine stored in routine running on this behaviour.</para>
        /// </summary>
        /// <param name="routine">Name of the function in code, including coroutines.</param>
        protected void StopCoroutine(Coroutine routine) {
            Updater.StopCoroutine(routine);
        }
        
        /// <summary>
        ///   <para>Stops the first coroutine named methodName, or the coroutine stored in routine running on this behaviour.</para>
        /// </summary>
        /// <param name="routine">Name of the function in code, including coroutines.</param>
        protected void StopCoroutine(IEnumerator routine) {
            Updater.StopCoroutine(routine);
        }
        
        /// <summary>
        ///   <para>Stops the first coroutine named methodName, or the coroutine stored in routine running on this behaviour.</para>
        /// </summary>
        /// <param name="methodName">Name of coroutine.</param>
        protected void StopCoroutine(string methodName) {
            Updater.StopCoroutine(methodName);
        }
        
        /// <summary>
        ///   <para>Stops all coroutines running on this behaviour.</para>
        /// </summary>
        protected void StopAllCoroutines() {
            Updater.StopAllCoroutines();
        }
        
        #endregion

        public static readonly List<BaseSingleton> AllSingletons = new List<BaseSingleton>();

        protected static T GetOrCreate<T>() where T : BaseSingleton {
            var instance = AllSingletons.FirstOrDefault(s => s.GetType() == typeof(T)) as T;

            instance = instance ? instance : CreateInstance<T>();

            instance.Initialize();

            return instance;
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeSingletons() {
            foreach(var singleton in AllSingletons) {
                singleton.CreateInstance();
            }
        }
    }
}