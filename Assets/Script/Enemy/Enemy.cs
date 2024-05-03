using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform target; // 玩家的Transform
    [SerializeField] int maxhealth = 100;
    [SerializeField] private float speed = 2f; // Boss移动速度

    private int currentHealth;

    Animator anim;

    public int damage = 1; // Boss对玩家的攻击伤害

    private Rigidbody2D rb; // Boss的Rigidbody2D组件

    void Start()
    {
        currentHealth = maxhealth;
        target = GameObject.Find ("Player").transform;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(target!= null)
        {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();

            transform.position += direction * speed * Time.deltaTime;

            var playerToTheRight = target.position.x > transform.position.x;
            transform.localScale = new Vector2(playerToTheRight ? -1 : 1, 1);
        }
       
    }

    public void Hit(int damage)
    {
        anim.SetTrigger("Hit");
        currentHealth -= damage;
        
        
        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
