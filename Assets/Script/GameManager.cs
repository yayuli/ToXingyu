using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MazeGenerator mazeGenerator;
    [SerializeField] private ObstaclesGenerate obstaclesGenerator;
    [SerializeField] private WeaponsGenerate weaponsGenerator;
    [SerializeField] private EnemysGenerate enemysGenerator;

    void Awake()
    {
        // Ensure MazeGenerator is initialized first
        mazeGenerator.GenerateMaze(mazeGenerator.mazeRows, mazeGenerator.mazeColumns);

        // Inject MazeGenerator into other components
        obstaclesGenerator.Initialize(mazeGenerator);
        weaponsGenerator.Initialize(mazeGenerator);
        enemysGenerator.Initialize(mazeGenerator);
    }
}

