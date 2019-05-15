using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainAnalysis : MonoBehaviour {

    public HexGrid mapa;

    public GameManager gameManager;

    //Tiene que haber un TerrainAnalysis para cada IA

    //Lista de prioridades de faccion
    List<Goal> priorities;
    //Que facción soy yo
    Factions myFaction;

    //Lista de CellGoals
    List<CellGoal> cellGoals;

    public USA usaFaction;
    public CCCP cccpFaction;
    public Aliens aliensFaction;

    public List<CellGoal> StartStudy(List<Goal> priorities, Factions myFaction)
    {
        cellGoals = new List<CellGoal>();

        this.priorities = priorities;
        this.myFaction = myFaction;

        sortGoalCells();

        return cellGoals;
    }

    void sortGoalCells()
    {
        //Calcular lista de objetivos (celdas) comparando celdas con la lista de prioridades.

        //Recorrer prioridades
        for(int i = 0; i < priorities.Count; i++)
        {
            switch (priorities[i].objective)
            {
                case Goal.TypeGoal.attackAliens:
                    //Devolver naves enemigas que pertenezcan a Aliens
                    for (int j =0; j< aliensFaction.units.Count; j++)
                    {
                        cellGoals.Add(new CellGoal(aliensFaction.units[i].GetUnitCell(), priorities[i].weight));
                    }
                    break;
                case Goal.TypeGoal.attackCCP:
                    //Devolver naves enemigas que pertenezcan a la URSS
                    cellGoals.Add(new CellGoal(cccpFaction.units[i].GetUnitCell(), priorities[i].weight));
                    break;
                case Goal.TypeGoal.attackUSA:
                    //Devolver naves enemigas que pertenezcan a USA
                    cellGoals.Add(new CellGoal(usaFaction.units[i].GetUnitCell(), priorities[i].weight));
                    break;

                case Goal.TypeGoal.defendBase:
                    //Devolver base de myFaction
                    switch (myFaction){
                        case Factions.ALIENS:
                            cellGoals.Add(new CellGoal(aliensFaction.spawnCell, priorities[i].weight));
                            break;
                        case Factions.USA:
                            cellGoals.Add(new CellGoal(usaFaction.spawnCell, priorities[i].weight));
                            break;
                        case Factions.CCCP:
                            cellGoals.Add(new CellGoal(cccpFaction.spawnCell, priorities[i].weight));
                            break;
                    }
                    break;

                case Goal.TypeGoal.defendCities:
                    //Coger lista de ciudades  del gameManager (gameManager.GetCities) que pertenecen a la facción y añadirlas con un peso "alto" a cellGoals
                    switch (myFaction)
                    {
                        case Factions.ALIENS:
                            for (int j = 0; j < aliensFaction.GetCities().Count; j++)
                            {
                                cellGoals.Add(new CellGoal(aliensFaction.cities[i], priorities[i].weight));
                            }
                            break;
                        case Factions.USA:
                            for (int j = 0; j < usaFaction.GetCities().Count; j++)
                            {
                                cellGoals.Add(new CellGoal(usaFaction.cities[i], priorities[i].weight));
                            }
                            break;
                        case Factions.CCCP:
                            for (int j = 0; j < cccpFaction.GetCities().Count; j++)
                            {
                                cellGoals.Add(new CellGoal(cccpFaction.cities[i], priorities[i].weight));
                            }
                            break;
                    }
                    break;

                case Goal.TypeGoal.conquestMaterialCities:
                    //Recorrer la lista de ciudades del gameManager (gameManager.cities), comprobar que no pertencen a la facción y añadir todas las cities a cellGoals
                    for (int j = 0; j < gameManager.cities.Count; j++)
                    {
                        if (gameManager.cities[i].faction == Factions.NONE)
                        {
                            if (gameManager.cities[i].resourceType == CityCell.ResourceType.MATERIAL)
                            {
                                if (priorities[i].weight == 1)
                                    cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight));
                                else
                                    cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight - 1));
                            }
                            /*
                            else if (gameManager.cities[i].resourceType == CityCell.ResourceType.ENERGY)
                            {
                                if (priorities[i].weight == 10)
                                    cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight));
                                else if(priorities[i].weight == 9)
                                    cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight + 1));
                                else
                                    cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight + 2));
                            }
                            */
                        }
                        else
                        {
                            if (gameManager.cities[i].resourceType == CityCell.ResourceType.MATERIAL)
                            {
                                 cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight));
                            }
                            /*
                            else if (gameManager.cities[i].resourceType == CityCell.ResourceType.ENERGY)
                            {
                                if (priorities[i].weight == 10)
                                    cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight));
                                else if (priorities[i].weight == 9)
                                    cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight + 1));
                                else
                                    cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight + 2));
                            }
                            */
                        }
                    }
                    //Si son material cities, con más peso que las energy cities
                    //Poner peso dependiendo de si está capturada o no
                    break;
                case Goal.TypeGoal.conquestEnergyCities:
                    //Recorrer la lista de ciudades del gameManager (gameManager.cities), comprobar que no pertencen a la facción y añadir todas las cities a cellGoals
                    for (int j = 0; j < gameManager.cities.Count; j++)
                    {
                        if (gameManager.cities[i].faction == Factions.NONE)
                        {
                            if (gameManager.cities[i].resourceType == CityCell.ResourceType.ENERGY)
                            {
                                if (priorities[i].weight == 1)
                                    cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight));
                                else
                                    cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight - 1));
                            }
                            /*
                            else if (gameManager.cities[i].resourceType == CityCell.ResourceType.MATERIAL)
                            {
                                if (priorities[i].weight == 10)
                                    cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight));
                                else if (priorities[i].weight == 9)
                                    cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight + 1));
                                else
                                    cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight + 2));
                            }
                            */
                        }
                        else
                        {
                            if (gameManager.cities[i].resourceType == CityCell.ResourceType.ENERGY)
                            {
                                cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight));
                            }
                            /*
                            else if (gameManager.cities[i].resourceType == CityCell.ResourceType.MATERIAL)
                            {
                                if (priorities[i].weight == 10)
                                    cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight));
                                else if (priorities[i].weight == 9)
                                    cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight + 1));
                                else
                                    cellGoals.Add(new CellGoal(gameManager.cities[i], priorities[i].weight + 2));
                            }
                            */
                        }
                    }
                    //Si son energy cities, con más peso que las material cities
                    //Poner peso dependiendo de si está capturada o no
                    break;
                case Goal.TypeGoal.conquestMoon:
                        cellGoals.Add(new CellGoal(gameManager.moonCell, priorities[i].weight));
                    break;
            }
        }

    }
}
