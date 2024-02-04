using UnityEngine;
using System.Collections.Generic;

public class ShootingController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10.0f;
    public float shootDistance = 4f;
    public SpriteRenderer spriteRenderer;

    private Vector2 shootDirection = Vector2.up;

    void Update()
    {
        HandleShooting();
    }

    public void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = shootDirection * projectileSpeed;

            //AudioManager.Instance.Play(1, "PlayerShoot", false);

            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Destroy(projectile, shootDistance / projectileSpeed);
        }
    }
}