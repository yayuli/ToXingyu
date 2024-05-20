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
            if (Player.instance != null && itemComponent != null && itemComponent.itemData != null)
            {
                // Apply the item effect using the item data
                Player.instance.ApplyItemEffect(itemComponent.itemData);

                // Destroy the item GameObject after use
                Destroy(gameObject);
            }
        }
    }
}
