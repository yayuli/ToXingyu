using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovingAbility", menuName = "Enemy Abilities/Moving")]
public class MovingAbility : EnemyAbility
{
    public float moveSpeed = 2.0f;
    public LayerMask wallLayer;

    public override void Execute(GameObject enemy)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // Get or create direction data associated with this enemy
        Vector2 direction = GetOrCreateDirection(enemy);

        // Move the enemy
        Vector2 newPosition = rb.position + direction * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPosition);

        // Check for walls and change direction if necessary
        if (Physics2D.Raycast(rb.position, direction, 0.5f, wallLayer))
        {
            direction = ChangeDirection(direction);
            StoreDirection(enemy, direction); // Store the new direction
        }
    }

    private Vector2 ChangeDirection(Vector2 currentDirection)
    {
        // Simple method to pick a random new direction
        int choice = Random.Range(0, 4);
        switch (choice)
        {
            case 0: return Vector2.up;
            case 1: return Vector2.down;
            case 2: return Vector2.left;
            case 3: return Vector2.right;
            default: return Vector2.up;
        }
    }

    // Store and retrieve direction in the enemy's GameObject for persistence between calls
    private Vector2 GetOrCreateDirection(GameObject enemy)
    {
        Vector2 direction;
        if (enemy.TryGetComponent<EnemyDirection>(out EnemyDirection dirComponent))
        {
            direction = dirComponent.Direction;
        }
        else
        {
            direction = ChangeDirection(Vector2.zero);
            enemy.AddComponent<EnemyDirection>().Direction = direction;
        }
        return direction;
    }

    private void StoreDirection(GameObject enemy, Vector2 direction)
    {
        if (enemy.TryGetComponent<EnemyDirection>(out EnemyDirection dirComponent))
        {
            dirComponent.Direction = direction;
        }
    }
}

public class EnemyDirection : MonoBehaviour
{
    public Vector2 Direction;
}

