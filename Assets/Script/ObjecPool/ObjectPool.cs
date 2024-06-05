using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string name;
        public GameObject prefab;
        public int size;
    }

    private const int ExpandAmount = 50;
    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    // 在这里定义一个字典来存储预制体引用
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();


    private static ObjectPool _instance;
    public static ObjectPool Instance
    {
        get
        {
           // if (_instance == null) Debug.LogError("ObjectPool instance not set");
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null)
        {
           // Debug.LogWarning("Multiple instances of ObjectPool found!");
            return;
        }
        _instance = this;
        InitializePools();

        LoadPrefabs();
    }

    void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            CreatePool(pool.name, pool.prefab, pool.size);
        }
    }

    public void CreatePool(string poolName, GameObject prefab, int size)
    {
        if (!poolDictionary.ContainsKey(poolName))
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }

            poolDictionary.Add(poolName, objectQueue);
        }
    }

    // 加载所有预制体到字典
    private void LoadPrefabs()
    {
        GameObject[] allPrefabs = Resources.LoadAll<GameObject>("Enemies");
        foreach (GameObject prefab in allPrefabs)
        {
            if (!prefabDictionary.ContainsKey(prefab.name))
            {
                prefabDictionary.Add(prefab.name, prefab);
            }
        }
    }

    // 通过名称查找预制体
    private GameObject FindPrefabByName(string prefabName)
    {
        if (prefabDictionary.TryGetValue(prefabName, out GameObject prefab))
        {
            return prefab;
        }
        Debug.LogWarning($"Prefab not found for name: {prefabName}");
        return null;
    }

    public GameObject SpawnFromPool(string poolName, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(poolName))
        {
            Debug.LogWarning($"No pool with name: {poolName} found. Attempting to create one.");
            var prefab = FindPrefabByName(poolName);
            if (prefab != null)
            {
                CreatePool(poolName, prefab, ExpandAmount);
            }
            else
            {
                Debug.LogError($"Failed to create pool for: {poolName} because the prefab could not be found.");
                return null;
            }
        }

        if (poolDictionary[poolName].Count == 0)
        {
            Debug.Log($"Expanding pool for: {poolName}");
            ExpandPool(poolName, ExpandAmount);
        }

        GameObject objectToSpawn = poolDictionary[poolName].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }


    private void ExpandPool(string poolName, int additionalCount)
    {
        var pool = pools.Find(p => p.name == poolName);
        if (pool == null)
        {
           // Debug.LogError($"No pool configuration found for prefab: {poolName}");
            return;
        }

        for (int i = 0; i < additionalCount; i++)
        {
            GameObject obj = Instantiate(pool.prefab);
            obj.SetActive(false);
            poolDictionary[poolName].Enqueue(obj);
        }
    }

    public void ReturnToPool(string poolName, GameObject objectToReturn)
    {
        if (poolDictionary.ContainsKey(poolName))
        {
            objectToReturn.SetActive(false);
            objectToReturn.transform.SetParent(null);  // 从父对象中移除
            poolDictionary[poolName].Enqueue(objectToReturn);
        }
        else
        {
            Debug.LogError("No pool found for: " + poolName);
            Destroy(objectToReturn);  // 如果没有相应的池，销毁对象
        }
    }

}
