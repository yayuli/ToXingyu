using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float followRange = 10.0f;
    public SpriteRenderer spriteRenderer; // 精灵渲染器
    public Sprite upSprite;    // 向上移动时的精灵
    public Sprite downSprite;  // 向下移动时的精灵
    public Sprite leftSprite;  // 向左移动时的精灵
    public Sprite rightSprite; // 向右移动时的精灵

    private Vector2 lastDirection = Vector2.right;

    private Rigidbody2D rb;
    //private GameObject nearestEnemy;
    private Vector2 movement;

<<<<<<< HEAD
    [Header ("Bomb item")]
    [SerializeField] private int bombCount = 0;//tracks the number of bombs
    [SerializeField] private GameObject bombPrefab;//for the bomb to drop


=======
>>>>>>> b0bdbdcdd17ecdd501e2b3891178de4b898c4323
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        // 根据移动输入更换精灵
        if (movement != Vector2.zero)
        {
            lastDirection = movement.normalized;
            ChangeSprite(lastDirection);
        }

        /*
        // 射击逻辑
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shootingController.HandleShooting();
        }
        */


        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null && Vector2.Distance(transform.position, nearestEnemy.transform.position) <= 2.0f)
        {
            AimAtEnemy(nearestEnemy);
        }
<<<<<<< HEAD

        HandleBombDrop();
=======
>>>>>>> b0bdbdcdd17ecdd501e2b3891178de4b898c4323
    }


    void FixedUpdate()
    {
        // 根据玩家的输入移动角色
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

    }

    void AimAtEnemy(GameObject enemy)
    {
        Vector2 directionToEnemy = enemy.transform.position - transform.position;
        Vector2 direction;

        if (Mathf.Abs(directionToEnemy.x) > Mathf.Abs(directionToEnemy.y))
        {
            // 敌人在水平方向
            direction = directionToEnemy.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            // 敌人在垂直方向
            direction = directionToEnemy.y > 0 ? Vector2.up : Vector2.down;
        }

        ChangeSprite(direction); // 更换为对应方向的精灵
    }



    void ChangeSprite(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            spriteRenderer.sprite = (direction.x > 0) ? rightSprite : leftSprite;
        }
        else
        {
            spriteRenderer.sprite = (direction.y > 0) ? upSprite : downSprite;
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                nearest = enemy;
                minDistance = distance;
            }
        }

        return nearest;
    }

<<<<<<< HEAD
    public void AddItem(int count)
    {
        bombCount += count;

        //here can update ui or other logic
    }

    void HandleBombDrop()
    {
        if(Input.GetKeyDown(KeyCode.Q)&& bombCount > 0)
        {
            DropBomb();
        }
    }

    void DropBomb()
    {
        if(bombPrefab!= null)
        {
            Instantiate(bombPrefab, transform.transform.position, Quaternion.identity);
            bombCount--;
        }
    }
=======
    

>>>>>>> b0bdbdcdd17ecdd501e2b3891178de4b898c4323
}



