using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeFaction : MonoBehaviour {

    //Este script sirve tanto para moon como para las minas como para los satélites.

    public GameObject Neutral;
    public GameObject USA;
    public GameObject URSS;
    public GameObject Alien;

    public GameObject Crane;
    public Material CraneMaterial1;
    public Material CraneMaterial2;
    public Material CraneMaterial3;

    public enum Faction
    {
        Neutral, Alien, USA, URSS
    }

    //[HideInInspector]
    public Faction faction; //De momento no lo uso, pero puede que lo queramos usar en un futuro.

    // Use this for initialization
    void Start () {

        if (faction == Faction.Neutral) setNeutral();
        if (faction == Faction.USA) setUSA();
        if (faction == Faction.URSS) setURSS();
        if (faction == Faction.Alien) setAlien();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void setUSA()
    {
        faction = Faction.USA;
        Neutral.SetActive(false);
        USA.SetActive(true);
        URSS.SetActive(false);
        Alien.SetActive(false);
        foreach(Transform children in Crane.transform)
        {
            children.gameObject.GetComponent<Renderer>().material = CraneMaterial2;
        }
    }
    public void setURSS()
    {
        faction = Faction.URSS;
        Neutral.SetActive(false);
        USA.SetActive(false);
        URSS.SetActive(true);
        Alien.SetActive(false);
        foreach (Transform children in Crane.transform)
        {
            children.gameObject.GetComponent<Renderer>().material = CraneMaterial1;
        }
    }
    public void setAlien()
    {
        faction = Faction.Alien;
        Neutral.SetActive(false);
        USA.SetActive(false);
        URSS.SetActive(false);
        Alien.SetActive(true);
        foreach (Transform children in Crane.transform)
        {
            children.gameObject.GetComponent<Renderer>().material = CraneMaterial3;
        }
    }
    public void setNeutral()
    {
        faction = Faction.Neutral;
        Neutral.SetActive(true);
        USA.SetActive(false);
        URSS.SetActive(false);
        Alien.SetActive(false);
        foreach (Transform children in Crane.transform)
        {
            children.gameObject.GetComponent<Renderer>().material = CraneMaterial1;
        }
    }
}
