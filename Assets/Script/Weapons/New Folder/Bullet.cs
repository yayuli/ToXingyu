using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public int damage;
    float speed = 18f;

    public GameObject hitEffect;


    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if(enemy != null)
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
