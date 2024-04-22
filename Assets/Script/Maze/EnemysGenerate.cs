using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysGenerate : MonoBehaviour
{
    // enemys prefab
    [SerializeField]
    private GameObject EnemysPrefab;
    [SerializeField]
    private int numberOfEnemys = 4; // bosses number

    private List<Vector2> availablePositions = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        if(MazeGenerator.Instance!=null)
        {
            availablePositions = new List<Vector2>(MazeGenerator.Instance.availablePositions);  // Copy positions from MazeGenerator
            PlaceEnemysRandomly();
        }
    }

    //bosses randomly generated method
    void PlaceEnemysRandomly()
    {
        for (int i = 0; i < numberOfEnemys; i++)
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
