using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
<<<<<<< HEAD
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private float radius = 1f; // Distance from the player
    [SerializeField] private int maxWeapons = 6; // Maximum number of weapons

    private Transform player;
    private List<GameObject> weapons = new List<GameObject>();

    void Start()
    {
        player = GameObject.Find("Player").transform;
        AddWeapon();//start with one weapon
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && weapons.Count < maxWeapons)
        {
            AddWeapon();
        }
    }
    

    void AddWeapon()
    {
        if (weapons.Count >= maxWeapons)
            return;//prevent adding more than the maxiunm number of weapons

        float angle = 360f / maxWeapons * weapons.Count;
        Vector2 positionOffset = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
        Vector2 spawnPosition = (Vector2)player.position + positionOffset;

        GameObject newWeapon = Instantiate(weaponPrefab, spawnPosition, Quaternion.identity, transform);
        weapons.Add(newWeapon);
        newWeapon.GetComponent<Weapon>().SetOffset(positionOffset);
=======
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
>>>>>>> b0bdbdcdd17ecdd501e2b3891178de4b898c4323
    }
}
