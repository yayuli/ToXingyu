using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is designed to handle the generation of various items in the game
// It implements the IGenerator interface to ensure it can be managed by a central game manager
public class ItemGenerator : MonoBehaviour, IGenerator
{
    [System.Serializable]
    public struct ItemConfig
    {
        public GameObject[] prefabs; // Allow multiple prefabs per item type
        public int count;
    }

    [SerializeField]
    private MazeGenerator mazeGenerator;//arroy of possible prefabs to spawn for this item type
    [SerializeField]
    private ItemConfig[] itemsToGenerate;//number of items to spawn

    // Initializes the generator with a reference to an existing MazeGenerator.cs
    public void Initialize(MazeGenerator mazeGenerator)
    {
        this.mazeGenerator = mazeGenerator;
    }

    // Generates all items as per the configuration
    public void Generate()
    {
        foreach (var item in itemsToGenerate)
        {
            PlaceItemsRandomly(item.prefabs, item.count);
        }
    }

    // Places items randomly on the map using available positions provided by the MazeGenerator.cs
    private void PlaceItemsRandomly(GameObject[] itemPrefabs, int itemCount)
    {
        for (int i = 0; i < itemCount; i++)
        {
            Vector2? position = PositionManager.Instance.GetRandomPosition();
            if (position != null)
            {
                GameObject itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
                Instantiate(itemPrefab, position.Value, Quaternion.identity);
            }
        }
    }
}
