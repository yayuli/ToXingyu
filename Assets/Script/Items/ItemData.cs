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

    //weapon
    public int attackPower;
    public float range;
    public int damage;
    public float speed;
    public float cooldown;

    public int cost;

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
        ShieldPotion,
        RangedWeapon,
        MeleeWeapon
    }
}
