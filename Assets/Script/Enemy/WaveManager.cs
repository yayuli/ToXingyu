using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    public int WaveNum { get; private set; } = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this method to advance to the next wave
    public void NextWave()
    {
        WaveNum++;
        UpdateAllEnemies();  // 
    }

    // Get the count of enemies for the current wave
    public int GetWaveEnemyCount()
    {
        // Implement logic to determine number based on WaveNum
        return 5 + (WaveNum - 1) * 10; 
    }

    //not need
    public float GetWaveMultiplier()
    {
        // 返回一个基于当前波次计算的乘数，可能用于调整游戏难度等
        return 1.0f + (WaveNum - 1) * 0.1f;  // 每波增加10%的难度或奖励
    }

    private void UpdateAllEnemies()
    {
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            enemy.InitializeAttributes();  // 强制每个敌人根据新波数重新计算属性
        }
    }
}
