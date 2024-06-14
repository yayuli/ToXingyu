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

    [Header("stoot")]
    private bool isAutoShootingEnable = true;

    private Transform closestEnemy;

    protected override void Update()
    {
        base.Update();
        transform.position = (Vector2)player.position + offset;

        FindClosestEnemy();
        //AimAtEnemy();
        HandleInput();
        if (isAutoShootingEnable)
        {
            AimAtEnemy();
            Shoot();
        }

    }

    void FindClosestEnemy()
    {
        closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance && distance <= weaponData.baseRange) // 使用 weaponData.range 替换 fireDistance
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
        if (closestEnemy == null || !isAutoShootingEnable)
            return;

        timeSinceLastShot += Time.deltaTime;
        float modifiedCooldown = weaponData.cooldown / (1 + Player.instance.attributes.attackSpeed / 100.0f);

        if (timeSinceLastShot >= modifiedCooldown)
        {
            Vector2 direction = (closestEnemy.position - muzzlePosition.position).normalized;
            FireBullet(direction);
            timeSinceLastShot = 0;
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isAutoShootingEnable = !isAutoShootingEnable;
            Debug.Log("Shooting mode changed: " + (isAutoShootingEnable ? "Auto" : "Manual"));
        }

        // 手动射击时，根据按键决定射击方向
        if (!isAutoShootingEnable)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ManualShoot(Vector2.right);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ManualShoot(Vector2.left);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ManualShoot(Vector2.up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ManualShoot(Vector2.down);
            }
        }
    }

    void ManualShoot(Vector2 direction)
    {
        FireBullet(direction);
        RotateWeapon(direction);
    }

    void RotateWeapon(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FireBullet(Vector2 direction)
    {
        var muzzleGO = Instantiate(muzzleEffectPrefab, muzzlePosition.position, Quaternion.identity);
        Destroy(muzzleGO, 0.05f);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var bulletGo = Instantiate(bulletPrefab, muzzlePosition.position, Quaternion.Euler(0, 0, angle));
        bulletGo.GetComponent<Bullet>().Initialize(weaponData, direction);
        Destroy(bulletGo, 3);
    }
}