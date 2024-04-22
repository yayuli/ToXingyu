using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesGenerate : MonoBehaviour, IGenerator
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private int numberOfObstacles = 10;

    private MazeGenerator mazeGenerator;

    public void Initialize(MazeGenerator mazeGenerator)
    {
        this.mazeGenerator = mazeGenerator;
       
    }

    public void Generate()
    {
        PlaceObstaclesRandomly();
    }
    private void PlaceObstaclesRandomly()
    {
        List<Vector2> availablePositions = new List<Vector2>(mazeGenerator.availablePositions);
        for (int i = 0; i < numberOfObstacles && availablePositions.Count > 0; i++)
        {
            if (availablePositions.Count > 0)
            {
                int randomIndex = Random.Range(0, availablePositions.Count);
                Vector2 obstaclePosition = availablePositions[randomIndex];
                Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);
                availablePositions.RemoveAt(randomIndex);
            }
                
        }
    }
}

