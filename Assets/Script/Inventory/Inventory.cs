using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();  // Changed from ItemData to Item

    public delegate void OnInventoryChanged();
    public event OnInventoryChanged onInventoryChanged;

    public void AddItem(Item item)
    {
        items.Add(item);
        onInventoryChanged?.Invoke();  // Notify any listeners that the inventory has changed
        Debug.Log(item.itemData.itemName + " added to inventory.");  // Assuming each Item has a reference to its ItemData
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        onInventoryChanged?.Invoke();  // Notify any listeners that the inventory has changed
        Debug.Log(item.itemData.itemName + " removed from inventory.");
    }

    public void UseItem(Item item, Player player)
    {
        if (items.Contains(item))
        {
            item.UseItem(player);  // Directly call UseItem on the Item component
            RemoveItem(item);  // Optionally remove the item after use
        }
    }

    public void DisplayItems()
    {
        foreach (var item in items)
        {
            Debug.Log("Inventory has: " + item.itemData.itemName);  // Log the name of each item
        }
    }
}
