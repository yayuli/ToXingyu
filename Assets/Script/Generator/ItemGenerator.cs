using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour, IGenerator
{
    [System.Serializable]
    public struct ItemConfig
    {
        public Item prefab;
        public int count;
        public float spawnDelay;
    }

    [SerializeField]
    private MazeGenerator mazeGenerator;
    [SerializeField]
    private ItemConfig[] itemsToGenerate;

    private bool isGenerating = false; 

    public void Initialize(MazeGenerator mazeGenerator)
    {
        this.mazeGenerator = mazeGenerator;
        if (!isGenerating)
        {
            StartCoroutine(GenerateItemsWithDelay());
        }
    }

    public void Generate()
    {
        if (isGenerating)
        {
            return;
        }

        isGenerating = true;
        foreach (var item in itemsToGenerate)
        {
            PlaceItemsRandomly(item);
        }
        isGenerating = false;
    }

    private IEnumerator GenerateItemsWithDelay()
    {
        isGenerating = true;
        foreach (var item in itemsToGenerate)
        {
            yield return new WaitForSeconds(item.spawnDelay);
            PlaceItemsRandomly(item);
        }
        isGenerating = false;
    }

    private void PlaceItemsRandomly(ItemConfig config)
    {
        for (int i = 0; i < config.count; i++)
        {
            Vector2? position = PositionManager.Instance.GetRandomPosition(false);//dont not allow the reuse position
            if (position != null)
            {
                Item newItem = Instantiate(config.prefab, position.Value, Quaternion.identity);

            }
        }
    }
}

