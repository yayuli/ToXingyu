using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// this can defines possible drops for different kind of enemies
/// </summary>
[CreateAssetMenu(fileName = "New Loot Table", menuName = "Loot/loot Table")]
public class LootTable : ScriptableObject
{
    public LootItem[] items;
    public int GetRandomItemIndex()
    {
        return Random.Range(0, items.Length);
    }
}