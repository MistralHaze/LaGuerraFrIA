using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour {

    public GameObject doorUp;
    public GameObject doorDown;
    public GameObject reference;
    public GameObject reference2;

    // 1 is NORMAL kind
    // 2 is HEAVY kind
    // 3 is POWER kind
    public GameObject ship1;
    public GameObject ship2;
    public GameObject ship3;


    GameObject auxShip;

    public float initialVelocitySpawn;

    float timerOpen;
    float timerClose;
    bool openDoor;
    bool closeDoor;
    public float DoorOffset;
    Vector3 offset;



	// Use this for initialization
	void Start () {

        //spawnShip(1);

	}
	
	// Update is called once per frame
	void Update () {

        if (openDoor)
        {
            timerOpen += Time.deltaTime;
            doorUp.transform.position = doorUp.transform.position + offset;
            doorDown.transform.position = doorDown.transform.position - offset;
            if(timerOpen >= 2)
            {
                openDoor = false;
                timerOpen = 0f;

                //Aquí luego, en vez de cambiar la velocidad supongo que habrá que darle el pathfinding o hacer que se pare al llegar al centro de la próxima casilla o algo.
                //auxShip.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -1f);
                auxShip.GetComponent<Rigidbody>().velocity = reference.transform.up.normalized * initialVelocitySpawn;
                Invoke("closeDoorF", 2.5f);
            }
        }
        else if (closeDoor)
        {
            timerClose += Time.deltaTime;
            doorUp.transform.position = doorUp.transform.position - offset;
            doorDown.transform.position = doorDown.transform.position + offset;
            if(timerClose >= 2)
            {
                closeDoor = false;
                timerClose = 0f;
            }
        }
	}

    void closeDoorF()
    {
        closeDoor = true;
    }

    //TYPE
    // 1 is NORMAL kind
    // 2 is HEAVY kind
    // 3 is POWER kind

    //FACTION
    // 1 is USA
    // 2 is URSS
    // 3 is Alien

    public void spawnShip(int type)
    {
        
        switch (type)
        {
            case 1:
                auxShip = Instantiate(ship1, reference.transform.position, new Quaternion());
                break;
            case 2:
                auxShip = Instantiate(ship2, reference.transform.position, new Quaternion());
                break;
            case 3:
                auxShip = Instantiate(ship3, reference.transform.position, new Quaternion());
                break;

        }
        if(type != 1)auxShip.transform.rotation = reference.transform.rotation;
        else auxShip.transform.rotation = reference2.transform.rotation;


        timerOpen = 0;
        openDoor = true;
        closeDoor = false;
        offset = new Vector3(0f, DoorOffset / 1000, 0f);

    }
}
