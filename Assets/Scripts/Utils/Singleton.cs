using UnityEngine;

namespace Utils
{
	public abstract class Singleton<T> : MonoBehaviour
		where T : MonoBehaviour
	{
		public static T Instance
		{
			get
			{
				if (_applicationIsNotQuitting && !_instance)
				{
					_instance = FindObjectOfType<T>();
				}

				return _instance;
			}
		}

		public static K CastInstance<K>() where K : T => (K)Instance;

		private static T _instance;
		private static bool _applicationIsNotQuitting = true;

		protected virtual void Awake()
		{
			if (_instance)
			{
				Destroy(gameObject);
			}
			else
			{
				_instance = this as T;
			}
		}

		protected virtual void OnDestroy()
		{
			if (_instance == this)
			{
				_applicationIsNotQuitting = false;
			}
		}
	}
}