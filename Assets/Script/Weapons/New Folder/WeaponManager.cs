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
    [SerializeField] private GameObject initialWeaponPrefab;  // 初始武器的预制体
    [SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private float radius = 1f;
    [SerializeField] private int maxWeapons = 6;

    private Transform player;
    private List<GameObject> weapons = new List<GameObject>();

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
