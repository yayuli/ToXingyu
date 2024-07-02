using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    private float speed;
    private int damage;
    private float range;
    private Vector2 direction;
    public bool shouldKnockBack = false;
    private Animator anim;

    public float SplashRange = 2;
   
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
        if (SplashRange > 0)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, SplashRange);
            foreach (Collider2D hitCollider in hitColliders)
            {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    anim.SetTrigger("Hit");
                    SFXManager.instance.PlaySFXPitched(5);
                    Debug.Log($"Splash Damage to {enemy.name} with Damage = {damage}");
                    enemy.TakeDamage(damage, shouldKnockBack);
                }
            }
        }
        else
        {
            var enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                anim.SetTrigger("Hit");
                Debug.Log($"Bullet hit enemy: {enemy.name} with Damage = {damage}");
                enemy.TakeDamage(damage, shouldKnockBack);
            }
        }

        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.GetComponent<Enemy>() != null)
        {
            anim.SetTrigger("Hit");
            TriggerHitEffect(transform.position);
        }

        // 传递正确的 prefab 和 gameObject 到对象池回收方法
        ObjectPool.Return(this.gameObject, gameObject); // 确保传递正确的参数
    }


    void TriggerHitEffect(Vector3 position)
    {
        if (hitEffect !=null)
        {
            Instantiate(hitEffect, position, Quaternion.identity);
        }
    }
}
