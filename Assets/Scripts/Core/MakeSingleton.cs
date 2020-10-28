using UnityEngine;

namespace AbilitySystem.Core
{

	public abstract class MakeSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T _instance = null;

		public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        string objName = "_" + typeof(T).Name;
                        GameObject go = GameObject.Find(objName);
                        if (go == null)
                        {
                            go = new GameObject
                            {
                                name = objName
                            };
                        }
                        _instance = go.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                throw new System.Exception("Can't get singleton component in Awake phase!");
            }
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }

		public static bool IsSingletonComponentActive()
        {
            return _instance != null;
        }
    }
}
