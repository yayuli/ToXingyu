using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;

    public void Initialize(ItemData data)
    {
        this.itemData = data;  // Initialize the item with its data
    }

    public void UseItem()
    {
        if (Player.instance == null)
        {
            Debug.LogError("Player instance is null.");
            return;
        }

        if (itemData == null)
        {
            Debug.LogError("ItemData is not set for this item.");
            return;
        }

        Player.instance.ApplyItemEffect(itemData);
    }
}
