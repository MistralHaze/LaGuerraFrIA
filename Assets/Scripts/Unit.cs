using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public float Health;
    public float Damage;
    public int MaxCellsRange;
    public float Speed;
    public int AttackRange;
    public Faction faction;
    public HexGrid hexGrid;
    public bool movedThisTurn;
    public bool attackedThisTurn;
    public string CityColor;

    [HideInInspector] public bool moving = false;
    [HideInInspector] public bool attacking = false;

    private shooting shoot;
    private List<Node> currentPath;
    private int pathIndex = 0; //Para recorrer la lista del path    
    private Vector3 target;
    private HexCell currentCell;
    private bool lastCellIsCity = false;

    public enum unitType
    {
        infantry, antiTank, Tank
    }

    public unitType type = unitType.infantry;
    public Factions myFaction;



    // Use this for initialization
    void Start ()
    {
        shoot = GetComponentInChildren<shooting>();
        movedThisTurn = false;
        attackedThisTurn = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (moving)
        {
            float step = Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            if (Vector3.Distance(transform.position, target) < Mathf.Epsilon)
            {
                updateTarget();
            }
        }       
	}

    void updateTarget()
    {
        if (pathIndex < currentPath.Count-1)
        {
            pathIndex++;
            target = currentPath[pathIndex].worldPosition;
            Aim(target);
        }
        else
        {
            moving = false;
            //GetComponent<Floating>().enabled = true;
        }
    }


    void OnTriggerEnter(Collider cell)
    {
        if (cell.gameObject.layer == LayerMask.NameToLayer("Cells"))
        {

            //Nos guardamos el componente HexCell, bien de la propia celda o de su padre (porque depende de la celda)
            HexCell cellCollided = cell.gameObject.GetComponent<HexCell>();
            if (cellCollided == null)
            {
                cellCollided = cell.gameObject.GetComponentInParent<HexCell>();
            }
            //Antes de actualizar vaciamos la celda anterior y la oscurecemos de nuevo, esto último solamente si no era de tipo ciudad
            if (currentCell != null)
            {
                currentCell.unitInside = null;
                if (!lastCellIsCity){ currentCell.ToneDown(); }
            }
            //Ahora ya actualizamos a la nueva celda            
            currentCell = cellCollided;
            currentCell.unitInside = this;
            //Para el color depende si la celda es una ciudad, porque esas se pueden controlar y entonces hay que resaltar del color de la facción
            if (cellCollided.Type != HexCell.CellType.CITYENERGY && cellCollided.Type != HexCell.CellType.CITYMATS &&
                cellCollided.Type != HexCell.CellType.CITYMOON)
            {                
                //Hacemos highlight de la celda
                currentCell.Highlight();
                lastCellIsCity = false;
            }
            else
            {
                cellCollided.TurnToColor(CityColor);
                cellCollided.faction = myFaction;
                faction.AddCity(cell.gameObject.GetComponentInParent<CityCell>());
                lastCellIsCity = true;
            }
        }
    }
    //Para comprobar si el camino seleccionado por el jugador/IA es correcto
    //No basta con contar el número de nodos recibidos, ya que las celdas de terreno difícil valen por 2
    bool checkIfPathIsCorrect(List<Node> path)
    {
        int steps = 0;
        for (int i = 0; i < path.Count; i++)
        {
            if (path[i].myCell.Type == HexCell.CellType.DIFFTERRAIN)
            {
                steps += 2;
            }
            else
            {
                steps++;
            }
        }
        
        return steps <= MaxCellsRange;
    }

    public void Aim(Vector3 aimTarget)
    {
        transform.LookAt(aimTarget);
    }

    public void Move(List<Node> path)
    {
        if (path != null && path.Count > 0 && checkIfPathIsCorrect(path))
        {
            movedThisTurn = true;
            currentPath = path;
            pathIndex = 0;
            moving = true;
            target = currentPath[pathIndex].worldPosition;
            Aim(target);
            //GetComponent<Floating>().enabled = false; //Desactivamos el efecto de flotar que sino el Vector3.MoveTowards no funciona
        }
        else if (path != null)
        {
            path[path.Count - 1].myCell.TurnToRed();
        }
    }

    public void Move()
    {
        Move(currentPath);
    }

    public void Attack(Unit enemy)
    {
        if (Vector3.Distance(this.transform.position, enemy.transform.position) < AttackRange)
        {
            attackedThisTurn = true;
            attacking = true;
            //Girar unidad hacia el enemigo
            Aim(enemy.gameObject.transform.position);
            //Ejecutar animación de ataque
            shoot.attack(enemy.gameObject);
            //Destruir enemigo si ha muerto
        }
    }

    public HexCell GetUnitCell()
    {
        return currentCell;
    }

    public void TakeDamage (float dmg)
    {
        Health -= dmg;
        if (Health <= 0)
        {
            faction.RemoveUnit(this);
            Destroy(this.gameObject);
        }
    }

    #region Getters&Setters
    public void SetPath(List<Node> path)
    {
        currentPath = path;
    }

    public List<Node> GetPath() { return currentPath; }

    #endregion
}

public class UnitHeapable : IHeapItem<UnitHeapable>
{
    public Unit unit;
    public int distanceToGoal;

    int heapIndex;

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }

        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(UnitHeapable other)
    {
        //Queremos dar prioridad a los caminos largos para quitarlos antes

        return distanceToGoal.CompareTo(other.distanceToGoal);
    }

    public UnitHeapable(Unit unit, int distanceToGoal)
    {
        this.unit = unit;
        this.distanceToGoal = distanceToGoal;
    }
}
