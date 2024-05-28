using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    public static PositionManager Instance { get; private set; }
    private List<Vector2> availablePositions;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Initialize(List<Vector2> positions)
    {
        availablePositions = new List<Vector2>(positions);
    }

    public Vector2? GetRandomPosition()
    {
        if (availablePositions.Count == 0)
            return null;

        int index = Random.Range(0, availablePositions.Count);
        return availablePositions[index];
    }

    public void ResetPositions(List<Vector2> positions)
    {
        availablePositions = new List<Vector2>(positions);
    }
}
