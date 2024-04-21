using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //private Weapon currentWeapon;

    public Transform firePoint; // 你可能仍然需要这个引用来确定武器攻击的发起位置
    public GameObject bulletPrefab;
    public float bulletSpeed= 10f;
    private float lastFire;
    public float fireDelay;
    private Vector2 lastDirection = Vector2.right;
    private Vector2 shootDirection;

    private Rigidbody2D rb;
    public float moveSpeed = 5.0f;

    public BombItem bombItemInRange; // 在玩家范围内的炸弹道具
    public GameObject bombPrefab; // 炸弹Prefab
    private int bombCount = 0; // 玩家拥有的炸弹数量

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Handle player movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveX, moveY).normalized;
        rb.velocity = movement * moveSpeed;

        if (movement != Vector2.zero)
        {
            shootDirection = movement;
        }

        // 射击
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot(shootDirection);
        }

        // 使用当前武器
        if (Input.GetKeyDown(KeyCode.Q))
        {
           // currentWeapon?.Use();
        }

        // 拾取炸弹
        if (Input.GetKeyDown(KeyCode.Z) && bombItemInRange != null)
        {
            AddBomb(1);
            Destroy(bombItemInRange.gameObject);
            bombItemInRange = null;
        }

        // 放置炸弹
        if (Input.GetKeyDown(KeyCode.X) && bombCount > 0)
        {
            PlaceBomb();
        }
    }

    void Shoot(Vector2 direction)
    {
        if (bulletPrefab && firePoint)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            rbBullet.velocity = direction * bulletSpeed;

            // 设置子弹旋转以面向射击方向
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    /*
    public void ChangeWeapon(Weapon newWeaponPrefab)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }
        currentWeapon = Instantiate(newWeaponPrefab, firePoint.position, Quaternion.identity, transform);
    }
    */
    // 增加炸弹数量
    public void AddBomb(int amount)
    {
        bombCount += amount;
    }

    // 放置炸弹
    private void PlaceBomb()
    {
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
        bombCount--;
    }
}
