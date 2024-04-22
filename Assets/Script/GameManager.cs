using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IGenerator[] generators;
    [SerializeField] private MazeGenerator mazeGenerator;

    void Awake()
    {
        // Automatically find all IGenerator implementations in the scene
        generators = FindObjectsOfType<MonoBehaviour>().OfType<IGenerator>().ToArray();
        InitializeGenerators();
    }

    private void InitializeGenerators()
    {
        if (mazeGenerator != null)
        {
            // Ensure MazeGenerator is initialized first
            mazeGenerator.GenerateMaze(mazeGenerator.mazeRows, mazeGenerator.mazeColumns);

            // Initialize and generate for all generators found
            foreach (var generator in generators)
            {
                generator.Initialize(mazeGenerator);
                generator.Generate();
            }
        }
        else
        {
            Debug.LogError("MazeGenerator instance not found!");
        }
    }
}
