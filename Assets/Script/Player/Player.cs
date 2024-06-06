using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player instance;

    [System.Serializable]
    public class PlayerAttributes
    {
        public int health = 20;
        public int maxHealth = 20;      // 最大生命值
        public int healthRegenRate = 0;  // 生命恢复速率
        public int damageFactor = 0;    // 伤害加成比例
        public int attackRangeFactor = 0;  // 攻击范围加成比例
        public int armor = 0;           // 护甲值
        public int criticalRate = 10;    // 暴击率
        public int criticalDamage = 50; // 暴击伤害
        public int attackSpeed = 0;     // 攻击速度加成比例
        public int dodgeRate = 0;       // 闪避率
        public int moveSpeedFactor = 0; // 移动速度加成比例
        public int pickUpRangeFactor = 0; // 捡取范围加成比例

    }

    [Header("Attributes")]
    public PlayerAttributes attributes; // Player attributes instance
    public delegate void AttributeChanged();
    public event AttributeChanged OnHealthRangeChanged;
    public event AttributeChanged OnMoveSpeedChanged;
    public event AttributeChanged OnAromoChanged;
    public event AttributeChanged OnAttackSpeedChanged;

    [Header("ItemData")]
    public ItemData equippedItem;

    [Header("Movement")]
    public float moveSpeed = 5.0f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private float stopX, stopY;

    [Header("Animation")]
    public Animator animator;

    [Header("Bomb item")]
    [SerializeField] private int bombCount = 0;
    [SerializeField] private GameObject bombPrefab;

    [Header("Health Bar")]
    [SerializeField] private Slider healthBar;

    [Header("PickUpLoot")]
    public float pickupRange = 1.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 使对象跨场景持续存在
        }
        else
        {
            Destroy(gameObject); // 确保不创建重复实例
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateHealthUI();
    }

    void Update()
    {
        HandleInput();
        HandleBombDrop();
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }

    #region Movement
    void HandleInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        if (movement != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            stopX = movement.x;
            stopY = movement.y;
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        animator.SetFloat("Horizontal", stopX);
        animator.SetFloat("Vertical", stopY);
    }


    void MoveCharacter()
    {
        float speed = moveSpeed * (1 + attributes.moveSpeedFactor / 100.0f);
        rb.velocity = movement * speed;
    }


    #endregion


    public void ResetPlayerState()
    {
        // 重置玩家的生命值、位置等
        attributes.health = attributes.maxHealth;
        UpdateHealthUI();
    }


    #region add bomb or can add other item which need pick up
    public void AddItem(int count)
    {
        bombCount += count;
        // Optionally update UI or other logic here
    }

    private void HandleBombDrop()
    {
        if (Input.GetKeyDown(KeyCode.Q) && bombCount > 0)
        {
            DropBomb();
        }
    }

    private void DropBomb()
    {
        if (bombPrefab != null)
        {
            Instantiate(bombPrefab, transform.position, Quaternion.identity);
            bombCount--;
        }
    }
    #endregion

    #region public attributes apply to Item script

    public IEnumerator ApplyTempStatBoost(System.Action increaseEffect, System.Action decreaseEffect, float duration)
    {
        increaseEffect.Invoke();
        yield return new WaitForSeconds(duration);
        decreaseEffect.Invoke();
    }
    #endregion

    #region Use items in game

    public void ApplyItemEffect(ItemData item)
    {
        Debug.Log($"Applying effect from item: {item.itemName}");
        switch (item.itemType)
        {
            case ItemData.ItemType.HealthPotion:
                Debug.Log($"Before Health Potion: Health={attributes.health}");
                ModifyHealth(item.healthRegeRate);
                Debug.Log($"After Health Potion: Health={attributes.health}");
                break;
            case ItemData.ItemType.ArmorSet:
                Debug.Log($"Before Armor Set: Armor={attributes.armor}");
                ModifyArmor(item.armor);
                Debug.Log($"After Armor Set: Armor={attributes.armor}");
                break;
            case ItemData.ItemType.SpeedBoots:
                Debug.Log($"Before Speed Boots: Move Speed Factor={attributes.moveSpeedFactor}");
                ModifyMoveSpeed(item.moveSpeedFactor);
                Debug.Log($"After Speed Boots: Move Speed Factor={attributes.moveSpeedFactor}");
                break;
            case ItemData.ItemType.PrecisionAmulet:
                Debug.Log($"Before Precision Amulet: Critical Rate={attributes.criticalRate}");
                //ModifyCriticalRate(item.criticalRate); // Check if "item.criticalRate" should really be affecting critical rate
                Debug.Log($"After Precision Amulet: Critical Rate={attributes.criticalRate}");
                break;
                // Add similar debug logs for other item types
        }
        UpdateHealthUI();
    }


    public void ModifyHealth(int amount)
    {
        attributes.health += amount;
        attributes.health = Mathf.Clamp(attributes.health, 0, attributes.maxHealth); // 确保 health 不超过 maxHealth 也不小于0
        OnHealthRangeChanged?.Invoke();
        UpdateHealthUI();

        if (attributes.health <= 0)
        {
            Debug.Log("Player has died.");
        }
    }

    public void ModifyArmor(int amount)
    {
        attributes.armor += amount;
        // 可以添加限制条件，例如护甲值不小于0
        attributes.armor = Mathf.Max(attributes.armor, 0);
        OnAromoChanged?.Invoke(); // 假设你有一个处理护甲变化的事件

        Debug.Log($"Armor updated to: {attributes.armor}");
    }

    public void ModifyAttackSpeed(int increment)
    {
        attributes.attackSpeed += increment;
        attributes.attackSpeed = Mathf.Max(attributes.attackSpeed, 0);
        OnAttackSpeedChanged?.Invoke();
        Debug.Log($"Attack Speed updated to: {attributes.attackSpeed}");
    }


    public void ModifyMoveSpeed(int increment)
    {
        Debug.Log($"Modifying Move Speed Factor: Current={attributes.moveSpeedFactor}, Increment={increment}");
        attributes.moveSpeedFactor += increment;
        Debug.Log($"New Move Speed Factor: {attributes.moveSpeedFactor}");
        OnMoveSpeedChanged?.Invoke();
    }

    /*
    public void ModifyDodgeRate(int increment)
    {
        attributes.dodgeRate += increment;
        // 可以添加限制条件，例如闪避率的上下限
        attributes.dodgeRate = Mathf.Clamp(attributes.dodgeRate, 0, 100);
        //OnDodgeRateChanged?.Invoke(); // 假设你有一个处理闪避率变化的事件

        Debug.Log($"Dodge Rate updated to: {attributes.dodgeRate}");
    }
    */

    #endregion

    #region UI
    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)attributes.health / attributes.maxHealth;
            // Debug.Log("Health updated to: " + attributes.health + "/" + attributes.maxHealth);
        }

    }


    public void TakeDamage(int damage)
    {
        int reducedDamage = Mathf.Max(damage - attributes.armor, 0); // 确保伤害值不会是负数
        ModifyHealth(-damage);
        UpdateHealthUI();
    }

    #endregion
}
