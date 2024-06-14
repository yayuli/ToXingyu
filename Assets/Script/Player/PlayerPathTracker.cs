using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPathTracker : MonoBehaviour
{
    public Transform player;
    public GameObject pathMarkerPrefab;  // 预制体，用于标记路径
    private Vector3 lastPosition;
    private int recyclingTime = 3;

    void Start()
    {
        lastPosition = player.position;
        MarkPath();  // 在起始位置标记
    }

    void Update()
    {
        if (Vector3.Distance(player.position, lastPosition) > 1.0f) 
        {
            lastPosition = player.position;
            MarkPath();
        }
    }

    void MarkPath()
    {
        GameObject marker = ObjectPool.Release(pathMarkerPrefab.gameObject, player.position, Quaternion.identity);
        StartCoroutine(ReturnMarkerToPool(marker, recyclingTime));
    }
    IEnumerator ReturnMarkerToPool(GameObject marker, float delay)
    {
        yield return new WaitForSeconds(delay);
        ObjectPool.Return(pathMarkerPrefab.gameObject, marker);
    }
}