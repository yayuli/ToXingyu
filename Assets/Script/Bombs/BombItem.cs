using UnityEngine;

public class BombItem : MonoBehaviour
{
    [SerializeField] private int bombsToAdd = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.AddItem(bombsToAdd);
                Destroy(gameObject);
            }
        }
    }
}