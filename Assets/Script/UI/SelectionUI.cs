using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionUI : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_Text[] upgradeDescTexts = new TMP_Text[3];
    public Image[] weaponIcons = new Image[3];
    public TMP_Text[] nameLevelTexts = new TMP_Text[3];
    public Button[] purchaseButtons = new Button[3];

    [Header("Item and Weapon Data")]
    public List<GameObject> allItems; // Store all item prefabs including weapons and other items
    public List<GameObject> rangedWeapons; // List of ranged weapon prefabs
    public List<GameObject> meleeWeapons; // List of melee weapon prefabs

    public WeaponManager weaponManager;  // 在 Inspector 中设置这个引用


    void Start()
    {
        UpdateAllDisplays();
        weaponManager = FindObjectOfType<WeaponManager>();
    
        if (weaponManager == null)
        {
        Debug.LogError("WeaponManager is not found in the scene.");
        }
    }

    // Update display for all selection slots
    private void UpdateAllDisplays()
    {
        List<GameObject> combinedItems = new List<GameObject>(allItems);
        combinedItems.AddRange(rangedWeapons);
        combinedItems.AddRange(meleeWeapons);

        HashSet<int> chosenIndices = new HashSet<int>();
        while (chosenIndices.Count < 3 && chosenIndices.Count < combinedItems.Count)
        {
            chosenIndices.Add(Random.Range(0, combinedItems.Count));
        }

        int i = 0;
        foreach (int index in chosenIndices)
        {
            UpdateButtonDisplay(combinedItems[index], i);
            i++;
        }
    }

    // Updates the display for a single selection slot
    private void UpdateButtonDisplay(GameObject itemPrefab, int displayIndex)
    {
        if (itemPrefab == null || displayIndex < 0 || displayIndex >= 3) return;

        Item itemComponent = itemPrefab.GetComponent<Item>();
        if (itemComponent == null || itemComponent.itemData == null)
        {
            Debug.LogError("Item prefab is missing Item component or Item Data");
            return;
        }

        ItemData itemData = itemComponent.itemData;
        upgradeDescTexts[displayIndex].text = itemData.description;
        weaponIcons[displayIndex].sprite = itemData.itemIcon;
        nameLevelTexts[displayIndex].text = $"{itemData.itemName} - Lvl";

        purchaseButtons[displayIndex].onClick.RemoveAllListeners();
        purchaseButtons[displayIndex].onClick.AddListener(() => PurchaseItem(itemPrefab));
    }

    // Handles item purchase logic
    private void PurchaseItem(GameObject itemPrefab)
    {
        if (Player.instance == null)
        {
            Debug.LogError("Player.instance is null.");
            return;
        }

        Item itemScript = itemPrefab.GetComponent<Item>();
        if (itemScript == null || itemScript.itemData == null)
        {
            Debug.LogError("Item prefab is missing Item component or Item Data.");
            return;
        }

        ItemData itemData = itemScript.itemData;
        Player.instance.ApplyItemEffect(itemData);

        // 如果物品是武器，添加到玩家
        if (itemData.itemType == ItemData.ItemType.MeleeWeapon || itemData.itemType == ItemData.ItemType.RangedWeapon)
        {
            weaponManager.AddWeapon(itemPrefab);  // 这里添加武器到玩家
        }
        ContinueGame();
    }

    // Continues the game after an item is purchased
    private void ContinueGame()
    {
        gameObject.SetActive(false); // Hide selection UI
        ExperienceLevelController.instance.ResumeGame(); // Resume game logic
    }
}
