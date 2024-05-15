using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [Header("Config")]
    public int damage;  // 近战武器的伤害值
    public float attackRange;  // 攻击范围
    public float attackDuration;  // 攻击持续时间

    private Animator anim;  // 动画组件
    private bool isAttacking;  // 是否正在攻击

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();  // 获取动画组件
    }

    protected override void Update()
    {
        base.Update();
        if (timeSinceLastShot >= fireRate && !isAttacking)
        {
            Shoot();
            timeSinceLastShot = 0;
        }
    }

    public override void Shoot()
    {
        if (!isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");  // 播放攻击动画

        yield return new WaitForSeconds(attackDuration / 2);  // 等待动画的前半段

        PerformHitDetection();  // 执行命中检测

        yield return new WaitForSeconds(attackDuration / 2);  // 等待动画的后半段

        isAttacking = false;
    }

    private void PerformHitDetection()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Enemy>().TakeDamage(damage);  // 对敌人造成伤害
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);  // 在编辑器中绘制攻击范围
    }
}
