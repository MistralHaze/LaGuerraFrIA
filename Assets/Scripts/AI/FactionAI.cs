using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionAI : MonoBehaviour {

    public MainBuilding USABase, CCCPBase, AliensBase;

    public USA usaFaction;
    public CCCP cccpFaction;
    public Aliens aliensFaction;

    public Factions myFactionType;
    public Faction myFaction;
    public MainBuilding myBuilding;


    #region Number Of Cities, Units and Resources of my Faction
    //USA
    [HideInInspector]
    public int usaNumberOfMaterialCities, usaNumberOfEnergyCities = 0;

    [HideInInspector]
    public int usaNumberOfTankUnits, usaNumberOfAntyTankUnits, usaNumberOfInfantryUnits = 0;

    //CCCP
    [HideInInspector]
    public int cccpNumberOfMaterialCities, cccpNumberOfEnergyCities = 0;
    [HideInInspector]
    public int cccpNumberOfTankUnits, cccpNumberOfAntyTankUnits, cccpNumberOfInfantryUnits = 0;


    //Aliens
    [HideInInspector]
    public int aliensNumberOfMaterialCities, aliensNumberOfEnergyCities = 0;

    [HideInInspector]
    public int aliensNumberOfTankUnits, aliensNumberOfAntyTankUnits, aliensNumberOfInfantryUnits = 0;

    [HideInInspector]
    public int currentMaterials, currentEnergy = 0;
    #endregion

    [HideInInspector]
    public Factions invadedMoon = Factions.NONE;

    //Arbol de decisiones
    DecisionTree decisionTree;

    //Array de decisiones
    public List<Goal> factionPriorities = new List<Goal>();

    //Siguiente Paso:
    //Analisis del terreno
    public TerrainAnalysis Analysis;

    //Allocation
    public Allocation unitGoals;

    //Estudiar la situacion de las OTRAS facciones (¿por separado?)

    //Estudiar la situacion de mi facción

    public /*IEnumerator*/ void StartTurn()
    {
        /*JM:
         * Esta función necesita ser una corutina para que el juego no pete
         * Cada una de sus subfunciones necesita ser ejecutada en MENOS DE 
         * 1 frame (16ms)
         * Después de cada tarea pesada se necesita un yield return null
         * 
         * Fuera, desde donde se llama a esta función, habrá que controlar que
         * haya acabado de alguna manera, por ejemplo con un callback
         */

        //Concretar la faccion de este FactionAI
        //Almacenado en my Faction por editor

        factionPriorities = new List<Goal>();

        studyMap();
        //yield return null //Esperamos al siguiente frame
        List<CellGoal> cellGoals = Analysis.StartStudy(factionPriorities, myFactionType);
        //yield return null //Esperamos al siguiente frame

        cellGoals.Sort(); //Ordenar cellGoals por sus pesos
        //yield return null //Esperamos al siguiente frame

        //Si no funciona el sort usar esta linea
        //List<Order> SortedList = objListOrder.OrderBy(o => o.OrderDate).ToList();

        unitGoals.StartUnitGoals(cellGoals, myFaction.units, GetEnemyUnits());
        //yield return null //Esperamos al siguiente frame
    }

    private Unit[] GetEnemyUnits()
    {
        List<Unit> enemyUnitsList = new List<Unit>();

        switch (myFactionType)
        {
            case Factions.USA:
                Helpers.ListHelper.AddList(enemyUnitsList, cccpFaction.units);
                Helpers.ListHelper.AddList(enemyUnitsList, aliensFaction.units);
                break;
            case Factions.CCCP:
                Helpers.ListHelper.AddList(enemyUnitsList, usaFaction.units);
                Helpers.ListHelper.AddList(enemyUnitsList, aliensFaction.units);
                break;
            case Factions.ALIENS:
                Helpers.ListHelper.AddList(enemyUnitsList, usaFaction.units);
                Helpers.ListHelper.AddList(enemyUnitsList, cccpFaction.units);
                break;
            case Factions.NONE:
                Helpers.ListHelper.AddList(enemyUnitsList, usaFaction.units);
                Helpers.ListHelper.AddList(enemyUnitsList, cccpFaction.units);
                Helpers.ListHelper.AddList(enemyUnitsList, aliensFaction.units);
                break;
        }

        return enemyUnitsList.ToArray();
    }

    void studyMap()
    {
        RestartFactionInfo();

        //cojo las ciudades que tiene cada faccion
        CountUnits();

        //cojo las unidades que tiene cada faccion
        CountCities();

        //cojo los recursos disponibles de MI faccion
        switch (myFactionType)
        {
            case Factions.CCCP:
                currentEnergy = cccpFaction.energy;
                currentMaterials = cccpFaction.material;
                break;

            case Factions.USA:
                currentEnergy = usaFaction.energy;
                currentMaterials = usaFaction.material;
                break;

            case Factions.ALIENS:
                currentEnergy = aliensFaction.energy;
                currentMaterials = aliensFaction.material;
                break;

        }

        calculateGoals();

        calculateAndSpawnNeededShip();
    }


    //P:ej si voy a atacar/me está atacando la URSS, estudiaré que tipo de nave es la más común y creare su counter (en función de las naves que tengo yo)
    void calculateAndSpawnNeededShip()
    {
        bool attack = false;
        Goal.TypeGoal current_attack_obj = Goal.TypeGoal.conquestMoon;
        int maxPeso = 11;

        //En base a las naves enemigas, mis naves y mis recursos

        foreach (Goal objective in factionPriorities)
        {
            if (objective.objective == Goal.TypeGoal.attackAliens || objective.objective == Goal.TypeGoal.attackCCP || objective.objective == Goal.TypeGoal.attackUSA)
            {
                attack = true;
                if (objective.weight < maxPeso)
                {
                    maxPeso = objective.weight;
                    current_attack_obj = objective.objective;
                }
            }
        }
        Unit.unitType unitToCreate = Unit.unitType.infantry;
        //Si no atacamos a nadie
        if (attack == false)
        {
            switch (myFactionType)
            {
                case Factions.ALIENS:
                    unitToCreate = findMinimumTypeOfUnit(aliensNumberOfInfantryUnits, aliensNumberOfAntyTankUnits,aliensNumberOfTankUnits);
                    break;
                case Factions.USA:
                    unitToCreate = findMinimumTypeOfUnit(usaNumberOfInfantryUnits, usaNumberOfAntyTankUnits, usaNumberOfTankUnits);
                    break;
                case Factions.CCCP:
                    unitToCreate = findMinimumTypeOfUnit(cccpNumberOfInfantryUnits, cccpNumberOfAntyTankUnits, cccpNumberOfTankUnits);
                    break;
            }

            switch (unitToCreate)
            {
                case Unit.unitType.infantry:
                    myBuilding.CreateBasicUnit();
                    break;

                case Unit.unitType.antiTank:
                    if (myFaction.GetEnergy() > myBuilding.IntermediateUnitEnergyCost && myFaction.GetMaterial() > myBuilding.IntermediateUnitMaterialCost)
                        myBuilding.CreateIntermediateUnit();
                    else
                        myBuilding.CreateBasicUnit();
                    break;

                case Unit.unitType.Tank:
                    if (myFaction.GetEnergy() > myBuilding.AdvancedUnitEnergyCost && myFaction.GetMaterial() > myBuilding.AdvancedUnitMaterialCost)
                        myBuilding.CreateAdvancedUnit();
                    else
                        myBuilding.CreateBasicUnit();
                    break;
            }
        }
        else
        {
            switch (current_attack_obj)
            {
                case (Goal.TypeGoal.attackAliens):
                    unitToCreate = findMaximumTypeOfUnit(aliensNumberOfInfantryUnits, aliensNumberOfAntyTankUnits, aliensNumberOfTankUnits);
                    break;

                case (Goal.TypeGoal.attackUSA):
                    unitToCreate = findMaximumTypeOfUnit(usaNumberOfInfantryUnits, usaNumberOfAntyTankUnits, usaNumberOfTankUnits);
                    break;

                case (Goal.TypeGoal.attackCCP):
                    unitToCreate = findMaximumTypeOfUnit(cccpNumberOfInfantryUnits, cccpNumberOfAntyTankUnits, cccpNumberOfTankUnits);
                    break;
            }

            //Este switch va por separado ya que si la IA necesita una clase de nave especial va a ahorrar recursos para poderla crear en el siguiente turno
            switch (unitToCreate)
            {
                case Unit.unitType.infantry:
                    myBuilding.CreateBasicUnit();
                    break;

                case Unit.unitType.antiTank:
                    if (myFaction.GetEnergy() > myBuilding.IntermediateUnitEnergyCost && myFaction.GetMaterial() > myBuilding.IntermediateUnitMaterialCost)
                        myBuilding.CreateIntermediateUnit();
                    break;

                case Unit.unitType.Tank:
                    if (myFaction.GetEnergy() > myBuilding.AdvancedUnitEnergyCost && myFaction.GetMaterial() > myBuilding.AdvancedUnitMaterialCost)
                        myBuilding.CreateAdvancedUnit();
                    break;
            }
        }

        //El resultado de esta funcion son las llamadas a myBuilding.Create"TypeOfUnit"()

    }

    List<Goal> calculateGoals()
    {
        //Crear un array con mis objetivos con peso asignado a ellos


        //Objetivo del arbol de decisiones: Estudiar la situacion de la faccion (me estan atacando, me interesa conquistar ciudades, me interesa atacar a USA) y los números cálculados (num tanques enemigos USA) en este script para conseguir el array de decisiones

        //Esto de lo hará el decision tree --> estudio del estado del mapa y me devolverá un array que cumpla estas condiciones
        //factionPriorities = decisionTree.MakeDecision();
        //que haga lo de arriba o que las hojas del arbol llamen a una funcion setGoals.

        //Otra posibilidad
        //return decisionTree.MakeDecision();

        //TODO: Esto CACA
        return new List<Goal>();

    }


    void CountCities()
    {
        List<CityCell> allCities = usaFaction.GetCities();

        for (int i = 0; i < allCities.Count; i++)
        {
            if (allCities[i].resourceType == CityCell.ResourceType.ENERGY)
                usaNumberOfEnergyCities++;
            if (allCities[i].resourceType == CityCell.ResourceType.MATERIAL)
                usaNumberOfMaterialCities++;
            if (allCities[i].resourceType == CityCell.ResourceType.MOON)
                invadedMoon = Factions.USA;

        }


        allCities = aliensFaction.GetCities();

        for (int i = 0; i < allCities.Count; i++)
        {
            if (allCities[i].resourceType == CityCell.ResourceType.ENERGY)
                aliensNumberOfEnergyCities++;
            if (allCities[i].resourceType == CityCell.ResourceType.MATERIAL)
                aliensNumberOfMaterialCities++;
            if (allCities[i].resourceType == CityCell.ResourceType.MOON)
                invadedMoon = Factions.ALIENS;

        }

        allCities = cccpFaction.GetCities();

        for (int i = 0; i < allCities.Count; i++)
        {
            if (allCities[i].resourceType == CityCell.ResourceType.ENERGY)
                cccpNumberOfEnergyCities++;
            if (allCities[i].resourceType == CityCell.ResourceType.MATERIAL)
                cccpNumberOfMaterialCities++;
            if (allCities[i].resourceType == CityCell.ResourceType.MOON)
                invadedMoon = Factions.CCCP;
        }
    }

    void CountUnits()
    {
        List<Unit> allUnits = usaFaction.units;

        for (int i = 0; i < allUnits.Count; i++)
        {
            if (allUnits[i].type == Unit.unitType.infantry)
                usaNumberOfInfantryUnits++;
            if (allUnits[i].type == Unit.unitType.Tank)
                usaNumberOfTankUnits++;
            if (allUnits[i].type == Unit.unitType.antiTank)
                usaNumberOfAntyTankUnits++;

        }

        allUnits = aliensFaction.units;

        for (int i = 0; i < allUnits.Count; i++)
        {
            if (allUnits[i].type == Unit.unitType.infantry)
                aliensNumberOfInfantryUnits++;
            if (allUnits[i].type == Unit.unitType.Tank)
                aliensNumberOfTankUnits++;
            if (allUnits[i].type == Unit.unitType.antiTank)
                aliensNumberOfAntyTankUnits++;

        }

        allUnits = cccpFaction.units;

        for (int i = 0; i < allUnits.Count; i++)
        {
            if (allUnits[i].type == Unit.unitType.infantry)
                cccpNumberOfInfantryUnits++;
            if (allUnits[i].type == Unit.unitType.Tank)
                cccpNumberOfTankUnits++;
            if (allUnits[i].type == Unit.unitType.antiTank)
                cccpNumberOfAntyTankUnits++;

        }
    }

    Unit.unitType findMinimumTypeOfUnit(int infantry, int antiTank, int Tank)
    {
        Unit.unitType unitToCreate = Unit.unitType.infantry;
        if (infantry < antiTank && infantry < Tank)
            unitToCreate = Unit.unitType.infantry;
        else if (antiTank < infantry && antiTank < Tank)
            unitToCreate = Unit.unitType.antiTank;
        else if (Tank < infantry && Tank < antiTank)
            unitToCreate = Unit.unitType.Tank;
        return unitToCreate;
    }


    Unit.unitType findMaximumTypeOfUnit(int infantry, int antiTank, int Tank)
    {
        Unit.unitType unitToCreate = Unit.unitType.infantry;
        if (infantry > antiTank && infantry > Tank)
            unitToCreate = Unit.unitType.infantry;
        else if (antiTank > infantry && antiTank > Tank)
            unitToCreate = Unit.unitType.antiTank;
        else if (Tank > infantry && Tank > antiTank)
            unitToCreate = Unit.unitType.Tank;
        return unitToCreate;
    }

    void RestartFactionInfo()
    {

        //USA
         usaNumberOfMaterialCities = 0;
         usaNumberOfEnergyCities = 0;

         usaNumberOfTankUnits = 0;
         usaNumberOfAntyTankUnits = 0;
         usaNumberOfInfantryUnits = 0;

        //CCCP
         cccpNumberOfMaterialCities = 0;
         cccpNumberOfEnergyCities = 0;

         cccpNumberOfTankUnits = 0;
         cccpNumberOfAntyTankUnits = 0;
         cccpNumberOfInfantryUnits = 0;


        //Aliens
         aliensNumberOfMaterialCities = 0;
         aliensNumberOfEnergyCities = 0;

         aliensNumberOfTankUnits = 0;
         aliensNumberOfAntyTankUnits = 0;
         aliensNumberOfInfantryUnits = 0;
    }

}

namespace Helpers
{
    public static class ListHelper
    {
        public static void AddList<T> (List<T> destiny, List<T> source)
        {
            foreach (T element in source)
                destiny.Add(element);
        }
    }
}
