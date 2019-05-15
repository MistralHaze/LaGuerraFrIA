using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Allocation : MonoBehaviour
{

    public int maxDistance = 15;
    public GameManager gameManager;
    public PathFinding pathfinder;
    public AnimationCurve allocationCurve;

    List<CellGoal> cellGoals;
    List<Unit> units;
    Unit[] enemyUnits;

    //Relacionar recursos como unidades con goals 
    public void StartUnitGoals(List<CellGoal> cellGoals, List<Unit> units, Unit[] enemyUnits)
    {
        this.cellGoals = cellGoals;
        this.units = units;
        this.enemyUnits = enemyUnits;

        connectGoalsWithUnits();

    }

    //Conectar goals con unidades
    void connectGoalsWithUnits()
    {
        Unit[] avaUnitsArray = new Unit[units.Count];
        units.CopyTo(avaUnitsArray);

        List<Unit> availableUnits = new List<Unit>(avaUnitsArray);

        //Recorremos la lista de celdas objetivo

        for (int i = 0; i < cellGoals.Count; i++)
        {
            if (availableUnits.Count <= 0) break;

            float proportionCurve = (float)i / ((float)cellGoals.Count - 1f);
            int allocatedUnitsNum = Mathf.FloorToInt(units.Count *  allocationCurve.Evaluate(proportionCurve));

            if (availableUnits.Count < allocatedUnitsNum) allocatedUnitsNum = availableUnits.Count;

            Unit[] allocatedUnits = new Unit[allocatedUnitsNum];

            Heap<UnitHeapable> heapUnits = new Heap<UnitHeapable>(availableUnits.Count);

            foreach (Unit unit in availableUnits)
            {
                //pathfinder.CalculaGridParaUnidad(unit, enemyUnits)
                pathfinder.SetUnitMap(unit, cellGoals);
                //Calcular la distancia desde la unidad hasta la celda objetivo goal.cell
                unit.SetPath(pathfinder.FindPathHexagonalAI(unit.GetUnitCell(), cellGoals[i].cell));
                heapUnits.Add(new UnitHeapable(unit, unit.GetPath().Count));
            }

            //Quitamos las unidades de mayor distancia hasta tener las adecuadas para la tarea
            while (heapUnits.Count > allocatedUnitsNum)
                heapUnits.RemoveFirst();

            //Hacer movimiento/acción de la unidad y removerla de units para que ningún otro objetivo la tenga en cuenta.

            //Los que quedan son los seleccionados para la tarea.
            //Como ya tienen su camino asignado, no necesitan más manipulación
            while (heapUnits.Count > 0)
            {
                Unit unit = heapUnits.RemoveFirst().unit;
                availableUnits.Remove(unit);
                unit.Move();
            }
                


        }
    }

}
