using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 负责管理玩家的武器选择和生成武器。这里是武器类型被选择和实例化的地方。
/// </summary>
public enum WeaponType
{
    Ranged,
    Melee
}

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    public List<GameObject> Weapons => weapons;

    [SerializeField] private GameObject initialWeaponPrefab;  // 初始武器的预制体
    [SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private float radius = 1f;
    [SerializeField] private int maxWeapons = 6;

    public WeaponPanel weaponPanel;

    private Transform player;
    private List<GameObject> weapons = new List<GameObject>();

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
        player = GameObject.Find("Player").transform;
        AddInitialWeapon();  // 添加初始武器
        weaponPanel = FindObjectOfType<WeaponPanel>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && weapons.Count < maxWeapons)
        {
            AddWeapon(WeaponType.Ranged);  // Optionally trigger this via UI instead
        }
    }

    // Handles adding the initial weapon
    void AddInitialWeapon()
    {
        if (initialWeaponPrefab != null)
        {
            AddWeapon(initialWeaponPrefab);
        }
        
    }

    // Overload to handle weapon type
    public void AddWeapon(WeaponType type)
    {
        GameObject weaponPrefab = GetWeaponPrefab(type);
        if (weaponPrefab)
        {
            AddWeapon(weaponPrefab);
        }
        weaponPanel.UpdateWeaponSlotsDisplay();
    }

    // Overload to handle direct GameObject instantiation
    public void AddWeapon(GameObject weaponPrefab)
    {
        if (weapons.Count >= maxWeapons)
        {
            Debug.Log("Reached maximum number of weapons.");
            return;
        }

        float angle = 360f / maxWeapons * weapons.Count;
        Vector2 positionOffset = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
        Vector2 spawnPosition = (Vector2)player.position + positionOffset;

        GameObject newWeapon = Instantiate(weaponPrefab, spawnPosition, Quaternion.identity, player);
        weapons.Add(newWeapon);

        // Ensure the weapon is correctly offset and initialized
        newWeapon.GetComponent<WeaponBase>().SetOffset(positionOffset);
        newWeapon.GetComponent<WeaponBase>().Initialize(weaponPrefab.GetComponent<Item>().itemData);

        weaponPanel.UpdateWeaponSlotsDisplay();
        
    }



    GameObject GetWeaponPrefab(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Ranged: return weaponPrefabs[0];
            case WeaponType.Melee: return weaponPrefabs[1];
            default: return null;
        }
    }

    public void TryMergeWeapons()
    {
        Dictionary<string, List<GameObject>> weaponGroups = new Dictionary<string, List<GameObject>>();

        // 分组同类型的武器
        foreach (var weapon in weapons)
        {
            Item item = weapon.GetComponent<Item>();
            string weaponKey = $"{item.itemData.name}_{item.itemData.level}";

            if (!weaponGroups.ContainsKey(weaponKey))
            {
                weaponGroups[weaponKey] = new List<GameObject>();
            }
            weaponGroups[weaponKey].Add(weapon);
        }

        // 检查每组武器是否可以合并
        foreach (var group in weaponGroups)
        {
            if (group.Value.Count >= 2)  // 如果有两个以上同样的武器
            {
                // 合并武器，这里简化为只合并两个
                GameObject weaponToUpgrade = group.Value[0];
                GameObject weaponToRemove = group.Value[1];

                // 执行升级逻辑
                UpgradeWeapon(weaponToUpgrade);

                // 从列表和场景中移除第二个武器
                weapons.Remove(weaponToRemove);
                Destroy(weaponToRemove);

                Debug.Log("Weapons merged and upgraded.");
                break;  // 每次只合并一对武器
            }
        }
        weaponPanel.UpdateWeaponSlotsDisplay();
    }

    void UpgradeWeapon(GameObject weapon)
    {
        Item item = weapon.GetComponent<Item>();
        item.itemData.level++;  // 假设每个武器都有一个级别
        item.itemData.attackPower += 10;  // 增加攻击力，根据实际需求调整

        // 根据等级选择颜色
        Color newColor = Color.white;  // 默认白色
        if (item.itemData.level == 2)
        {
            newColor = Color.blue;
        }
        else if (item.itemData.level == 3)
        {
            newColor = Color.red;
        }

        // 更新颜色
        UpdateColor(weapon, newColor);
    }

    void UpdateColor(GameObject weapon, Color newColor)
    {
        Renderer weaponRenderer = weapon.GetComponent<Renderer>();
        if (weaponRenderer != null)
        {
            weaponRenderer.material.color = newColor;
        }

        // 递归更新子对象的颜色
        foreach (Transform child in weapon.transform)
        {
            UpdateColor(child.gameObject, newColor);
        }
    }

}
