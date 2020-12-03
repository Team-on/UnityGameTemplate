using UnityEngine;

namespace LightGive
{
	public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
	{
		public bool isDontDestroy;

		private static T instance;
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = (T)FindObjectOfType(typeof(T));
					if (instance == null)
					{
						Debug.LogError(typeof(T) + " is nothing");
					}
				}
				return instance;
			}
		}

		protected virtual void Awake()
		{
			if (CheckInstance() && isDontDestroy) {
				//DontDestroyOnLoad(this.gameObject);
			}
		}

		protected bool CheckInstance()
		{
			if (this == Instance) { return true; }
			Destroy(this);
			return false;
		}
	}
}