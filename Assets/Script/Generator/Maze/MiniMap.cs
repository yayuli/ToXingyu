using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    // 负责迷你地图的显示和更新
    public Camera miniMapCamera;
    public float miniMapSize = 5f;
    public Transform player;
    public MazeGenerator mazeGenerator;  // 引用迷宫生成器以访问 allCells

    private HashSet<Vector2> visibleCells = new HashSet<Vector2>();

    void Start()
    {
        miniMapCamera.orthographicSize = miniMapSize;
    }

    void Update()
    {
        if (player != null)
        {
            miniMapCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, miniMapCamera.transform.position.z);
        }
        UpdateVisibleCells();
    }

    void UpdateVisibleCells()
    {
        visibleCells.Clear();
        // 计算当前视野范围内的单元格
        Vector2 min = new Vector2(miniMapCamera.transform.position.x - miniMapSize, miniMapCamera.transform.position.y - miniMapSize);
        Vector2 max = new Vector2(miniMapCamera.transform.position.x + miniMapSize, miniMapCamera.transform.position.y + miniMapSize);

        foreach (var cellPos in mazeGenerator.AllCells.Keys)
        {
            if (cellPos.x >= min.x && cellPos.x <= max.x && cellPos.y >= min.y && cellPos.y <= max.y)
            {
                mazeGenerator.AllCells[cellPos].cellObject.SetActive(true);
            }
            else
            {
                mazeGenerator.AllCells[cellPos].cellObject.SetActive(false);
            }
        }

    }
}