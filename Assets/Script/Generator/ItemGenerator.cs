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
        public int incrementPerLevel;
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
        // 获取当前波次编号
        int currentWave = WaveManager.Instance.WaveNum;

        // 根据波次增加物品数量
        int itemCount = config.count + (currentWave - 1) * config.incrementPerLevel; 

        for (int i = 0; i < itemCount; i++)
        {
            Vector2? position = PositionManager.Instance.GetRandomPosition(false); // 不允许重用位置
            if (position != null)
            {
                Item newItem = Instantiate(config.prefab, position.Value, Quaternion.identity);
            }
        }
    }

}

