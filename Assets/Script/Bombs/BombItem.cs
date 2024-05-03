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
<<<<<<< HEAD
                player.AddItem(bombsToAdd);
=======
                //player.AddBomb(bombsToAdd);
>>>>>>> b0bdbdcdd17ecdd501e2b3891178de4b898c4323
                Destroy(gameObject);
            }
        }
    }
}