using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
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

        // Method to modify health
        public void ModifyHealth(int amount)
        {
            Health += amount;
            Health = Mathf.Clamp(Health, 0, MaxHealth); 
            if (Health <= 0)
            {
                // Trigger death logic
                Debug.Log("Player has died.");
            }
        }
    }

    [Header("Attributes")]
    public PlayerAttributes attributes; // Player attributes instance

    [Header("ItemData")]
    public ItemData equippedItem;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.0f;
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
    public void UseEquippedItem()
    {
        if (equippedItem != null)
        {
            ApplyItemEffect(equippedItem);
        }
    }

    private void ApplyItemEffect(ItemData item)
    {
        switch (item.itemType)
        {
            case ItemData.ItemType.HealthPotion:
                attributes.ModifyHealth(item.effectMagnitude);
                Debug.Log("Used Health Potion: " + item.effectMagnitude);
                UpdateHealthUI(); // update health UI
                break;
        }

        Debug.Log("Used item: " + item.itemName);
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
        attributes.ModifyHealth(-damage); //call ModifyHealth and pass ina negative munber to represent damage
        UpdateHealthUI(); 
        if (attributes.Health <= 0)
        {
            // 这里可以添加玩家死亡时的逻辑add the logic when the player dirs in the future
            Debug.Log("Player died.");
        }
    }

    #endregion
}
