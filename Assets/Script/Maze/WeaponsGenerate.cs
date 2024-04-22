using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsGenerate : MonoBehaviour
{

    //Weapon prefabs
    [SerializeField]
    private GameObject[] weaponPrefabs;
    [SerializeField]
    private int numberOfWeapons = 3;

    private List<Vector2> availablePositions = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        if(MazeGenerator.Instance!=null)
        {
            availablePositions = new List<Vector2>(MazeGenerator.Instance.availablePositions);  // Copy positions from MazeGenerator
            PlaceWeaponsRandomly();
        }
    }

    //weapons randomly generated method
    void PlaceWeaponsRandomly()
    {
        for (int i = 0; i < numberOfWeapons; i++)
        {
            if (availablePositions.Count > 0)
            {
                int randomIndex = Random.Range(0, availablePositions.Count);
                Vector2 weaponPosition = availablePositions[randomIndex];
                GameObject weaponPrefab = weaponPrefabs[Random.Range(0, weaponPrefabs.Length)];
                Instantiate(weaponPrefab, weaponPosition, Quaternion.identity);
                availablePositions.RemoveAt(randomIndex);
            }
        }
    }

}
