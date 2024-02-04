using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private Transform playerTransform; // 玩家的Transform组件
    private Vector3 cameraOffset; // 相机相对于玩家的偏移量

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.5f; // 平滑移动的因子

    public bool lookAtPlayer = false; // 是否始终让相机朝向玩家

    void Start()
    {
        // 动态查找玩家对象
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            // 计算并存储相机初始时相对于玩家的偏移量
            cameraOffset = transform.position - playerTransform.position;
        }
        else
        {
            Debug.LogError("CameraFollowPlayer: Player object not found.");
        }
    }

    void LateUpdate()
    {
        // 如果没有找到玩家，不执行任何操作
        if (playerTransform == null)
            return;

        // 计算相机的新位置
        Vector3 newPos = playerTransform.position + cameraOffset;

        // 使用线性插值平滑地移动相机到新位置
        transform.position = Vector3.Lerp(transform.position, newPos, smoothFactor);

        // 如果设置为始终朝向玩家，更新相机的朝向
        if (lookAtPlayer)
        {
            transform.LookAt(playerTransform);
        }
    }
}

