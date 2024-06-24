using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    private float speed;
    private int damage;
    private float range;
    private Vector2 direction;
   
    private Animator anim;
   
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

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
            anim.SetTrigger("Hit");

            Debug.Log($"Bullet hit enemy: {enemy.name} with Damage = {damage}");
            ObjectPool.Return(gameObject, gameObject);
            
            SFXManager.instance.PlaySFXPitched(5);
            enemy.TakeDamage(damage);
            //can add effect
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            anim.SetTrigger("Hit");
            TriggerHitEffect(transform.position);
            ObjectPool.Return(gameObject, gameObject);
        }
    }
    void TriggerHitEffect(Vector3 position)
    {
        if (hitEffect !=null)
        {
            Instantiate(hitEffect, position, Quaternion.identity);
        }
    }
}
