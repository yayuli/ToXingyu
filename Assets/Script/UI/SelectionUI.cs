using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionUI : MonoBehaviour
{
    public TMP_Text[] upgradeDescTexts = new TMP_Text[3];
    public Image[] weaponIcons = new Image[3];
    public TMP_Text[] nameLevelTexts = new TMP_Text[3];
    public Button[] purchaseButtons = new Button[3];

    // 用于存储所有可能的Item预制件
    public List<GameObject> allItems;

    void Start()
    {
        // 更新所有三个选择框
        UpdateAllDisplays();
    }

    // 更新所有三个选择框的显示
    private void UpdateAllDisplays()
    {
        HashSet<int> chosenIndices = new HashSet<int>();
        while (chosenIndices.Count < 3)
        {
            chosenIndices.Add(Random.Range(0, allItems.Count));
        }

        int i = 0;
        foreach (int index in chosenIndices)
        {
            UpdateButtonDisplay(allItems[index], i);
            i++;
        }
    }

    // 更新指定选择框的显示
    private void UpdateButtonDisplay(GameObject itemPrefab, int displayIndex)
    {
        if (itemPrefab == null || displayIndex < 0 || displayIndex >= 3) return;

        Item itemComponent = itemPrefab.GetComponent<Item>();
        if (itemComponent == null || itemComponent.itemData == null)
        {
            Debug.LogError("Item prefab is missing Item component or ItemData");
            return;
        }

        ItemData itemData = itemComponent.itemData;

        upgradeDescTexts[displayIndex].text = itemData.description;
        weaponIcons[displayIndex].sprite = itemData.itemIcon;
        nameLevelTexts[displayIndex].text = $"{itemData.itemName} - Lvl";

        // 按钮事件
        purchaseButtons[displayIndex].onClick.RemoveAllListeners();
        purchaseButtons[displayIndex].onClick.AddListener(() => PurchaseItem(itemPrefab));
    }

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
            Debug.LogError("Item prefab is missing Item component or ItemData.");
            return;
        }

        ItemData itemData = itemScript.itemData;

        // apply item effect
        Player.instance.ApplyItemEffect(itemData);

        //continue game
        ContinueGame();
    }

    private void ContinueGame()
    {
        //hide selection UI 
        gameObject.SetActive(false);

        //call the Experencerleve script's continue game method
        ExperienceLevelController.instance.ResumeGame();
    }
}
