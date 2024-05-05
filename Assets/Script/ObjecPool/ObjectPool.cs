using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;//pool Gamejects Tag
        public GameObject prefab;//pool gameObject
        public int size;//initial pool size
        public int expandBy = 20;//pool expand size
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public static ObjectPool Instance;


    void Awake()
    {
        Instance = this;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        //
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectQueue);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag) || poolDictionary[tag].Count == 0)
        {
            Debug.LogWarning("No object available in the pool and cannot expand: " + tag);
            return null;
        }

        if (poolDictionary[tag].Count ==0)
        {
            Debug.Log("expanding pool for:" + tag);
            ExpandPool(tag, pools.Find(p => p.tag == tag).expandBy);
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    private void ExpandPool(string tag, int additionalCount)
    {
        Pool pool = pools.Find(p => p.tag == tag);
        if(pool ==null)
        {
            Debug.LogError("no pool configuration found for tag:" + tag);
            return;
        }

        for (int i = 0; i < additionalCount; i++)
        {
            GameObject obj = Instantiate(pool.prefab);
            obj.SetActive(false);
            poolDictionary[tag].Enqueue(obj);
        }
    }
    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError("Invalid pool tag specified");
            return;
        }

        objectToReturn.SetActive(false);
        poolDictionary[tag].Enqueue(objectToReturn);
    }

}
