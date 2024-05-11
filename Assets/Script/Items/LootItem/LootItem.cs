using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// define what kind of loot can be dropped, this allows for easy customization and expansion of loot types
/// </summary>
[CreateAssetMenu(fileName = "New Loot Item", menuName = "Loot/Loot Item")]
public class LootItem : ScriptableObject
{
    public GameObject itemPrefab;
    public string itemName;
    public Sprite itemIcon;
    public int experienceValue;//can change or add additional loot feature
}
