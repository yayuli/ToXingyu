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
        public int Health;
        public int MaxHealth = 100;
        public int Energy;
        public int Strength;
        public int Dexterity;
        public int Intelligence;
        public int Luck;

        // Special attributes
        public int MemoryPoints;
        public float RegenerationRate;
        public int Willpower;

    }

    [Header("Attributes")]
    public PlayerAttributes attributes; // Player attributes instance
    public delegate void AttributeChanged();
    public event AttributeChanged OnHealthChanged;
    public event AttributeChanged OnStrengthChanged;
    public event AttributeChanged OnEnergyChanged;

    [Header("ItemData")]
    public ItemData equippedItem;

    [Header("Movement")]
    public float moveSpeed = 5.0f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private float lastHorizontalInput;

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
        instance = this;
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
    void MoveCharacter()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void HandleInput()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        //animator.SetFloat("Horizontal", movement.x);
        //animator.SetFloat("Vertical", movement.y);
        float speed = movement.magnitude * moveSpeed;
        animator.SetFloat("Speed", speed);

        if (movement.x != lastHorizontalInput && movement.x != 0)
        {
           // FlipCharacterBasedOnDirection(movement.x);
            lastHorizontalInput = movement.x;
        }
    }
    /*
    void FlipCharacterBasedOnDirection(float horizontalInput)
    {
        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);
        }
    }
    */
#endregion

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
        switch (item.itemType)
        {
            case ItemData.ItemType.HealthPotion:
                ModifyHealth(item.effectMagnitude);
                break;
            case ItemData.ItemType.EnergyDrink:
                ModifyEnergy(item.effectMagnitude);
                break;
            case ItemData.ItemType.StrengthElixir:
                ModifyStrength(item.effectMagnitude);
                break;
            case ItemData.ItemType.AgilityBoots:
                attributes.Dexterity += item.effectMagnitude;
                break;
            case ItemData.ItemType.WisdomScroll:
                attributes.Intelligence += item.effectMagnitude;
                break;
            case ItemData.ItemType.LuckCharm:
                StartCoroutine(ApplyTempStatBoost(() => attributes.Luck += item.effectMagnitude,
                                                         () => attributes.Luck -= item.effectMagnitude,
                                                         item.duration));
                break;
            case ItemData.ItemType.MemoryCrystal:
                attributes.MemoryPoints += item.effectMagnitude;
                break;
            case ItemData.ItemType.RegenerationAmulet:
                StartCoroutine(ApplyTempStatBoost(() => attributes.RegenerationRate += item.effectMagnitude,
                                                         () => attributes.RegenerationRate -= item.effectMagnitude,
                                                         item.duration));
                break;
            case ItemData.ItemType.ShieldPotion:
                StartCoroutine(ApplyTempStatBoost(() => attributes.Willpower += item.effectMagnitude,
                                                         () => attributes.Willpower -= item.effectMagnitude,
                                                         item.duration));
                break;
        }
       // ObjectPool.Instance.ReturnToPool(item prefabName, gameObject);
        UpdateHealthUI();

    }
    public void ModifyHealth(int amount)
    {
        attributes.Health += amount;
        attributes.Health = Mathf.Clamp(attributes.Health, 0, attributes.MaxHealth); // 确保健康值不会超出范围

        // 触发健康改变事件
        OnHealthChanged?.Invoke();

        if (attributes.Health <= 0)
        {
            Debug.Log("Player has died.");  // 处理玩家死亡逻辑
        }
    }

    public void ModifyStrength(int amount)
    {
        attributes.Strength += amount;
        OnStrengthChanged?.Invoke();
    }

    public void ModifyEnergy(int amount)
    {
        attributes.Energy += amount;
        OnEnergyChanged?.Invoke();
    }
    #endregion

    #region UI
    private void UpdateHealthUI()
    {

        healthBar.value = (float)attributes.Health / attributes.MaxHealth;
        Debug.Log("Health updated to: " + attributes.Health);
    }

    public void TakeDamage(int damage)
    {
        ModifyHealth(-damage);
    }

    #endregion
}
