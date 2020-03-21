using yaSingleton.Utility;

namespace yaSingleton.Helpers {
    /// <summary>
    /// Singleton updater class. Instantiates a single MonoBehaviour and uses it to send Unity's events to all singletons. Has a sexy editor.
    /// </summary>
    public class SingletonUpdater : ExecutorBehavior {
        
        private static SingletonUpdater _updater;

        internal static SingletonUpdater Updater {
            get {
                if(_updater == null) {
                    _updater = Create<SingletonUpdater>("Singleton Updater", true);
                }

                return _updater;
            }
        }
    }
}