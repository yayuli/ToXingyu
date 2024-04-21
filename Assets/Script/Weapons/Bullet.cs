using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    public GameObject hitEffect;

    public float lifetime = 0.5f;

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
    void Start()
    {
        StartCoroutine(DeathDelay());
        Destroy(gameObject, lifetime);
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {

        
        // 处理撞击逻辑，例如对敌人造成伤害
        // 示例：如果撞击的对象有生命值组件
        Health health = other.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject); // 销毁子弹
        }

        

        // 销毁子弹
        Destroy(gameObject);
    }
    
}
