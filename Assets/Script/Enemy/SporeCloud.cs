using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// define the damage code of spore cloud prefab to the player in the TixicSpore.cs
/// </summary>
public class SporeCloud : MonoBehaviour
{
    [SerializeField] private float duration = 4f;  // Duration the cloud exists
    [SerializeField] private int damage = 1;

    private void Start()
    {
        Destroy(gameObject, duration);  // Automatically destroy the cloud after a set time
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}