using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //[SerializeField] GameObject enemyPrefab;
    [SerializeField] private float timeBetweenSpawns = 2f;
    [SerializeField] private int maxEnemies = 20;// Maximum number of enemies
    private float currentTimeBetweenSpawns;
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
    }

    private void Update()
    {
        currentTimeBetweenSpawns -= Time.deltaTime;
        if (currentTimeBetweenSpawns <= 0 && currentEnemies < maxEnemies)
        {
            SpawnEnemy();
            currentTimeBetweenSpawns = timeBetweenSpawns;
        }
    }

    private Vector2 RandomPosition()
    {
        return new Vector2(Random.Range(-16, 16), Random.Range(-8, 8));
    }

    void SpawnEnemy()
    {
        GameObject enemy = ObjectPool.Instance.SpawnFromPool("Enemy", RandomPosition(), Quaternion.identity);
        if (enemy != null)
        {
            enemy.transform.SetParent(enemiesParent);
            currentEnemies++;
        }
    }

    public void DestroyEnemy(GameObject enemy)
    {
        ObjectPool.Instance.ReturnToPool("Enemy", enemy);
        currentEnemies--;
        if (currentEnemies < maxEnemies)
        {
            SpawnEnemy(); // Consider spawning another enemy if below max limit
        }
    }

    //玩家死亡或关卡重置，destroy all enemies
    public void OnDestroyAllEnemies()
    {
        foreach (Transform enemy in enemiesParent)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}
