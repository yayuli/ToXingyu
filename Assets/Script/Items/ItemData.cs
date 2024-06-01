using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite itemIcon;
    public int effectMagnitude;
    public float duration; // Use 0 for permanent effects

    [Header("ItemType")]
    public ItemType itemType;

    [Header("player attri")]
    public int maxHealth = 0;
    public int healthRegeRate = 0;
    public int damageFactor = 0;
    public int attackRangeFactor = 0;
    public int armor = 0;
    public int criticalRate = 0;
    public int criticalDamage = 0;
    public int attackSpeed = 0;
    public int dodgeRate = 0;
    public int moveSpeedFactor = 0;
    public int pickUpRangeFactor = 0;

    [Header("Weapon")]
    public GameObject prefab;
    public int attackPower;
    public float speed;
    public int damage;  // 可以对应于 damage
    public float cooldown;
    public float range;
    public int level = 1;  // 新增武器等级
    public Color weaponColor = Color.white;  // 武器默认颜色

    public int cost;

    public enum ItemType
    {
        HealthPotion,
        ArmorSet,
        SpeedBoots,
        PrecisionAmulet,
        StrengthElixir,
        RegenerationRing,
        AgilityGloves,
        RegenerationAmulet,
        VisionTelescope,
        EnergyDrink,
        LuckyCharm,

        //others
        RangedWeapon,
        MeleeWeapon,
        Bomb
    }

    [System.Serializable]
    public class PlayerAttributes
    {
        
    }
}
