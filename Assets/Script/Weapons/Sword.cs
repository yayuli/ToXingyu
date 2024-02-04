using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public int damage = 25;
    public float attackRange = 1f;
    public LayerMask enemyLayers;

    public override void Use()
    {
        MeleeAttack();
    }

    void MeleeAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            // 这里应该调用敌人受伤的方法，例如 enemy.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (transform.position == null)
            return;

        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
