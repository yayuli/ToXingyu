using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform target; // player's Transform
    [SerializeField] protected int maxHealth = 50;
    [SerializeField] protected float speed = 2f; // enemy speed

    protected int currentHealth;
    protected Animator anim;

    [SerializeField] private int damage = 1; // boss attack damage to player

    private Rigidbody2D rb;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        target = GameObject.Find ("Player").transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MoveTowardsTarget();
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

    public void TakeDamage(int damage)
    {
        anim.SetTrigger("Hit");
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

   
    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(1);
        EnemyManager.Instance.DestroyEnemy(gameObject);
    }

    protected virtual void Die()
    {
        //anim.SetTrigger("Die");
        // 可能需要等待死亡动画播放完成后再销毁
        StartCoroutine(DestroyAfterAnimation());
    }

}
