using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite itemIcon;
    public int effectMagnitude;
    public float duration; // Use 0 for permanent effects
    public ItemType itemType;

    public enum ItemType
    {
        HealthPotion,
        EnergyDrink,
        StrengthElixir,
        AgilityBoots,
        WisdomScroll,
        LuckCharm,
        MemoryCrystal,
        RegenerationAmulet,
        ShieldPotion
    }
}
