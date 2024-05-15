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

    // 用于存储所有可能的ItemData对象
    public List<ItemData> allItems;

    void Start()
    {
        // 更新所有三个选择框
        UpdateAllDisplays();
    }

    // 更新所有三个选择框的显示
    private void UpdateAllDisplays()
    {
        // 随机获取三个不同的ItemData，确保不重复
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
    private void UpdateButtonDisplay(ItemData item, int displayIndex)
    {
        if (item == null || displayIndex < 0 || displayIndex >= 3) return;

        upgradeDescTexts[displayIndex].text = item.description;
        weaponIcons[displayIndex].sprite = item.itemIcon;
        nameLevelTexts[displayIndex].text = $"{item.itemName} - Lvl";  // 根据实际等级系统调整
    }
}
