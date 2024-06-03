
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject prefab;  // 敌人预制体
        public float spawnDelay;  // 产生间隔
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
        if (Instance == null)
        {
            Instance = this;
            enemiesParent = new GameObject("Enemies").transform;
            nextSpawnTime = new Dictionary<string, float>();
            gameStartTime = Time.time;
        }
        else
        {
            Destroy(gameObject); // 避免重复实例
        }
    }

    void Start()
    {
        foreach (var enemyType in enemyTypes)
        {
            nextSpawnTime[enemyType.prefab.name] = Time.time + enemyType.spawnDelay;
            ObjectPool.Instance.CreatePool(enemyType.prefab.name, enemyType.prefab, enemyType.baseMaxCount);
        }
    }

    void Update()
    {
        float elapsedTime = Time.time - gameStartTime;
        AdjustMaxEnemiesBasedOnTime(elapsedTime);

        foreach (var enemyType in enemyTypes)
        {
            AttemptToSpawnEnemy(enemyType, elapsedTime);
        }
    }

    private void AdjustMaxEnemiesBasedOnTime(float elapsedTime)
    {
        maxEnemies = 100 + Mathf.FloorToInt(elapsedTime / 30f) * 10;
    }

    private void AttemptToSpawnEnemy(EnemyType enemyType, float elapsedTime)
    {
        if (Time.time >= nextSpawnTime[enemyType.prefab.name] && currentEnemies < maxEnemies)
        {
            if (Random.Range(0f, 1f) <= GetSpawnProbability(enemyType, elapsedTime))
            {
                SpawnEnemy(enemyType.prefab.name);
                nextSpawnTime[enemyType.prefab.name] = Time.time + enemyType.spawnDelay;
            }
        }
    }

    void SpawnEnemy(string prefabName)
    {
        Vector2? position = PositionManager.Instance.GetRandomPosition(true);
        if (position != null)
        {
            GameObject enemy = ObjectPool.Instance.SpawnFromPool(prefabName, position.Value, Quaternion.identity);
            if (enemy != null)
            {
                enemy.transform.SetParent(enemiesParent);
                currentEnemies++;
            }
        }
    }

    private float GetSpawnProbability(EnemyType enemyType, float elapsedTime)
    {
        return Mathf.Clamp01(enemyType.baseSpawnProbability + (elapsedTime / 600f));  // 动态增加概率
    }

    public void DestroyEnemy(GameObject enemy)
    {
        string prefabName = enemy.name.Replace("(Clone)", "").Trim();
        ObjectPool.Instance.ReturnToPool(prefabName, enemy);
        currentEnemies--;
    }

    public void OnDestroyAllEnemies()
    {
        foreach (Transform enemy in enemiesParent)
        {
            DestroyEnemy(enemy.gameObject);
        }
        currentEnemies = 0;
    }
}
