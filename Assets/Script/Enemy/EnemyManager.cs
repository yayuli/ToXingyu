using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public string tag;  // Tag used in the object pool
        public float spawnDelay;  // Time between spawns
        public int maxCount;  // Max number of this type allowed at once
        public float spawnProbability;  // Probability of this type spawning
    }

    public List<EnemyType> enemyTypes;
    private Dictionary<string, float> nextSpawnTime;

    //to store the count of each type of enemy
    private Dictionary<string, int> enemyCountByType = new Dictionary<string, int>();

    [SerializeField] private int maxEnemies = 20; // Maximum number of enemies
    private int currentEnemies = 0;

    private Transform enemiesParent;

    public static EnemyManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        enemiesParent = new GameObject("Enemies").transform;
        nextSpawnTime = new Dictionary<string, float>();

        foreach (var enemyType in enemyTypes)
        {
            nextSpawnTime[enemyType.tag] = Time.time + enemyType.spawnDelay;
        }
    }

    private void Update()
    {
        foreach (var enemyType in enemyTypes)
        {
            if (Time.time >= nextSpawnTime[enemyType.tag] && GetCount(enemyType.tag) < enemyType.maxCount)
            {
                if (Random.Range(0f, 1f) <= enemyType.spawnProbability && currentEnemies < maxEnemies)
                {
                    SpawnEnemy(enemyType.tag);
                    nextSpawnTime[enemyType.tag] = Time.time + enemyType.spawnDelay;
                }
            }
        }
    }


    void SpawnEnemy(string tag)
    {
        Vector2? position = PositionManager.Instance.GetRandomPosition(); 
        if (position != null && currentEnemies < maxEnemies)
        {
            GameObject enemy = ObjectPool.Instance.SpawnFromPool(tag, position.Value, Quaternion.identity);
            if (enemy != null)
            {
                enemy.transform.SetParent(enemiesParent);
                currentEnemies++;

                //update the count when enemies are spawned/
                if (!enemyCountByType.ContainsKey(tag))
                {
                    enemyCountByType[tag] = 0;
                }
                enemyCountByType[tag]++;
            }
        }
    }


    public void DestroyEnemy(GameObject enemy)
    {
        ObjectPool.Instance.ReturnToPool(enemy.tag, enemy);
        currentEnemies--;  // Decrement the count of current enemies

        //update the count when enemies are destory/
        if(enemyCountByType.ContainsKey(enemy.tag))
        {
            enemyCountByType[enemy.tag]--;
        }
    }


    private int GetCount(string tag)
    {
        if(enemyCountByType.ContainsKey(tag))
        {
            return enemyCountByType[tag];

        }
        return 0;//if the tag is not in the count dictionary , return 0
    }

    /* 简单的示例，展示如何使用 GetCount 方法来调整游戏难度：以后可以根据需求添加功能
     * void UpdateDifficulty()
{
    int currentCount = GetCount("strongEnemy");  // 获取强敌的当前数量
    float playerHealth = player.GetHealthPercentage();  // 获取玩家的健康百分比

    if (playerHealth > 80% && currentCount < 5)
    {
        // 玩家健康状况良好且强敌较少，增加强敌的生成
        IncreaseSpawnRate("strongEnemy");
    }
    else if (playerHealth < 50%)
    {
        // 玩家健康状况较差，减少强敌的生成
        DecreaseSpawnRate("strongEnemy");
    }
}
*/
    public void OnDestroyAllEnemies()
    {
        foreach (Transform enemy in enemiesParent)
        {
            GameObject enemyGameObject = enemy.gameObject;
            ObjectPool.Instance.ReturnToPool(enemyGameObject.tag, enemyGameObject);
        }
        currentEnemies = 0;  // Reset count
    }
}
