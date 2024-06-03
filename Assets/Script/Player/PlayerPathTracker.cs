using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPathTracker : MonoBehaviour
{
    public Transform player;
    public GameObject pathMarkerPrefab;  // 预制体，用于标记路径
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = player.position;
        MarkPath();  // 在起始位置标记
    }

    void Update()
    {
        if (Vector3.Distance(player.position, lastPosition) > 1.0f) // 如果玩家移动超过1米
        {
            lastPosition = player.position;
            MarkPath();
        }
    }

    void MarkPath()
    {
        Instantiate(pathMarkerPrefab, player.position, Quaternion.identity);  // 在玩家位置生成路径标记
    }
}