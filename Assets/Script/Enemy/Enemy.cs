using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyConfig config;
    [SerializeField] protected EnemyAbility ability;

    [SerializeField] protected Transform target; // player's Transform

    private float currentHealth;
    private float currentSpeed;
    private float currentDamage;

    public int damage = 1;
    [SerializeField] private float shootingRange = 10f;
    [SerializeField] private float shootingCooldown = 2f;
    private float lastShootTime = 0;

    [SerializeField] private LootTable lootTable;//reference to the loot Table

    protected Animator anim;

    private Rigidbody2D rb;

    //public GameObject score;

    protected virtual void Start()
    {
        InitializeAttributes();
        //currentHealth = maxHealth;
        //currentHealth = (int)health;
        target = GameObject.Find("Player").transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        //Debug.Log($"Start - Health is: {health} set in {this.gameObject.name}");
    }

    void Update()
    {
        MoveTowardsTarget();

        AbilityShoot();

    }

    public void InitializeAttributes()
    {
        int waveNum = WaveManager.Instance.WaveNum;
        currentHealth = config.baseHealth + (waveNum - 1) * config.healthIncrementPerWave;
        currentSpeed = config.baseSpeed + (waveNum - 1) * config.speedIncrementPerWave;
        currentDamage = config.baseDamage + (waveNum - 1) * config.damageIncrementPerWave;
        Debug.Log($"Wave {waveNum}: Health {currentHealth}, Speed {currentSpeed}, Damage {currentDamage}");
    }

    protected void MoveTowardsTarget()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * currentSpeed;

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
                Debug.Log("take 1 damage");
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
        // Destroy(ScoreText, 1f)
        Destroy(gameObject);
        DropLoot();
        Destroy(gameObject);
        //Destroy(gameObject);
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
