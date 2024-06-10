using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    public string itemName;
    private GameManager gameManager;

    void Start()
    {
        // 尝试获取GameManager的实例
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the trigger.");
            if (gameManager != null)
            {
                gameManager.RefreshCurrentLevel();
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("GameManager is null.");
            }
        }
    }

}
