using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mine : MonoBehaviour {

    public GameObject stick1;
    public GameObject stick2;
    public GameObject platform;

    public GameObject reference1;
    public GameObject reference2;

    public GameObject reference4;
    public GameObject reference5;

    public GameObject reference6;
    public GameObject reference7;

    public GameObject cube;
    public GameObject cube2;
    public GameObject cube3;
    public GameObject cube4;
    public GameObject cube5;
    public GameObject cube6;
    GameObject cubeMain;

    int cubeCount;

    public float PickUpTime;

    float maxScale;
    float initialScale;
   // bool changeDirection;

    enum CraneState
    {
       PICKUP, UP, LEFT, RELEASE, ROTATEBACK, RIGHT, DOWN
    }

    CraneState craneState;

    float pickupTimer;

    float maxAngle;
    float angle;


	// Use this for initialization
	void Start () {
        maxScale = Mathf.Abs(stick1.transform.localScale.y/4.5f);
        initialScale = stick1.transform.localScale.y;
        //changeDirection = false;
        craneState = CraneState.PICKUP;
        pickupTimer = 0f;
        maxAngle = 45f;
        angle = 0f;

        cubeCount = 1;

        cubeMain = cube;

        //Llamar a instanciar cubo.
	}

    // Update is called once per frame
    void Update() {

        switch (craneState)
        {
            case CraneState.PICKUP:
                pickupTimer += Time.deltaTime;
                if (pickupTimer >= PickUpTime)
                {
                    craneState = CraneState.UP;
                    pickupTimer = 0f;

                }
                break;

            case CraneState.UP:

                if (Mathf.Abs(stick1.transform.localScale.y) > maxScale)
                {
                    float newYScale = Mathf.Abs(stick1.transform.localScale.y - maxScale * 4.5f * 0.005f);
                    stick1.transform.localScale = new Vector3(stick1.transform.localScale.x, newYScale, stick1.transform.localScale.z);
                    stick2.transform.localScale = new Vector3(stick1.transform.localScale.x, newYScale, stick1.transform.localScale.z);
                    stick1.transform.position = reference1.transform.position - new Vector3(0, stick1.GetComponent<Renderer>().bounds.size.y, 0) / 2;
                    stick2.transform.position = reference2.transform.position - new Vector3(0, stick2.GetComponent<Renderer>().bounds.size.y, 0) / 2;
                    platform.transform.position = reference1.transform.position - new Vector3(0, stick1.GetComponent<Renderer>().bounds.size.y, -platform.GetComponent<Renderer>().bounds.size.z / 2);
                   // cube.transform.position = reference1.transform.position - new Vector3(0, stick1.GetComponent<Renderer>().bounds.size.y, -platform.GetComponent<Renderer>().bounds.size.z / 2);

                }

                else
                {
                    stick1.GetComponent<Rigidbody>().velocity = new Vector3(-maxScale * 3f, 0, 0);
                    stick2.GetComponent<Rigidbody>().velocity = new Vector3(-maxScale * 3f, 0, 0);
                    platform.GetComponent<Rigidbody>().velocity = new Vector3(-maxScale * 3f, 0, 0);
                    cubeMain.GetComponent<Rigidbody>().velocity = new Vector3(-maxScale * 3f, 0, 0);
                    craneState = CraneState.LEFT;
                    /*stick1.transform.position += new Vector3(-0.001f, 0, 0);
                    stick2.transform.position += new Vector3(-0.001f, 0, 0);*/
                }
                break;

            case CraneState.LEFT:
                
                if (stick1.transform.position.x < reference4.transform.position.x)
                {
                    stick1.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    stick2.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    platform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    platform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                    cubeMain.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    craneState = CraneState.RELEASE;
                }
                break;

            case CraneState.RELEASE:
                //girar plataforma
                angle += 0.5f;
                platform.transform.Rotate(0.5f, 0, 0);
                if(angle >= maxAngle)
                {
                    if (cubeCount == 1)
                    {
                        cubeCount++;
                        cubeMain = cube2;
                        cubeMain.transform.position = reference6.transform.position;

                    }
                    else if (cubeCount == 2)
                    {
                        cubeCount++;
                        cubeMain = cube3;
                        cubeMain.transform.position = reference6.transform.position;
                    }
                    else if (cubeCount == 3)
                    {
                        cubeCount++;
                        cubeMain = cube4;
                        cubeMain.transform.position = reference6.transform.position;
                    }
                    else if (cubeCount == 4)
                    {
                        cubeCount++;
                        cubeMain = cube5;
                        cubeMain.transform.position = reference6.transform.position;
                    }
                    else if (cubeCount == 5)
                    {
                        cubeCount++;
                        cubeMain = cube6;
                        cubeMain.transform.position = reference6.transform.position;
                    }
                    else if (cubeCount == 6)
                    {
                        cubeCount = 1;
                        cubeMain = cube;
                        cubeMain.transform.position = reference6.transform.position;
                    }


                    angle = 0f;
                    craneState = CraneState.ROTATEBACK;
                }
                break;
            case CraneState.ROTATEBACK:
                angle += 0.5f;
                platform.transform.Rotate(-0.5f, 0, 0);
                if (angle >= maxAngle)
                {
                    angle = 0f;
                    stick1.GetComponent<Rigidbody>().velocity = new Vector3(maxScale * 3f, 0, 0);
                    stick2.GetComponent<Rigidbody>().velocity = new Vector3(maxScale * 3f, 0, 0);
                    platform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                    platform.GetComponent<Rigidbody>().velocity = new Vector3(maxScale * 3f, 0, 0);
                    //platform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    
                    craneState = CraneState.RIGHT;

                }
                break;
            case CraneState.RIGHT:
                if (stick1.transform.position.x > reference1.transform.position.x)
                {
                    stick1.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    stick2.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    platform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    craneState = CraneState.DOWN;
                }
                break;
            case CraneState.DOWN:
                if (Mathf.Abs(stick1.transform.localScale.y) < initialScale)
                {
                    float newYScale = Mathf.Abs(stick1.transform.localScale.y + maxScale * 4.5f * 0.005f);
                    stick1.transform.localScale = new Vector3(stick1.transform.localScale.x, newYScale, stick1.transform.localScale.z);
                    stick2.transform.localScale = new Vector3(stick1.transform.localScale.x, newYScale, stick1.transform.localScale.z);
                    stick1.transform.position = reference1.transform.position - new Vector3(0, stick1.GetComponent<Renderer>().bounds.size.y, 0) / 2;
                    stick2.transform.position = reference2.transform.position - new Vector3(0, stick2.GetComponent<Renderer>().bounds.size.y, 0) / 2;
                    platform.transform.position = reference1.transform.position - new Vector3(0, stick1.GetComponent<Renderer>().bounds.size.y, -platform.GetComponent<Renderer>().bounds.size.z / 2);

                }

                else {

                    cubeMain.transform.position = reference7.transform.position;
                    craneState = CraneState.PICKUP;

                }
                break;


        }
        
       
    }
}
