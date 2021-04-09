using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [Range(1, 10)]
    public float horizontalRotationSpeed = 1;
    [Range(1, 10)]
    public float verticalRotationSpeed = 1;

    private float xRotation = 0f;
    private float yRotation = 0f;
    
    void Update()
    {
        // Rotate Camera on Mouse Move if Control pressed
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            RotateCamera();
    }

    private void RotateCamera()
    {
        yRotation += verticalRotationSpeed * Input.GetAxis("Mouse X");
        xRotation -= horizontalRotationSpeed * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(xRotation, yRotation, 0);
    }
}
