using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Transform target; // 玩家的Transform
    public float speed ; // Boss移动速度
    public int damage = 10; // Boss对玩家的攻击伤害

    private Rigidbody2D rb; // Boss的Rigidbody2D组件

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

   
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 检查是否碰撞到玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            // 调用玩家的受伤逻辑
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Boss攻击玩家后销毁自己
            Destroy(gameObject);
        }
    }
}
