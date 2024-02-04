using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joke : MonoBehaviour
{
    public BossHealth bossHealth;
    public bool isGoodJoke; // indicates if it's a good joke
    

    void Start()
    {
        bossHealth = FindObjectOfType<BossHealth>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(1);
                //AudioManager.Instance.Play(0, "bg", false);
            }

            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            // Original logic for handling player collision
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                if (isGoodJoke)
                {
                    playerHealth.Heal(1); // Heal the player
                }
                
                else
                {
                    playerHealth.TakeDamage(1); // Inflict damage
                }
            }

            Destroy(gameObject); 
        }
    }
}
