using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance { get; private set; }
    public bool IsAutoShootingEnabled { get; private set; }

    void Awake()
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

    public void SetShootingMode(bool isAuto)
    {
        IsAutoShootingEnabled = isAuto;
        Debug.Log("Shooting mode set to: " + (isAuto ? "Auto" : "Manual"));
    }
}