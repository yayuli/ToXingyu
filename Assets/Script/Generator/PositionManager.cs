using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PositionManager : MonoBehaviour
{
    public static PositionManager Instance { get; private set; }
    private List<Vector2> availablePositions;
    private HashSet<Vector2> occupiedPositions;

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
        occupiedPositions = new HashSet<Vector2>();
    }

    public Vector2?GetRandomPosition(bool allowOverlap)
    {

        if (availablePositions.Count == 0)
            return null;

        int index = Random.Range(0, availablePositions.Count);
        Vector2 position = availablePositions[index];

        if (!allowOverlap)
        {
            availablePositions.RemoveAt(index);
            occupiedPositions.Add(position);
        }

        return position;
    }

    public void ReleasePosition(Vector2 position)
    {
        if (occupiedPositions.Contains(position))
        {
            occupiedPositions.Remove(position);
            availablePositions.Add(position);
        }
    }

    public void ResetPositions(List<Vector2> positions)
    {
        availablePositions = new List<Vector2>(positions);
        occupiedPositions.Clear();
    }

}
