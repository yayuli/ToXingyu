using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    // This script needs to know about the Item component on the same GameObject.
    private Item itemComponent;

    void Awake()
    {
        // Get the Item component from the same GameObject
        itemComponent = GetComponent<Item>();
        if (itemComponent == null)
        {
            Debug.LogError("Item component not found on the GameObject");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Check if the itemComponent is not null
            if (itemComponent != null)
            {
                // Apply the item effect using the Item script's UseItem method
                itemComponent.UseItem(other.GetComponent<Player>());

                // Destroy the item GameObject after use
                Destroy(gameObject);
            }
        }
    }
}
