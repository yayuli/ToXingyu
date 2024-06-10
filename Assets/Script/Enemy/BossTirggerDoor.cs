using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTirggerDoor : MonoBehaviour
{
    public void OnDefeated()
    {
        MazeGenerator mazeGenerator = FindObjectOfType<MazeGenerator>();
        if (mazeGenerator != null)
        {
            mazeGenerator.MakeExit();
        }
        else
        {
            Debug.LogError("MazeGenerator not found!");
        }
    }
}
