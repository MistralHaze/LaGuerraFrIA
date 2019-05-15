using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public bool show;
    public GameObject target;
    public float dmg;
    public shooting shtng;
    public bool amITheLastBullet = false;

    private float distanceMultiplier = 1;
    public Vector3 targetPos;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        //print(Vector3.Distance(this.transform.position, target.transform.position));
        if(Vector3.Distance(this.transform.position, targetPos) < 4f * distanceMultiplier)
        {
            if (target != null)
            {
                target.GetComponent<Unit>().TakeDamage(dmg);
            }
            if (amITheLastBullet)
            {
                shtng.GetComponentInParent<Unit>().attacking = false;
            }            
            Destroy(this.gameObject);
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        //Destroy(this.gameObject);
    }

    public void setDistanceMultiplier()
    {
        distanceMultiplier = 2f;
    }
}
