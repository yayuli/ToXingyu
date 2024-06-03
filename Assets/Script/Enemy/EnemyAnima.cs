using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnima : MonoBehaviour
{
    public Transform sprite;
    public float speed;
    public float minSize, maxSize;
    private float activeSize;

    // Start is called before the first frame update
    void Start()
    {
        activeSize = maxSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
