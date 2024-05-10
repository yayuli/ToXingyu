using UnityEngine;

[CreateAssetMenu(fileName = "ShootingAbility", menuName = "Enemy Abilities/Shooting")]
public class ShootingAbility : EnemyAbility
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;


    public override void Execute(GameObject enemy)
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        GameObject projectile = GameObject.Instantiate(projectilePrefab, enemy.transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        Vector2 direction = (player.position - enemy.transform.position).normalized;
        rb.velocity = direction * projectileSpeed;
    }
}
