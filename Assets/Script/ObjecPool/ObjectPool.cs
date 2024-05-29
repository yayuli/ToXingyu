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

    private static ObjectPool _instance;
    public static ObjectPool Instance
    {
        get
        {
            if (_instance == null) Debug.LogError("ObjectPool instance not set");
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null)
        {
            //Debug.LogWarning("Multiple instances of ObjectPool found!");
            return;
        }
        _instance = this;
        InitializePools();
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

    public GameObject SpawnFromPool(string poolName, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(poolName))
        {
            //Debug.LogWarning("No pool with name: " + poolName);
            return null;
        }

        if (poolDictionary[poolName].Count == 0)
        {
            //Debug.Log("Expanding pool for: " + poolName);
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
            //Debug.LogError("No pool configuration found for prefab: " + poolName);
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
        if (!poolDictionary.ContainsKey(poolName))
        {
            //Debug.LogError("Invalid pool prefab name specified");
            return;
        }

        objectToReturn.SetActive(false);
        poolDictionary[poolName].Enqueue(objectToReturn);
    }
}
