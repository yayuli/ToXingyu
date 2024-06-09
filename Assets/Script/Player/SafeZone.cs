using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //EnemyManager.Instance.DisableEnemies();
        }
    }

    private void OnYriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //EnemyManager.Instance.EnableEnemies();
        }
    }
}
