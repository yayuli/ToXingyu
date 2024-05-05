using UnityEngine;

public class BombItem : MonoBehaviour
{
    [SerializeField] private int bombsToAdd = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.AddItem(bombsToAdd);
                Destroy(gameObject);
            }
        }
    }
}