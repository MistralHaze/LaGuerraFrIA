using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateReceptor : MonoBehaviour {

    public GameObject receptor;
    public float rotatePower;
    Vector3 rotateOffset;

    // Use this for initialization
    void Start () {
        rotateOffset = new Vector3(0, rotatePower, 0);
	}
	
	// Update is called once per frame
	void Update () {
        receptor.transform.Rotate(rotateOffset);
	}
}
