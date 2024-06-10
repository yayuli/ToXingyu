using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;  // 可能用于显示击中效果的特效

    private float speed;  // 子弹的速度
    private int damage=25;  // 子弹的伤害值
    private Vector2 direction;

    private void FixedUpdate()
    {
        // 使用速度属性来移动子弹
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    // 初始化子弹的属性
    public void Initialize(int newDamage, float newSpeed, Vector2 newDirection)
    {
        damage = newDamage;
        speed = newSpeed;
        direction = newDirection.normalized;

         GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            Destroy(gameObject);
            enemy.TakeDamage(25);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
