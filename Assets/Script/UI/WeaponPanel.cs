using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class WeaponSlotUI
{
    public GameObject detailPanel; // 每个slot的详情面板
    public TMP_Text detailText; // 显示详细信息的文本
    public TMP_Text upgradeDetailText;
    public Button upgradeButton; // 升级按钮
    public Button sellButton; // 卖出按钮
    public Button cancelButton; // 取消按钮
}

public class WeaponPanel : MonoBehaviour
{
    public static WeaponPanel instance;
    public Image[] weaponSlots; // 武器槽数组
    public WeaponSlotUI[] weaponSlotsUI; // 每个武器槽的UI组件

    private WeaponManager weaponManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 使对象跨场景持续存在
        }
        else
        {
            Destroy(gameObject); // 确保不创建重复实例
        }
    }

    void Start()
    {
        weaponManager = WeaponManager.instance;
        InitializeWeaponSlots();
        UpdateWeaponSlotsDisplay(); // 更新武器槽的显示状态
    }
   
    void InitializeWeaponSlots()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            int index = i;  // Local copy of the loop variable for use in lambda expressions
            weaponSlotsUI[index].detailPanel.SetActive(false); // 初始时隐藏所有详情面板
            weaponSlotsUI[index].cancelButton.onClick.AddListener(() => weaponSlotsUI[index].detailPanel.SetActive(false));
            weaponSlotsUI[index].upgradeButton.onClick.AddListener(() => UpgradeWeapon(weaponManager.Weapons[index]));
            weaponSlotsUI[index].sellButton.onClick.AddListener(() => SellWeapon(weaponManager.Weapons[index]));

            // 为每个武器槽添加点击事件来显示详情面板
            if (weaponSlots[index].gameObject.GetComponent<Button>() == null)
            {
                weaponSlots[index].gameObject.AddComponent<Button>();
            }
            weaponSlots[index].gameObject.GetComponent<Button>().onClick.AddListener(() => OnWeaponSelect(index));
        }
    }

    public void UpdateWeaponSlotsDisplay()
    {
        Dictionary<string, int> weaponCounts = new Dictionary<string, int>();

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (i < weaponManager.Weapons.Count && weaponManager.Weapons[i] != null)
            {
                Item item = weaponManager.Weapons[i].GetComponent<Item>();
                weaponSlots[i].sprite = item.itemData.itemIcon;
                weaponSlots[i].gameObject.SetActive(true);
                SFXManager.instance.PlaySFXPitched(9);

                if (!weaponCounts.ContainsKey(item.itemData.name))
                {
                    weaponCounts[item.itemData.name] = 0;
                }
                weaponCounts[item.itemData.name]++;
            }
            else
            {
                weaponSlots[i].gameObject.SetActive(false);
            }
        }

        foreach (var count in weaponCounts)
        {
            if (count.Value >= 2)
            {
                //Debug.Log($"You have {count.Value} weapons of type {count.Key} ready to merge.");
            }
        }
    }

    public void OnWeaponSelect(int index)
    {
        foreach (var slotUI in weaponSlotsUI)
        {
            slotUI.detailPanel.SetActive(false); // 隐藏所有面板
        }

        if (index < weaponManager.Weapons.Count && weaponManager.Weapons[index] != null)
        {
            WeaponSlotUI selectedSlotUI = weaponSlotsUI[index];
            selectedSlotUI.detailPanel.SetActive(true);
            Item item = weaponManager.Weapons[index].GetComponent<Item>();
            selectedSlotUI.detailText.text = item.itemData.description;

            // 更新升级信息
            string upgradeInfo = $"<color=#000000>Damage:</color> <color=#33900E>{item.itemData.CurrentDamage} " +
                             $"(<color=#900D46>+{item.itemData.damageIncreasePerLevel}</color>)\n" +
                             $"<color=#000000>Speed:</color> <color=#33900E>{item.itemData.CurrentSpeed:F1}" +
                             $"(<color=#900D46>+{item.itemData.speedIncreasePerLevel:F1}</color>)\n" +
                             $"<color=#000000>Range:</color> <color=#33900E>{item.itemData.CurrentRange:F1} " +
                             $"(<color=#900D46>+{item.itemData.rangeIncreasePerLevel:F1}</color>)";

            selectedSlotUI.upgradeDetailText.text = upgradeInfo;
        }
    }

    public void UpgradeWeapon(GameObject weapon)
    {
        if (weapon != null)
        {
            Debug.Log("Upgrading weapon: " + weapon.name);
            weaponManager.TryMergeWeapons();
        }
        else
        {
            Debug.LogError("No weapon selected for upgrade.");
        }
    }

    void SellWeapon(GameObject weapon)
    {
        if (weapon != null)
        {
            Item item = weapon.GetComponent<Item>();

            ExperienceLevelController.instance.AddExperience(item.itemData.cost);

            weaponManager.RemoveWeapon(weapon);

            // 销毁武器对象
            Destroy(weapon);

            // 更新UI或其他逻辑
            UpdateWeaponSlotsDisplay();
        }
    }
}
