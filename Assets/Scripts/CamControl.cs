using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{

    public float movementSpeed = 3f, rotationSpeed = 100f;

    InputController input;
    Camera cam;
    public Transform rotationPoint;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }

    void Start()
    {
        input = InputController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (input.cameraRotate != 0)
            transform.RotateAround(rotationPoint.position, Vector3.up, -input.cameraRotate * rotationSpeed * Time.deltaTime);
        if (input.verticalCamMovement != 0 || input.horizontalCamMovement != 0)
        {
            transform.position += (Vector3)(transform.localToWorldMatrix * (input.verticalCamMovement * Vector3.forward + input.horizontalCamMovement * Vector3.right));
        }


    }
}
