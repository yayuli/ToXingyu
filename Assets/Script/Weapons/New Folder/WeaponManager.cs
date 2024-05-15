using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Ranged,
    Melee
}

public class WeaponManager : MonoBehaviour
{
    public static WeaponType selectedWeaponType = WeaponType.Ranged;  // 默认选择远程武器

    [SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private float radius = 1f;
    [SerializeField] private int maxWeapons = 6;

    private Transform player;
    private List<GameObject> weapons = new List<GameObject>();

    void Start()
    {
        player = GameObject.Find("Player").transform;
        AddWeapon(selectedWeaponType);  // 根据玩家选择的武器类型生成初始武器
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && weapons.Count < maxWeapons)
        {
            AddWeapon(WeaponType.Ranged);  // 按下G键并且武器数量未达到上限时增加一把远程武器
        }
    }

    void AddWeapon(WeaponType weaponType)
    {
        if (weapons.Count >= maxWeapons)
            return;

        float angle = 360f / maxWeapons * weapons.Count;
        Vector2 positionOffset = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
        Vector2 spawnPosition = (Vector2)player.position + positionOffset;

        GameObject weaponPrefab = GetWeaponPrefab(weaponType);
        GameObject newWeapon = Instantiate(weaponPrefab, spawnPosition, Quaternion.identity, transform);
        weapons.Add(newWeapon);
        newWeapon.GetComponent<WeaponBase>().SetOffset(positionOffset);
    }

    GameObject GetWeaponPrefab(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Ranged:
                return weaponPrefabs[0];
            case WeaponType.Melee:
                return weaponPrefabs[1];
            default:
                return null;
        }
    }
}
