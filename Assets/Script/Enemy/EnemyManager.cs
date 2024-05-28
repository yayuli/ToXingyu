using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject prefab;  // 使用敌人预制件而不是枚举
        public float spawnDelay;  // 时间间隔
        public int baseMaxCount;  // 基础最大数量
        public float baseSpawnProbability;  // 基础出现概率
    }

    public List<EnemyType> enemyTypes;
    private Dictionary<string, float> nextSpawnTime;

    [SerializeField] private int maxEnemies = 100; // 最大敌人数量
    private int currentEnemies = 0;
    private Transform enemiesParent;

    private float gameStartTime; // 游戏开始时间

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

            // 创建对象池
            ObjectPool.Instance.CreatePool(enemyType.prefab.name, enemyType.prefab, enemyType.baseMaxCount);
        }

        gameStartTime = Time.time; // 记录游戏开始时间
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
                Debug.Log($"Spawned enemy: {prefabName} at {position.Value}. Total enemies: {currentEnemies}");
            }
            else
            {
                Debug.LogWarning($"Failed to spawn enemy from pool: {prefabName}");
            }
        }
        else
        {
            Debug.LogWarning($"Failed to find spawn position or reached max enemies: {maxEnemies}");
        }
    }

    private void Update()
    {
        float elapsedTime = Time.time - gameStartTime; // 计算游戏运行时间

        // 动态调整 maxEnemies，根据游戏运行时间增加
        maxEnemies = 100 + Mathf.FloorToInt(elapsedTime / 30f) * 10; // 每30秒增加10个最大敌人

        foreach (var enemyType in enemyTypes)
        {
            string prefabName = enemyType.prefab.name;
            int maxCount = GetMaxCount(enemyType, elapsedTime);
            float spawnProbability = GetSpawnProbability(enemyType, elapsedTime);
            int currentCount = GetCount(prefabName);

            if (Time.time >= nextSpawnTime[prefabName] && currentCount < maxCount)
            {
                if (Random.Range(0f, 1f) <= spawnProbability && currentEnemies < maxEnemies)
                {
                    Debug.Log($"Attempting to spawn {prefabName}. Current count: {currentCount}, Max count: {maxCount}, Spawn probability: {spawnProbability}");
                    SpawnEnemy(prefabName);
                    nextSpawnTime[prefabName] = Time.time + enemyType.spawnDelay;
                }
                else
                {
                    Debug.Log($"Spawn probability check failed for {prefabName}. Random value: {Random.Range(0f, 1f)}, Spawn probability: {spawnProbability}");
                }
            }
            else
            {
                Debug.Log($"Spawn condition not met for {prefabName}. Next spawn time: {nextSpawnTime[prefabName]}, Current time: {Time.time}, Current count: {currentCount}, Max count: {maxCount}");
            }
        }
    }

    public void DestroyEnemy(GameObject enemy)
    {
        string prefabName = enemy.name.Replace("(Clone)", "").Trim();
        ObjectPool.Instance.ReturnToPool(prefabName, enemy);
        currentEnemies--;
        Debug.Log($"Destroyed enemy: {prefabName}. Total enemies: {currentEnemies}");
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
        Debug.Log("All enemies destroyed. Total enemies: 0");
    }

    private int GetMaxCount(EnemyType enemyType, float elapsedTime)
    {
        return enemyType.baseMaxCount + Mathf.FloorToInt(elapsedTime / 60f);  // 每分钟增加一次
    }

    private float GetSpawnProbability(EnemyType enemyType, float elapsedTime)
    {
        return Mathf.Clamp01(enemyType.baseSpawnProbability + (elapsedTime / 600f));  // 每10分钟增加一次
    }
}
