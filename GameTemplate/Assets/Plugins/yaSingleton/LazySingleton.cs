namespace yaSingleton {
    /// <summary>
    /// Singleton class. It'll be lazy-initialized when first accessed.
    /// Inherit by passing the inherited type (e.g. class GameManager : LazySingleton&lt;GameManager&gt;)
    /// </summary>
    /// <typeparam name="TSingleton">The Inherited Singleton's Type</typeparam>
    public abstract class LazySingleton<TSingleton> : BaseSingleton where TSingleton : BaseSingleton {
        public static TSingleton Instance {
            get { return Initializer<TSingleton>.LazyInstance; }
        }

        /// <summary>
        /// No need to manually create instances of LazySingletons
        /// </summary>
        internal override void CreateInstance() { }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class Initializer<T> where T : BaseSingleton {
            static Initializer() { }

            internal static readonly T LazyInstance = GetOrCreate<T>();
        }
    }
}