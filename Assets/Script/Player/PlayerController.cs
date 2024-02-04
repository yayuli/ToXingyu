using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Weapon currentWeapon;

    public Transform firePoint; // 你可能仍然需要这个引用来确定武器攻击的发起位置

    private Rigidbody2D rb;

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
        Vector2 moveVector = new Vector2(moveX, moveY).normalized;
        rb.velocity = moveVector * 5.0f; // 假设这是玩家的移动速度

        // 使用当前武器
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentWeapon?.Use();
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

    // 更换武器
    public void ChangeWeapon(Weapon newWeaponPrefab)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }
        currentWeapon = Instantiate(newWeaponPrefab, firePoint.position, Quaternion.identity, transform);
    }

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
