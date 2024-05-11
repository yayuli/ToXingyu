using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Image[] equippedImages; // 这是用于显示技能图标的Image数组
    [SerializeField] private Player player;      // 玩家的引用

    private void Update()
    {
        UpdateEquippedItemUI();
    }

    void UpdateEquippedItemUI()
    {
        // 更新技能图标，确保每个技能都有对应的Image显示
        for (int i = 0; i < player.equippedItems.Length; i++)
        {
            if (i < equippedImages.Length)
            {
                equippedImages[i].sprite = player.equippedItems[i].itemIcon; // 设置技能图标
                equippedImages[i].gameObject.SetActive(true); // 确保对应的gameObject是激活的
            }
        }

        // 如果技能数量少于Image元素的数量，隐藏多余的Image
        for (int i = player.equippedItems.Length; i < equippedImages.Length; i++)
        {
            equippedImages[i].gameObject.SetActive(false); // 隐藏多余的gameObject
        }
    }
}
