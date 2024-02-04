using UnityEngine;

public class BombItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.bombItemInRange = this; // 玩家进入炸弹道具范围
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && player.bombItemInRange == this)
            {
                player.bombItemInRange = null; // 玩家离开炸弹道具范围
            }
        }
    }

}
