using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 这个类继承自 WeaponBase，实现了远程武器的特定行为，如瞄准和射击。
/// 这里是可以使用新的 ItemData 属性（如 attackPower, range, cooldown）
/// </summary>
public class RangedWeapon : WeaponBase
{
    [Header("Prefabs")]
    public GameObject bulletPrefab;
    public Transform muzzlePosition;
    public GameObject muzzleEffectPrefab;
    public ItemData weaponData;//ussed itemdata

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
            if (distance < closestDistance && distance <= weaponData.range) // 使用 weaponData.range 替换 fireDistance
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
        if (timeSinceLastShot >= weaponData.cooldown)
        {
            FireBullet();  // 从这个方法分离出射击的具体逻辑
            timeSinceLastShot = 0;  // Reset the shooting timer
        }
    }

    void FireBullet()
    {
        var muzzleGO = Instantiate(muzzleEffectPrefab, muzzlePosition.position, transform.rotation);
        Destroy(muzzleGO, 0.05f);  // Destroy muzzle effect shortly after

        var bulletGo = Instantiate(bulletPrefab, muzzlePosition.position, transform.rotation);
        bulletGo.GetComponent<Bullet>().Initialize(weaponData.attackPower, weaponData.speed);  // Initialize bullet
        Destroy(bulletGo, 3);  // Destroy the bullet after some time
    }


}

