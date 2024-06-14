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
    public string Weapondescription;
    public float cooldown = 0.5f;
    public int baseDamage = 25;  // 初始伤害值
    public float baseSpeed = 5.0f;  // 初始速度
    public float baseRange = 5.0f;  // 初始射程
    public int level = 1;  // 当前等级

    public int damageIncreasePerLevel = 5;  // 每级升级增加的伤害值
    public float speedIncreasePerLevel = 0.5f;  // 每级升级增加的速度
    public float rangeIncreasePerLevel = 1.0f;  // 每级升级增加的射程

    public int CurrentDamage => baseDamage + (level - 1) * damageIncreasePerLevel;
    public float CurrentSpeed => baseSpeed + (level - 1) * speedIncreasePerLevel;
    public float CurrentRange => baseRange + (level - 1) * rangeIncreasePerLevel;

    public int cost;

    [Header("bullet")]
    public Color baseColor = Color.white;
    public Vector3 baseSize = new Vector3(0.17f, 0.17f, 1);
    public Color CurrentColor => baseColor * (1 + (level - 1) * 0.1f); // 计算当前颜色
    public Vector3 CurrentSize
    {
        get
        {
            // 使用对数函数来平滑增长
            float growthFactor = Mathf.Log(level + 1);
            return baseSize * growthFactor;
        }
    }
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

    public void UpgradeWeapon()
    {
        level++;
        Debug.Log($"{itemName} upgraded to level {level}: Damage = {CurrentDamage}, Speed = {CurrentSpeed}, Range = {CurrentRange}");
    }

    public Color GetWeaponColorByLevel()
    {
        switch (level)
        {
            case 1:
                return Color.white;
            case 2:
                return Color.blue;
            case 3:
                return Color.green;
            case 4:
                return Color.yellow;
            case 5:
                return Color.red;
            default:
                return Color.magenta; // 超过5级使用此颜色
        }
    }

    [System.Serializable]
    public class PlayerAttributes
    {
        
    }
}
