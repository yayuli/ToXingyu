using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;

    public void Initialize(ItemData data)
    {
        this.itemData = data;  // Initialize the item with its data
    }

    public void UseItem(Player player)
    {
        if (itemData == null)
        {
            Debug.LogError("ItemData is not set for this item.");
            return;
        }

        switch (itemData.itemType)
        {
            case ItemData.ItemType.HealthPotion:
                player.attributes.ModifyHealth(itemData.effectMagnitude);
                break;
            case ItemData.ItemType.EnergyDrink:
                player.attributes.Energy += itemData.effectMagnitude;
                break;
            case ItemData.ItemType.StrengthElixir:
                StartCoroutine(player.ApplyTempStatBoost(() => player.attributes.Strength += itemData.effectMagnitude,
                                                         () => player.attributes.Strength -= itemData.effectMagnitude,
                                                         itemData.duration));
                break;
            case ItemData.ItemType.AgilityBoots:
                player.attributes.Dexterity += itemData.effectMagnitude;
                break;
            case ItemData.ItemType.WisdomScroll:
                player.attributes.Intelligence += itemData.effectMagnitude;
                break;
            case ItemData.ItemType.LuckCharm:
                StartCoroutine(player.ApplyTempStatBoost(() => player.attributes.Luck += itemData.effectMagnitude,
                                                         () => player.attributes.Luck -= itemData.effectMagnitude,
                                                         itemData.duration));
                break;
            case ItemData.ItemType.MemoryCrystal:
                player.attributes.MemoryPoints += itemData.effectMagnitude;
                break;
            case ItemData.ItemType.RegenerationAmulet:
                StartCoroutine(player.ApplyTempStatBoost(() => player.attributes.RegenerationRate += itemData.effectMagnitude,
                                                         () => player.attributes.RegenerationRate -= itemData.effectMagnitude,
                                                         itemData.duration));
                break;
            case ItemData.ItemType.ShieldPotion:
                StartCoroutine(player.ApplyTempStatBoost(() => player.attributes.Willpower += itemData.effectMagnitude,
                                                         () => player.attributes.Willpower -= itemData.effectMagnitude,
                                                         itemData.duration));
                break;
        }
    }
}

