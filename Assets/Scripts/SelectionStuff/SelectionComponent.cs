using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionComponent : MonoBehaviour {

    
    public HexGrid hexGrid;
    public PathFinding pathFinding;

    private float MaxDistance = 2000; //Used for raycasts
    private Unit unitSelected = null;
    private HexCell cellSelected = null;
    private HexCell currentCell = null;
    private bool cellSelectionMode = false;
    private Faction faction;

    void Start ()
    {
        faction = GetComponent<Faction>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !cellSelectionMode) //cellSelectionMode es true cuando ya tenemos una ud seleccionada y tenemos que escoger celda destino
        {
            //Después, comprobar si se ha clicado en algo
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, MaxDistance, LayerMask.GetMask("Cells")))
            {                
                cellSelected = hit.collider.GetComponent<HexCell>();
                if (cellSelected == null)
                {
                    cellSelected = hit.collider.GetComponentInParent<HexCell>();
                }
                //Comprobar si en la celda hay una unidad de nuestra facción
                if (cellSelected.unitInside != null && cellSelected.unitInside.faction == faction 
                    && !cellSelected.unitInside.moving && !cellSelected.unitInside.attacking)
                {
                    //showUnitHUD
                    //print("Unidad seleccionada");
                    unitSelected = cellSelected.unitInside;
                    cellSelectionMode = true;
                }
            }
        }

        if (cellSelectionMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, MaxDistance, LayerMask.GetMask("Cells")))
            {
                currentCell = hit.collider.GetComponent<HexCell>();
                if (currentCell == null)
                {
                    currentCell = hit.collider.GetComponentInParent<HexCell>();
                }
                //Ahora haciendo click izq podemos seleccionar celda destino para la ud
                //NOTA: COMPROBAR SI CURRENTCELL DEVUELVE NULL CUANDO NO ESTÁ EL RATÓN SOBRE UNA CELDA
                if (Input.GetMouseButtonDown(0) && currentCell != null && currentCell != cellSelected)
                {
                    //print("Enviando unidad a celda destino");
                    //Si la celda está vacía y aún no hemos movido a la unidad
                    if (currentCell.unitInside == null && !cellSelected.unitInside.movedThisTurn) //IMPORTANTE: FALTAN COMPROBACIONES DE LOS OTROS TIPOS DE CELDA
                    {
                        //FALTA COMPROBAR SI LA UNIDAD AÚN PUEDE MOVERSE
                        unitSelected.Move(pathFinding.FindPathHexagonal(unitSelected.GetUnitCell(), currentCell));
                        emptySelection();
                    }
                    //Si hay una ud enemiga y aún no hemos atacado con esta unidad                   
                    else if (currentCell.unitInside != null && unitSelected.faction != currentCell.unitInside.faction
                        && !cellSelected.unitInside.attackedThisTurn)
                    {
                        //Si la ud tiene rango para atacar a la ud enemiga
                        //if(calcular rango de alguna manera) { unitSelected.Attack(currentCell.unitInside); }
                        unitSelected.Attack(currentCell.unitInside);
                        emptySelection();
                    }
                }
            }
        }
        //Si hacemos click derecho y había algo seleccionado, lo deseleccionamos
        if (Input.GetMouseButtonDown(1) && cellSelectionMode)
        {
            emptySelection();
            //Hide HUD
        }
    }

    void emptySelection()
    {
        unitSelected = null;
        cellSelected = null;
        currentCell = null;
        cellSelectionMode = false;
    } 

}
