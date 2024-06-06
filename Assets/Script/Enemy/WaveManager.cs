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
    }

    // Get the count of enemies for the current wave
    public int GetWaveEnemyCount()
    {
        // Implement logic to determine number based on WaveNum
        return 5; // Default value for demo purposes
    }

    // Get the attributes for enemies based on current wave
    public float[] GetWaveAttributes()
    {
        // Return attributes like health, speed, etc.
        return new float[] { 100, 2, 10 }; // Health, speed, damage
    }
}
