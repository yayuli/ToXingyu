using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class WeaponPanel : MonoBehaviour
{
    public static WeaponPanel instance;
    public GameObject detailPanel; // 详情面板
    public TMP_Text detailText; // 显示详细信息的文本
    public Button upgradeButton; // 升级按钮
    public Button sellButton; // 卖出按钮
    public Button cancelButton; // 取消按钮
    public Image[] weaponSlots; // 武器槽数组

    private WeaponManager weaponManager;
    public GameObject selectedWeaponPrefab;  // 当前选中的武器预制体

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

    public void SelectWeapon(GameObject weaponPrefab)
    {
        selectedWeaponPrefab = weaponPrefab;
    }

    void Start()
    {
        weaponManager = WeaponManager.instance;
        InitializeWeaponSlots();
        detailPanel.SetActive(false);//make sure the details panel is hidden initially
        UpdateWeaponSlotsDisplay();//display statusof weapon slots during initialization
        cancelButton.onClick.AddListener(() => detailPanel.SetActive(false));  // 只在Start中添加一次
    }
  

    void InitializeWeaponSlots()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            int index = i; // Important for capturing the correct loop variable in the lambda expression
            weaponSlots[i].gameObject.AddComponent<Button>().onClick.AddListener(() => OnWeaponSelect(index));
            //weaponSlots[i].gameObject.SetActive(true);
        }
    }

    public void UpdateWeaponSlotsDisplay()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (i < weaponManager.Weapons.Count && weaponManager.Weapons[i] != null)
            {
                Item item = weaponManager.Weapons[i].GetComponent<Item>();
                if (item != null && item.itemData != null)
                {
                    weaponSlots[i].sprite = item.itemData.itemIcon;
                    weaponSlots[i].gameObject.SetActive(true);
                }
                else
                {
                    weaponSlots[i].gameObject.SetActive(false);
                }
            }
            else
            {
                weaponSlots[i].gameObject.SetActive(false);
            }
        }
    }


    public void OnWeaponSelect(int index)
    {
        if (index < weaponManager.Weapons.Count && weaponManager.Weapons[index] != null)
        {
            GameObject weapon = weaponManager.Weapons[index];
            detailText.text = weapon.GetComponent<Item>().itemData.description;
            detailPanel.SetActive(true);

            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => UpgradeWeapon(weapon));
            sellButton.onClick.RemoveAllListeners();
            sellButton.onClick.AddListener(() => SellWeapon(weapon));
        }
        else
        {
            detailPanel.SetActive(false);
        }
    }


    public void HideDetailPanel()
    {
        detailPanel.SetActive(false);
    }



    public void UpgradeWeapon(GameObject weapon)
    {
        if (selectedWeaponPrefab != null)
        {
            weaponManager.TryMergeWeapons(selectedWeaponPrefab);
        }
        else
        {
            Debug.LogError("No weapon selected for upgrade.");
        }
    }

    void SellWeapon(GameObject weapon)
    {
        // Sell logic here
        Debug.Log("Selling weapon...");
        //weaponManager.RecycleWeapon(weapon);
    }
}
