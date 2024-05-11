using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    private LootItem lootData;

    public void Initialize(LootItem data)
    {
        lootData = data;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player picked up " + lootData.itemName);
            Destroy(gameObject);
        }
    }
}
