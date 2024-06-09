using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemyAbility ability;

    [SerializeField] protected Transform target; // player's Transform
    [SerializeField] public float health;
    [SerializeField] public float speed;// enemy speed
    [SerializeField] private int damage = 1; //attack damage to player
    public float attackPower;

    [SerializeField] private float shootingRange = 10f;
    [SerializeField] private float shootingCooldown = 2f;
    private float lastShootTime = 0;

    [SerializeField] private LootTable lootTable;//reference to the loot Table


    protected int currentHealth;
    protected Animator anim;

    private Rigidbody2D rb;

    //public GameObject score;

    protected virtual void Start()
    {
        //currentHealth = maxHealth;
        target = GameObject.Find("Player").transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
       
    }

    void Update()
    {
        MoveTowardsTarget();

        AbilityShoot();

    }

    public void InitializeAttributes(float[] attributes)
    {
        if (attributes.Length >= 3)
        {
            health = attributes[0];
            speed = attributes[1];
            attackPower = attributes[2];
        }
    }

    protected void MoveTowardsTarget()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime); // 使用Rigidbody2D移动

            bool playerToTheRight = target.position.x > transform.position.x;
            transform.localScale = new Vector2(playerToTheRight ? 1 : -1, 1); // 根据玩家位置翻转敌人
        }
    }

    public void PerformAbility()
    {
        ability.Execute(gameObject);
    }

    protected void AbilityShoot()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        if (distanceToPlayer < shootingRange && Time.time > lastShootTime + shootingCooldown)
        {
            PerformAbility();
            lastShootTime = Time.time;  // Update last shoot time
        }
    }
    public void TakeDamage(int damage)
    {
        anim.SetTrigger("Hit");
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }

    /*
    IEnumerator DestroyAfterAnimation()
    {
        DropLoot();
        yield return new WaitForSeconds(1);
        EnemyManager.Instance.DestroyEnemy(gameObject);
    }
    */
    public void Die()
    {
        Vector3 scorePosition = transform.position + new Vector3(0, 1.5f, 0);
       // GameObject ScoreText = Instantiate(score,scorePosition,Quaternion.identity);
       // Destroy(ScoreText, 1f);
        DropLoot();
        Destroy(gameObject);
        //anim.SetTrigger("Die");
        // 可能需要等待死亡动画播放完成后再销毁
        //EnemyManager.Instance.DestroyEnemy(gameObject);

    }

    public void DropLoot()
    {
        if (lootTable != null && lootTable.items.Length > 0)
        {
            int index = lootTable.GetRandomItemIndex();
            LootItem droppedItem = lootTable.items[index];
            GameObject lootPrefab = Instantiate(droppedItem.itemPrefab, transform.position, Quaternion.identity);
            lootPrefab.GetComponent<Loot>().Initialize(droppedItem);
        }
    }
}
