using System.Collections.Generic;
using AbilitySystem.Core;
using UnityEngine;

namespace AbilitySystem.Manager
{

    public class PoolingManager : MakeSingleton<PoolingManager>
    {
        [SerializeField] Transform _activeObjectsParent;

        readonly Dictionary<string, Queue<GameObject>> _pool = new Dictionary<string, Queue<GameObject>>();


        public delegate void OnArrowEvent();
        public static OnArrowEvent OnArrow;


        GameObject Create(GameObject obj)
        {
            GameObject newObject = Instantiate(obj);
            newObject.name = obj.name;
            return newObject;
        }

       
        void AssignTransform(Transform obj, Vector3 position, Quaternion rotation)
        {
            obj.SetParent(_activeObjectsParent);
            obj.position = position;
            obj.rotation = rotation;
        }

        
        public void Add(GameObject obj)
        {
            obj.transform.SetParent(transform);

            if (!_pool.ContainsKey(obj.name))
            {
                _pool.Add(obj.name, new Queue<GameObject>());
            }

            _pool[obj.name].Enqueue(obj);

            obj.SetActive(false);
        }

       
        public GameObject Get(GameObject obj, Vector3 position, Quaternion rotation)
        {
            GameObject newObject;

            if (!_pool.ContainsKey(obj.name))
            {
                newObject = Create(obj);
            }
            else if (_pool[obj.name].Count == 0)
            {
                newObject = Create(obj);
            }
            else
            {
                newObject = _pool[obj.name].Dequeue();
                newObject.SetActive(true);
            }

            AssignTransform(newObject.transform, position, rotation);

            return newObject;
        }
    }
}
