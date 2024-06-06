using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCamera : MonoBehaviour
{
    public Camera minimapCamera;
    [SerializeField]
    private float sizeIncrement = 10f;
    // Start is called before the first frame update
    void Start()
    {
        if (minimapCamera == null)
            minimapCamera = GetComponent<Camera>();
    }

    public void IncreaseCameraSize()
    {
        if (minimapCamera != null)
        {
            minimapCamera.orthographicSize += sizeIncrement;
        }
     
    }
}
