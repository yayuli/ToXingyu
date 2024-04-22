using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsGenerate : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private int numberOfWeapons = 3;

    private MazeGenerator mazeGenerator;

    public void Initialize(MazeGenerator mazeGenerator)
    {
        this.mazeGenerator = mazeGenerator;
        PlaceWeaponsRandomly();
    }

    private void PlaceWeaponsRandomly()
    {
        List<Vector2> availablePositions = new List<Vector2>(mazeGenerator.availablePositions);
        for (int i = 0; i < numberOfWeapons && availablePositions.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            GameObject weaponPrefab = weaponPrefabs[Random.Range(0, weaponPrefabs.Length)];
            Instantiate(weaponPrefab, availablePositions[randomIndex], Quaternion.identity);
            availablePositions.RemoveAt(randomIndex);
        }
    }
}
