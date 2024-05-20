using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab; // 池中物品的预制件
        public int size;//initial pool size
        
    }
    public int expandBy = 20;//pool expand size
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public static ObjectPool Instance;


    void Awake()
    {
        Instance = this;
        InitializePools();
    }

    void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }

            poolDictionary.Add(pool.prefab.name, objectQueue);
        }
    }


    public GameObject SpawnFromPool(string prefabName, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(prefabName))
        {
            Debug.LogWarning("No object available in the pool and cannot expand: " + prefabName);
            return null;
        }

        if (poolDictionary[prefabName].Count == 0)
        {
            Debug.Log("Expanding pool for: " + prefabName);
            ExpandPool(prefabName, expandBy);
        }

        GameObject objectToSpawn = poolDictionary[prefabName].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    private void ExpandPool(string prefabName, int additionalCount)
    {
        var pool = pools.Find(p => p.prefab.name == prefabName);
        if (pool == null)
        {
            Debug.LogError("No pool configuration found for prefab: " + prefabName);
            return;
        }

        for (int i = 0; i < additionalCount; i++)
        {
            GameObject obj = Instantiate(pool.prefab);
            obj.SetActive(false);
            poolDictionary[prefabName].Enqueue(obj);
        }
    }


    public void ReturnToPool(string prefabName, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(prefabName))
        {
            Debug.LogError("Invalid pool prefab name specified");
            return;
        }

        objectToReturn.SetActive(false);
        poolDictionary[prefabName].Enqueue(objectToReturn);
    }
}
