using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionUI : MonoBehaviour
{
    public static SelectionUI instance;

    [Header("UI Components")]
    public TMP_Text[] upgradeDescTexts = new TMP_Text[3];
    public Image[] weaponIcons = new Image[3];
    public TMP_Text[] nameLevelTexts = new TMP_Text[3];
    public TMP_Text[] priceTexts = new TMP_Text[3];
    public Button[] purchaseButtons = new Button[3];
    public Button refreshButton;
    public Button exitButton;

    [Header("Item and Weapon Data")]
    public List<GameObject> allItems; // Store all item prefabs including weapons and other items
    public List<GameObject> rangedWeapons; // List of ranged weapon prefabs
    public List<GameObject> meleeWeapons; // List of melee weapon prefabs

    [Header("Weapon Display Panel")]
    public GameObject weaponPanel; // 武器展示Panel
    public Image[] weaponDisplaySlots = new Image[6]; // 武器槽数组，假设最多展示6个武器

    [Header("Items display panel")]
    [SerializeField] private GameObject itemUIPanel; // 预先绑定的UI面板
    [SerializeField] private Image[] itemImage;        // 物品图标
    private int currentItemIndex = 0;

    public WeaponManager weaponManager;  // 在 Inspector 中设置这个引用

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
        UpdateAllDisplays();
        weaponManager = FindObjectOfType<WeaponManager>();
    
        if (weaponManager == null)
        {
        Debug.LogError("WeaponManager is not found in the scene.");
        }

        refreshButton.onClick.AddListener(RefreshDisplays);//refresh button event
        exitButton.onClick.AddListener(CloseSelectionUI);
    }

    private void RefreshDisplays()
    {
        Debug.Log("Refresh button clicked.");
        int refreshCost = ExperienceLevelController.instance.CalculateRefreshCost();
        Debug.Log("Calculated refresh cost: " + refreshCost);

        if (ExperienceLevelController.instance.CanAfford(refreshCost))
        {
            Debug.Log("Can afford refresh.");
            ExperienceLevelController.instance.SpendExperience(refreshCost);
            ExperienceLevelController.instance.IncrementRefreshCount();
            UpdateAllDisplays();
            Debug.Log("Displays updated after refresh.");
        }
        else
        {
            Debug.Log("Not enough experience points to refresh.");
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

    //handler items display panel ui
    private void UpdateItemUI(ItemData itemData)
    {
        if (itemData.itemType == ItemData.ItemType.MeleeWeapon || itemData.itemType == ItemData.ItemType.RangedWeapon)
        {
            return;
        }

        if (itemData != null && currentItemIndex < itemImage.Length)
        {
            itemImage[currentItemIndex].sprite = itemData.itemIcon;
            itemImage[currentItemIndex].gameObject.SetActive(true);
            currentItemIndex++;
        }

        itemUIPanel.SetActive(currentItemIndex > 0);
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
        priceTexts[displayIndex].text = $"{itemData.cost}XP";

        purchaseButtons[displayIndex].onClick.RemoveAllListeners();
        purchaseButtons[displayIndex].onClick.AddListener(() => PurchaseItem(itemPrefab, itemData.cost));
    }

    // Handles item purchase logic
    private void PurchaseItem(GameObject itemPrefab, int cost)
    {
        if (Player.instance == null || !ExperienceLevelController.instance.CanAfford(cost))
        {
           // Debug.LogError("Player.instance is null or not enough experience.");
            return;
        }

        ExperienceLevelController.instance.SpendExperience(cost); // 消费经验值

        Item itemScript = itemPrefab.GetComponent<Item>();
        if (itemScript == null || itemScript.itemData == null)
        {
          //  Debug.LogError("Item prefab is missing Item component or Item Data.");
            return;
        }

        ItemData itemData = itemScript.itemData;
        Player.instance.ApplyItemEffect(itemData);
        UpdateItemUI(itemData);//update item display ui

        // 如果物品是武器，添加到玩家
        if (itemData.itemType == ItemData.ItemType.MeleeWeapon || itemData.itemType == ItemData.ItemType.RangedWeapon)
        {
            weaponManager.AddWeapon(itemPrefab);  // 这里添加武器到玩家
        }

        else if (itemData.itemType == ItemData.ItemType.Bomb)
        {
            BombItem bombItem = itemPrefab.GetComponent<BombItem>();
            if (bombItem != null)
            {
                Player.instance.AddItem(bombItem.bombsToAdd); // 增加炸弹数量
            }
            else
            {
                Debug.LogError("BombItem component is missing on the prefab.");
            }
        }
        SFXManager.instance.PlaySFXPitched(9);
        //ContinueGame();
    }


    private void CloseSelectionUI()
    {
        gameObject.SetActive(false); // Hide selection UI
        ExperienceLevelController.instance.ResumeGame(); // Resume game logic
        ContinueGame();
    }

    // Continues the game after an item is purchased
    private void ContinueGame()
    {
        gameObject.SetActive(false); // Hide selection UI
        ExperienceLevelController.instance.ResumeGame(); // Resume game logic
    }
}
