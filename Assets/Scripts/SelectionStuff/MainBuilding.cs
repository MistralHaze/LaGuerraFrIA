using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBuilding : MonoBehaviour {
        
    public HexGrid hexGrid;

    [Header("Units spawn time")]
    public float BasicUnitCreationTime;
    public float IntermediateUnitCreationTime;
    public float AdvancedUnitCreationTime;
    [Header("Units cost")]

    public int BasicUnitMaterialCost=50;
    public int BasicUnitEnergyCost=50;

    public int IntermediateUnitMaterialCost=30;
    public int IntermediateUnitEnergyCost=70;


    public int AdvancedUnitMaterialCost=70;
    public int AdvancedUnitEnergyCost=30;


    [Header("Prefabs reference")]
    public GameObject BasicUnit;
    public GameObject IntermediateUnit;
    public GameObject AdvancedUnit;

    Faction faction;
    //Para controlar que, mientras se está haciendo una ud, no podamos hacer otra
    bool creatingUnit = false;

    private void Start()
    {
        faction = GetComponent<Faction>();
    }

    //Infantry
    public void CreateBasicUnit ()
    {
        if (!creatingUnit && faction.GetMaterial() >= BasicUnitMaterialCost && faction.GetEnergy() >= BasicUnitEnergyCost && faction.spawnCell.unitInside == null)
        {
            creatingUnit = true;
            faction.RemoveMaterial(BasicUnitMaterialCost);
            faction.RemoveEnergy(BasicUnitEnergyCost);
            StartCoroutine(basicUnitSpawner());
        }
    }

    //AntiTank
    public void CreateIntermediateUnit ()
    {
        if (!creatingUnit && faction.GetMaterial() >= IntermediateUnitMaterialCost && faction.GetEnergy() >= IntermediateUnitEnergyCost && faction.spawnCell.unitInside == null)
        {
            creatingUnit = true;
            faction.RemoveMaterial(IntermediateUnitMaterialCost);
            faction.RemoveEnergy(IntermediateUnitEnergyCost);
            StartCoroutine(intermediateUnitSpawner());
        }
    }

    //Tank
    public void CreateAdvancedUnit ()
    {
        if (!creatingUnit && faction.GetMaterial() >= AdvancedUnitMaterialCost && faction.GetEnergy() >= AdvancedUnitEnergyCost && faction.spawnCell.unitInside == null)
        {
            creatingUnit = true;
            faction.RemoveMaterial(AdvancedUnitMaterialCost);
            faction.RemoveEnergy(AdvancedUnitEnergyCost);
            StartCoroutine(advancedUnitSpawner());
        }
    }

    IEnumerator basicUnitSpawner()
    {        
        yield return new WaitForSeconds(BasicUnitCreationTime);
        //SpawnUnit
        GameObject aux = Instantiate(BasicUnit, faction.spawnCell.transform.position, BasicUnit.transform.rotation);
        faction.AddUnit(aux.GetComponent<Unit>());
        aux.GetComponent<Unit>().faction = faction;
        aux.GetComponent<Unit>().hexGrid = hexGrid;
        creatingUnit = false;
    }

    IEnumerator intermediateUnitSpawner()
    {
        yield return new WaitForSeconds(IntermediateUnitCreationTime);
        //SpawnUnit
        GameObject aux = Instantiate(IntermediateUnit, faction.spawnCell.transform.position, BasicUnit.transform.rotation);
        faction.AddUnit(aux.GetComponent<Unit>());
        aux.GetComponent<Unit>().faction = faction;
        aux.GetComponent<Unit>().hexGrid = hexGrid;
        creatingUnit = false;
    }

    IEnumerator advancedUnitSpawner()
    {
        yield return new WaitForSeconds(AdvancedUnitCreationTime);
        //SpawnUnit
        GameObject aux = Instantiate(AdvancedUnit, faction.spawnCell.transform.position, BasicUnit.transform.rotation);
        faction.AddUnit(aux.GetComponent<Unit>());
        aux.GetComponent<Unit>().faction = faction;
        aux.GetComponent<Unit>().hexGrid = hexGrid;
        creatingUnit = false;
    }
}
