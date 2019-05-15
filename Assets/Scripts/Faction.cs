using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Faction : MonoBehaviour
{
    #region Variables
    const int CITY_MATERIAL = 20;
    const int CITY_BASE_MATERIAL = 50;
    const int CITY_ENERGY = 20;
    const int CITY_BASE_ENERGY = 50;

    public GameManager gameManager;
    public SelectionComponent selectionComponent;
    public HexCell spawnCell;
    public bool playerFaction = false;
    //public int specialResource = 0;
    //public CityCell cityBase;
    public Factions myFaction;
    public FactionAI IA;

    [HideInInspector]
    public List<Unit> units;
    public List<CityCell> cities;
    public int material = 0;
    public int energy = 0;
    #endregion

    //HAY FUNCIONES QUE TIENEN EN CUENTA QUE LA FACCIÓN ES CONTROLADA POR EL HUMANO, TENER EN CUENTA ESO PARA LA IA

    void Awake()
    {
        units = new List<Unit>();
        cities = new List<CityCell>();
    }

    // Use this for initialization
    void Start()
    {

    }

    void UpdateMaterials()
    {
        checkCities();
        for (int i = 0; i < cities.Count; i++)
        {
            if (cities[i].resourceType == CityCell.ResourceType.ENERGY)
            {
                energy += CITY_ENERGY;
            }
            else if (cities[i].resourceType == CityCell.ResourceType.MATERIAL)
            {
                material += CITY_MATERIAL;
            }
            else if (cities[i].resourceType == CityCell.ResourceType.MOON)
            {
                energy += CITY_ENERGY;
                material += CITY_MATERIAL;
            }
        }
        material += CITY_BASE_MATERIAL;
        energy += CITY_BASE_ENERGY;
    }

    //CON ESTOS GETTERS Y SETTERS DEPENDE COMO LO QUERAMOS HACER, LO DEJO PARA RELLENAR
    #region GET $ SET

    public int GetMaterial()
    {
        return material;
    }    

    public void RemoveMaterial(int material)
    {
        this.material -= material;
    }

    public int GetEnergy()
    {
        return energy;
    }

    public void AddEnergy(int energy)
    {
        this.energy += energy;
    }

    public void RemoveEnergy(int energy)
    {
        this.energy -= energy;
    }
    
    public int GetNumberUnits()
    {
        return units.Count;
    }

    public void AddUnit(Unit unit)
    {
        units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public List<Unit> GetListUnits()
    {
        return units;
    }

    public List<CityCell> GetCities()
    {
        checkCities();
        return cities;
    }

    public void AddCity (CityCell city)
    {
        cities.Add(city);
    }

    public bool IsPlayerFaction()
    {
        return playerFaction;
    }

    public void SetPlayerFaction(bool playerFaction)
    {
        this.playerFaction = playerFaction;
    }
    #endregion

    private void checkCities()
    {
        for (int i = 0; i < cities.Count; i++)
        {
            if (cities[i].faction != myFaction)
            {
                print("Eliminando ciudad");
                cities.RemoveAt(i);
                i--;
            }            
        }
    }

    public void PlayHuman()
    {
        print("Turno del jugador");
        //Principo de turno
        UpdateMaterials();
        selectionComponent.enabled = true;
    }

    public void PlayIA()
    {
        //DO IA´S SHIT
        print("Turno de IA: " + this.gameObject.name);

        //TODO
        //IA.StartTurn();

        gameManager.NextTurn();
    }

    public abstract void FactionPower();

    public void EndTurn()
    {
        if (playerFaction)
        {
            selectionComponent.enabled = false;
        }
        gameManager.NextTurn();        
    }
}
