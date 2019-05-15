using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    public static InputController instance;

    public Vector2 mouseCamMovPadding = new Vector2(0.2f, 0.2f);
    public float cameraRotate, verticalCamMovement, horizontalCamMovement,
        mousePanAccel = 0.2f;

    Vector2 RealPadding;
    Camera cam;

    // Use this for initialization
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);

        instance = this;

        cam = Camera.main;
    }

    private void Start()
    {
        mouseCamMovPadding = new Vector2(mouseCamMovPadding.x * cam.pixelWidth,
    mouseCamMovPadding.y * cam.pixelHeight);
    }

    // Update is called once per frame
    void Update()
    {
        cameraRotate = Input.GetAxis("Rotation");
        verticalCamMovement = Input.GetAxis("Vertical");
        horizontalCamMovement = Input.GetAxis("Horizontal");

        Vector2 mp = Input.mousePosition;// Debug.Log(mp);

        if(mp.x > 0 && mp.y > 0 && mp.x < cam.pixelWidth && mp.y < cam.pixelHeight)
        {
            if (mp.x < mouseCamMovPadding.x || mp.x > cam.pixelWidth - mouseCamMovPadding.x)
            {
                horizontalCamMovement = mp.x < 0.5f * cam.pixelWidth ? -1f : 1f;

                //horizontalCamMovement = Mathf.Lerp(horizontalCamMovement, objectiveValue, mousePanAccel);
            }

            if (mp.y < mouseCamMovPadding.y || mp.y > cam.pixelHeight - mouseCamMovPadding.y)
            {
                verticalCamMovement = mp.y < 0.5f * cam.pixelHeight ? -1f : 1f;

                //verticalCamMovement = Mathf.Lerp(verticalCamMovement, objectiveValue, mousePanAccel);
            }
        }

        

    }

}
