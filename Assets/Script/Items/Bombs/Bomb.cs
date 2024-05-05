using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionEffect; // explosion effect Prefab
    public float delay = 2f; // explosion delay time 

    void Start()
    {
        Invoke("Explode", delay); // call the explosion method after 2 secs
    }

    void Explode()
    {
        // instantiate the explosion effect get a reference to the particle system
        GameObject explosionInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        ParticleSystem explosionParticles = explosionInstance.GetComponent<ParticleSystem>();

        // detection within explosion range
        float explosionRadius = 2f; // explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        //if a destructible layer is detected, destroy it
        foreach (Collider2D hit in colliders)
        {
            if (hit.CompareTag("Wall"))
            {
                Destroy(hit.gameObject); 
            }
        }

        // destroyed the particle effect after the particales have finished playing
        if (explosionParticles != null)
        {
            //wait for the particles to finish playing
            Destroy(explosionInstance, explosionParticles.main.duration);
        }
        else
        {
            Destroy(explosionInstance, 2f); 
        }

        Destroy(gameObject);
    }

}

