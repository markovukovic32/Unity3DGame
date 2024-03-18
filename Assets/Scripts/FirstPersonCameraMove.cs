using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraMove : MonoBehaviour
{
    private Transform cameraPosition;
    void Start()
    {
        cameraPosition = GameObject.FindWithTag("CameraPos").transform;
    }

    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
