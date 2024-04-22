using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysGenerate : MonoBehaviour
{
    // enemys prefab
    [SerializeField]private GameObject EnemysPrefab;
    [SerializeField]private int numberOfEnemys = 4; // bosses number

    private MazeGenerator mazeGenerator;

    public void Initialize(MazeGenerator mazeGenerator)
    {
        this.mazeGenerator = mazeGenerator;
        PlaceEnemysRandomly();
    }
   

    //bosses randomly generated method
    void PlaceEnemysRandomly()
    {
        List<Vector2> availablePositions = new List<Vector2>(mazeGenerator.availablePositions);
        for (int i = 0; i < numberOfEnemys && availablePositions.Count > 0; i++)
        {
            if (availablePositions.Count > 0)
            {
                int randomIndex = Random.Range(0, availablePositions.Count);
                Vector2 enemysPosition = availablePositions[randomIndex];
                Instantiate(EnemysPrefab, enemysPosition, Quaternion.identity);
                availablePositions.RemoveAt(randomIndex);
            }
        }
    }
}
