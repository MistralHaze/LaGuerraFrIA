using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasResourcesUpdate : MonoBehaviour {

    public Text material;
    public Text energy;
    public Text special;

    Faction currentFaction;

	// Use this for initialization
	void Start () {
        currentFaction = GetComponent<Faction>();
	}
	
	// Update is called once per frame
	void Update () {
        material.text = "Materials: " + currentFaction.GetMaterial();
        energy.text = "Energy: " + currentFaction.GetEnergy();
	}
}
