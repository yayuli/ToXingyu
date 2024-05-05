using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Inventory playerInventory;
    public GameObject inventoryPanel;  // Reference to the UI panel that contains the inventory slots
    public GameObject inventoryItemPrefab;  // A prefab used to instantiate inventory items in the UI

    void Start()
    {
        if (playerInventory != null)
        {
            playerInventory.onInventoryChanged += UpdateInventoryDisplay;
        }
        UpdateInventoryDisplay();  // Initial update to set the display at game start
    }

    void UpdateInventoryDisplay()
    {
        // Clear existing inventory display
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);  // Remove all current items displayed
        }

        // Loop through playerInventory.items and add each to the UI
        foreach (var item in playerInventory.items)
        {
            GameObject itemUI = Instantiate(inventoryItemPrefab, inventoryPanel.transform);
            itemUI.GetComponentInChildren<Image>().sprite = item.itemData.itemIcon;  // Assuming the prefab has an Image component
            itemUI.GetComponentInChildren<Text>().text = item.itemData.itemName;  // Assuming there's a Text component to display the name
        }

        Debug.Log("Inventory display updated.");
    }

    void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.onInventoryChanged -= UpdateInventoryDisplay;
        }
    }
}
