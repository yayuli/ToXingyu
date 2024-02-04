using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{

    public GameObject wallL;
    public GameObject wallR;
    public GameObject wallU;
    public GameObject wallD;

    public void DestroyWall(GameObject wall)
    {
        // 可以在这里添加动画或特效
        Destroy(wall);
    }

}