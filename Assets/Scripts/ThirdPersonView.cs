using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonView : MonoBehaviour
{
    private const float YMin = -50.0f;
    private const float YMax = 50.0f;

    private Transform lookAt;

    public float distance;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    public float sensitivity = 4.0f;

    private void Start()
    {
        lookAt = GameObject.FindWithTag("CameraPos").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime; // Invert Y-axis movement

        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Vector3 Direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = lookAt.position + rotation * Direction;
        //transform.position += new Vector3(0, 0, 0);


        transform.LookAt(lookAt.position);
    }
}
