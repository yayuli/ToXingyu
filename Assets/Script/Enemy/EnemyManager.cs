using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject prefab;  // 使用敌人预制件而不是枚举
        public float spawnDelay;  // 时间间隔
        public int maxCount;  // 最大数量
        public float spawnProbability;  // 出现概率
    }

    public List<EnemyType> enemyTypes;
    private Dictionary<string, float> nextSpawnTime;

    [SerializeField] private int maxEnemies = 100; // 最大敌人数量
    private int currentEnemies = 0;
    private Transform enemiesParent;

    public static EnemyManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        enemiesParent = new GameObject("Enemies").transform;
        nextSpawnTime = new Dictionary<string, float>();

        foreach (var enemyType in enemyTypes)
        {
            nextSpawnTime[enemyType.prefab.name] = Time.time + enemyType.spawnDelay;
        }
    }

    void SpawnEnemy(string prefabName)
    {
        Vector2? position = PositionManager.Instance.GetRandomPosition();
        if (position != null && currentEnemies < maxEnemies)
        {
            GameObject enemy = ObjectPool.Instance.SpawnFromPool(prefabName, position.Value, Quaternion.identity);
            if (enemy != null)
            {
                enemy.transform.SetParent(enemiesParent);
                currentEnemies++;
            }
        }
    }

    private void Update()
    {
        foreach (var enemyType in enemyTypes)
        {
            string prefabName = enemyType.prefab.name;
            if (Time.time >= nextSpawnTime[prefabName] && GetCount(prefabName) < enemyType.maxCount)
            {
                if (Random.Range(0f, 1f) <= enemyType.spawnProbability && currentEnemies < maxEnemies)
                {
                    SpawnEnemy(prefabName);
                    nextSpawnTime[prefabName] = Time.time + enemyType.spawnDelay;
                }
            }
        }
    }

    public void DestroyEnemy(GameObject enemy)
    {
        string prefabName = enemy.name.Replace("(Clone)", "").Trim();
        ObjectPool.Instance.ReturnToPool(prefabName, enemy);
        currentEnemies--;
    }

    private int GetCount(string prefabName)
    {
        // 计算当前活跃的指定敌人数量
        int count = 0;
        foreach (Transform child in enemiesParent)
        {
            if (child.name.StartsWith(prefabName))
                count++;
        }
        return count;
    }

    public void OnDestroyAllEnemies()
    {
        foreach (Transform enemy in enemiesParent)
        {
            GameObject enemyGameObject = enemy.gameObject;
            string prefabName = enemyGameObject.name.Replace("(Clone)", "").Trim();
            ObjectPool.Instance.ReturnToPool(prefabName, enemyGameObject);
        }
        currentEnemies = 0;  // 重置数量
    }
}
