using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] GameObject weaponPrefab;

    Transform player;
    List<Vector2> weaponPositions = new List<Vector2>();

    int spawnedWeapons = 0;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.Find("Player").transform;

        weaponPositions.Add(new Vector2(-1.2f, 1f));
        weaponPositions.Add(new Vector2(1.2f, 1f));

        weaponPositions.Add(new Vector2(-1.4f, -0.2f));
        weaponPositions.Add(new Vector2(1.4f, -0.2f));

        weaponPositions.Add(new Vector2(-1f, -0.5f));
        weaponPositions.Add(new Vector2(1f, -0.5f));

        AddWeapon();
        AddWeapon();
    }

    private void Update()
    {
        //for testing
        if (Input.GetKeyDown(KeyCode.G)) AddWeapon();
    }


    void AddWeapon()
    {
        var pos = weaponPositions[spawnedWeapons];

        var newWeapon = Instantiate(weaponPrefab, pos, Quaternion.identity);

        newWeapon.GetComponent<Weapon>().SetOffset(pos);

        spawnedWeapons++;
    }
}
