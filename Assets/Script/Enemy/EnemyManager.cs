using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [Header("Enemy Spawn Settings")]
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject spawnWarning;
    [SerializeField] float spawnTime = 3f;
    [SerializeField] float spawnRadius = 1.5f;

    [Header("Boss")]
    [SerializeField] GameObject bossPrefab;
    [SerializeField] Transform bossSpawnPoint;

    //defining delegate a and events(for Weapen script)
    //public delegate void EnemyUpdateHandler();
    //public event EnemyUpdateHandler OnEnemyUpdated;

    private List<Vector2> spawnPosList = new List<Vector2>();
    private List<GameObject> enemyList = new List<GameObject>();
    private WaitForSeconds waitSpawnTime;
    private WaitForSeconds waitSpawnWarningTime = new WaitForSeconds(1f);
    private WaitForSeconds waitSpawnInterval = new WaitForSeconds(0.04f);
    private Coroutine spawnBossCoroutine;

    public List<GameObject> allEnemies = new List<GameObject>();


    void Awake()
    {
        Instance = this;
        waitSpawnTime = new WaitForSeconds(spawnTime);
    }

    public void OnEnable()
    {
        Invoke("StartSpawning", 2f);
    }
    void StartSpawning()
    {
        StartCoroutine(SpawnEnemy());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public IEnumerator SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("Enemy prefabs array is null or empty.");
            yield break;
        }
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Spawn points array is null or empty.");
            yield break;
        }

        yield return waitSpawnWarningTime;
        while (true)
        {
            if (WaveManager.Instance.WaveNum == 5 && spawnBossCoroutine == null)
            {
                spawnBossCoroutine = StartCoroutine(SpawnBoss());
            }
            StartCoroutine(SpawnEnemies(WaveManager.Instance.GetWaveEnemyCount()));
            yield return waitSpawnTime;
        }
    }

    public IEnumerator SpawnEnemies(int enemyNum)
    {
        spawnPosList.Clear();
        enemyList.Clear();
        for (int i = 0; i < enemyNum; i++)
        {
            spawnPosList.Add(spawnPoints[Random.Range(0, spawnPoints.Length)].position + Random.insideUnitSphere * spawnRadius);
            enemyList.Add(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
        }

        for (int i = 0; i < enemyNum; i++)
        {
            yield return waitSpawnInterval;
            ObjectPool.Release(spawnWarning, spawnPosList[i], Quaternion.identity);
            //AudioManager.Instance.PlayRandomSFX();
        }

        yield return waitSpawnWarningTime;

        for (int i = 0; i < enemyNum; i++)
        {
            GameObject enemy = ObjectPool.Release(enemyList[i], spawnPosList[i], Quaternion.identity);
            enemy.GetComponent<Enemy>().InitializeAttributes();
            allEnemies.Add(enemy);
        }
    }

    public IEnumerator SpawnBoss()
    {
        yield return waitSpawnWarningTime;
        ObjectPool.Release(spawnWarning, bossSpawnPoint.position, Quaternion.identity, Vector3.one * 2);
        yield return waitSpawnWarningTime;
        GameObject boss = ObjectPool.Release(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        allEnemies.Add(boss);
        // Additional boss setup can go here
    }

    public void ResetAndSpawnEnemies()
    {
        ClearAllEnemies();
        StartCoroutine(SpawnEnemy());
    }

    private void ClearAllEnemies()
    {
        foreach (GameObject enemy in allEnemies)
        {
            Destroy(enemy);
        }
        allEnemies.Clear();
    }

    public void RemoveEnemy(GameObject enemy)
    {
        allEnemies.Remove(enemy);
    }

    public void ClearAllEnemies(bool dropLoot = false)
    {
        for (int i = allEnemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = allEnemies[i].GetComponent<Enemy>();
            if (dropLoot)
            {
                enemy.DropLoot();
            }
            else
            {
                enemy.Die();
            }
        }
        StopAllCoroutines();
    }

    
}
