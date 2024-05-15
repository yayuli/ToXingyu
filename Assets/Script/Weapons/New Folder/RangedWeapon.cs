using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : WeaponBase
{
    [Header("Prefabs")]
    public GameObject bulletPrefab;
    public Transform muzzlePosition;
    public GameObject muzzleEffectPrefab;

    private Transform closestEnemy;

    protected override void Update()
    {
        base.Update();
        transform.position = (Vector2)player.position + offset;

        FindClosestEnemy();
        AimAtEnemy();
        Shoot();
    }

    void FindClosestEnemy()
    {
        closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance && distance <= fireDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }
    }

    void AimAtEnemy()
    {
        if (closestEnemy != null)
        {
            Vector3 direction = closestEnemy.position - transform.position;
            direction.Normalize();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle);

            transform.position = (Vector2)player.position + offset;

        }
    }

    public override void Shoot()
    {
        if (closestEnemy == null)
            return;

        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= fireRate)
        {
            // 创建枪口特效
            var muzzleGO = Instantiate(muzzleEffectPrefab, muzzlePosition.position, transform.rotation);
            muzzleGO.transform.SetParent(transform);
            Destroy(muzzleGO, 0.05f);  // 短时间后销毁特效

            // 创建子弹
            var bulletGo = Instantiate(bulletPrefab, muzzlePosition.position, transform.rotation);
            Destroy(bulletGo, 3);  // 3秒后销毁子弹

            // 重置时间，准备下一次射击
            timeSinceLastShot = 0;
        }
    }

}

