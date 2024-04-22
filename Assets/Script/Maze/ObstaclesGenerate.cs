using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesGenerate : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private int numberOfObstacles = 10;

    private MazeGenerator mazeGenerator;

    public void Initialize(MazeGenerator mazeGenerator)
    {
        this.mazeGenerator = mazeGenerator;
        PlaceObstaclesRandomly();
    }

    private void PlaceObstaclesRandomly()
    {
        List<Vector2> availablePositions = new List<Vector2>(mazeGenerator.availablePositions);
        for (int i = 0; i < numberOfObstacles && availablePositions.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Vector2 obstaclePosition = availablePositions[randomIndex];
            Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);
            availablePositions.RemoveAt(randomIndex);
        }
    }
}

