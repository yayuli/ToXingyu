using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    private float speed;
    private int damage;
    private float range;
    private Vector2 direction;
   

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public void Initialize(ItemData weaponData, Vector2 newDirection)
    {
        damage = weaponData.CurrentDamage;
        speed = weaponData.CurrentSpeed;
        range = weaponData.CurrentRange;
        direction = newDirection.normalized;

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = weaponData.CurrentColor;
        }

        // 设置子弹大小
        transform.localScale = weaponData.CurrentSize;

        GetComponent<Rigidbody2D>().velocity = direction * speed;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            Debug.Log($"Bullet hit enemy: {enemy.name} with Damage = {damage}");
            ObjectPool.Return(gameObject, gameObject);
            enemy.TakeDamage(damage);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            ObjectPool.Return(gameObject, gameObject);
        }
    }
}
