using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public GameObject Prefab;
        public int Size;
        private Queue<GameObject> poolObjects;
        public Transform PoolParent;

        public void Initialize(Transform parent)
        {
            PoolParent = parent;
            poolObjects = new Queue<GameObject>();

            for (int i = 0; i < Size; i++)
            {
                GameObject obj = Instantiate(Prefab, PoolParent);
                obj.SetActive(false);
                poolObjects.Enqueue(obj);
            }
        }

        public GameObject GetPreparedObject(Vector3 position, Quaternion rotation, Vector3? localScale = null)
        {
            if (poolObjects.Count == 0)
            {
                Debug.LogWarning($"Expanding pool for: {Prefab.name}");
                ExpandPool(5);  // Expand pool by 5 additional objects
            }

            GameObject obj = poolObjects.Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            if (localScale.HasValue)
            {
                obj.transform.localScale = localScale.Value;
            }
            obj.SetActive(true);
            SFXManager.instance.PlaySFXPitched(1);
            return obj;
        }

        private void ExpandPool(int expandBy)
        {
            for (int i = 0; i < expandBy; i++)
            {
                GameObject obj = Instantiate(Prefab, PoolParent);
                obj.SetActive(false);
                poolObjects.Enqueue(obj);
            }
        }

        public void ReturnObjectToPool(GameObject objectToReturn)
        {
            objectToReturn.SetActive(false);
            poolObjects.Enqueue(objectToReturn);
        }
    }

    public static ObjectPool Instance { get; private set; }
    public List<Pool> Pools;
    private Dictionary<GameObject, Pool> prefabToPoolMap;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        prefabToPoolMap = new Dictionary<GameObject, Pool>();
        foreach (Pool pool in Pools)
        {
            Transform poolParent = new GameObject("Pool - " + pool.Prefab.name).transform;
            poolParent.parent = transform;
            pool.Initialize(poolParent);
            prefabToPoolMap[pool.Prefab] = pool;
        }
    }

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3? localScale = null)
    {
        if (Instance.prefabToPoolMap.TryGetValue(prefab, out Pool pool))
        {
            return pool.GetPreparedObject(position, rotation, localScale);
        }
        else
        {
            return null;
        }
    }

    public static void Return(GameObject prefab, GameObject objectToReturn)
    {
        if (Instance.prefabToPoolMap.TryGetValue(prefab, out Pool pool))
        {
            pool.ReturnObjectToPool(objectToReturn);
        }
        else
        {
            Destroy(objectToReturn);
        }
    }
}
