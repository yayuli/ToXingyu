using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// 负责管理玩家的武器选择和生成武器。这里是武器类型被选择和实例化的地方。
/// 
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
    public Sprite Image;

    private Transform player;
    [SerializeField] private List<GameObject> weapons = new List<GameObject>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        player = GameObject.Find("Player").transform;
        AddInitialWeapon();  // 添加初始武器
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
        else
        {
            Debug.LogError("Initial weapon prefab is not assigned.");
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
    }

    public void AddWeapon(GameObject weaponPrefab)
    {
        ItemData newItemData = weaponPrefab.GetComponent<Item>().itemData;

        // 查找是否存在同类型的武器
        foreach (GameObject w in weapons)
        {
            ItemData existingData = w.GetComponent<WeaponBase>().GetData();
            if (existingData.itemName == newItemData.itemName)
            {
                // 合并武器逻辑
                MergeWeapons(existingData, newItemData);
                return;
            }
        }

        // 如果没有可合并的，添加新武器
        InstantiateAndAddWeapon(weaponPrefab);
    }

    void MergeWeapons(ItemData existingData, ItemData newData)
    {
        GameObject weaponObject = weapons.First(w => w.GetComponent<WeaponBase>().GetData().itemName == existingData.itemName);
        existingData.level++;
        existingData.attackPower += newData.attackPower;
        existingData.range += newData.range;
        existingData.effectMagnitude += newData.effectMagnitude;  // 递增伤害值

        // 更新颜色逻辑
        if (existingData.level == 2)
            existingData.weaponColor = Color.blue;
        else if (existingData.level == 3)
            existingData.weaponColor = Color.red;

        // 更新武器实例
        weaponObject.GetComponent<WeaponBase>().UpdateWeaponInstance();
    }

    public void MergeWeapons(GameObject selectedWeapon)
    {
        // Find a weapon to merge with and merge them
        // This is a simplified placeholder logic
        foreach (var weapon in weapons)
        {
            if (weapon != selectedWeapon && weapon.GetComponent<Item>().itemData.itemName == selectedWeapon.GetComponent<Item>().itemData.itemName)
            {
                // Simplified merging logic: increase power
                selectedWeapon.GetComponent<Item>().itemData.attackPower += 5;
                Destroy(weapon); // Destroy the merged weapon
                break;
            }
        }
    }

    public void RecycleWeapon(GameObject selectedWeapon)
    {
        // Remove and recycle the weapon, e.g., converting it into resources
        weapons.Remove(selectedWeapon);
        Destroy(selectedWeapon);
    }


    void InstantiateAndAddWeapon(GameObject weaponPrefab)
    {
        float angle = 360f / maxWeapons * weapons.Count;
        Vector2 positionOffset = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
        Vector2 spawnPosition = (Vector2)player.position + positionOffset;

        GameObject newWeapon = Instantiate(weaponPrefab, spawnPosition, Quaternion.identity, player);
        weapons.Add(newWeapon);

        // 初始化新武器
        newWeapon.GetComponent<WeaponBase>().Initialize(weaponPrefab.GetComponent<Item>().itemData);
        newWeapon.GetComponent<WeaponBase>().SetOffset(positionOffset);
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
}
