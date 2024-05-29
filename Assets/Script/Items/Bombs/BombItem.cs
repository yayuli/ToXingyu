using UnityEngine;

public class BombItem : MonoBehaviour
{
    public int bombsToAdd = 1;

    public void OnTriggerEnter2D(Collider2D other)
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