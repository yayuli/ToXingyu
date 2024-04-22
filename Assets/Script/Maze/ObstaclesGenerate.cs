using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesGenerate : MonoBehaviour
{
   
    //obstacle prefab
    [SerializeField]
    private GameObject obstaclePrefab;
    [SerializeField]
    private int numberOfObstacles = 10;

    private List<Vector2> availablePositions = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        if(MazeGenerator.Instance!=null)
        {
            availablePositions = new List<Vector2>(MazeGenerator.Instance.availablePositions);  // Copy positions from MazeGenerator
            PlaceObstaclesRandomly();
        }
    }

    void PlaceObstaclesRandomly()
    {
        for (int i = 0; i < numberOfObstacles; i++)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Vector2 obstaclePosition = availablePositions[randomIndex];
            Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);
            availablePositions.RemoveAt(randomIndex);
        }
    }
}
