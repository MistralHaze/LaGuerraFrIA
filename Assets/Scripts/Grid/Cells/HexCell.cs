using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public abstract class HexCell : MonoBehaviour
{
    public enum CellType { PLAIN, HQ, DIFFTERRAIN, CITYENERGY, CITYMATS, CITYMOON, UNWALKABLE }

    //public HexCell usaModel, cccpModel, alienModel; //SE DEBE INICIALIZAR DESDE FUERA

    public HexCoordinates coordinates;
    public float pfWeight = 1;
    public int indexInParent;
    public Factions faction = Factions.NONE;
    public Unit unitInside;


    public bool walkable = true;

    //Color color;
    public HexGrid parentGrid;
    [SerializeField]
    HexCell[] neighbors;

    protected CellType type;


    #region Getters && Setters
    /*
    public Factions Faction
    {
        get { return faction; }

        set { faction = value; SwapFaction(); }
    }*/
    /*
    public Color Color
    {
        get { return color; }
        set
        {
            if (rend == null) rend = GetComponent<MeshRenderer>();

            color = value;
            rend.sharedMaterial.SetColor("_Color", color);
        }
    }*/

    public CellType Type
    {
        get { return type; }
        set
        {
            if (value != type)
            {
                type = value;
                ChangeCellType();
            }
        }
    }

    public HexGrid Parent
    {
        get { return parentGrid; }
        set { parentGrid = value; }
    }

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }
    public HexCell[] GetAllNeighbors()
    {
        return neighbors;
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public void InitializeNeighbors(HexCell[] cells)
    {
        int i = indexInParent, side = parentGrid.side,
            x = coordinates.X, z = coordinates.Z;
        neighbors = new HexCell[6];

        if (x > 0)
        {
            SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if (z > 0)
        {
            if ((z & 1) == 0) // & bitwise operator. Even numbers are always full of 1 (1=2,11=4,111=8,etc)
            {
                SetNeighbor(HexDirection.SE, cells[i - side + z]); //Hay que tener en cuenta la diferencia de alturas( para las contrarias, los nortes, se hace automatico)
                if (x > 0)
                {
                    SetNeighbor(HexDirection.SW, cells[i - side - 1 + z]);
                }
            }
            else
            {
                SetNeighbor(HexDirection.SW, cells[i - side + z - 1]);
                if (x < side - 1)
                {
                    SetNeighbor(HexDirection.SE, cells[i - side + z]);
                }
            }
        }
    }

    #endregion

    public virtual void Awake()
    {
        parentGrid = transform.parent.GetComponent<HexGrid>();

        pfWeight = 1;
        walkable = true;
    }

    public virtual void TurnToColor(string color) { }
    public abstract void TurnToRed();
    public abstract void Highlight();
    public abstract void ToneDown();

    /*
    private void SwapFaction()
    {
        HexCell newModel = null;

        switch (faction)
        {
            case Factions.ALIENS: newModel = alienModel; break;

            case Factions.USA: newModel = usaModel; break;

            case Factions.CCCP: newModel = cccpModel; break;
        }

        HexCell newCell = Instantiate(newModel, transform.position, transform.rotation, transform.parent).GetComponent<HexCell>();
        CopyCellData(this, newCell);
        newCell.name = newModel.name + "[" + coordinates.X + ", " + coordinates.Y + ", " + coordinates.Z + "]";
        newCell.transform.localScale = transform.localScale;

        Destroy(gameObject);
    }*/
    /*
    void Update()
    {
        if(unitInside != null)
        {
            print("Soy la celda " + this.gameObject.name + " y se ha actualizado mi variable unidad");
        }
    }
    */

    private void ChangeCellType()
    {
        HexCell newModel = parentGrid.plainCellPrefab;

        switch (type)
        {
            case CellType.PLAIN:
                newModel = parentGrid.plainCellPrefab; break;

            case CellType.HQ:
                switch (faction)
                {
                    case Factions.USA: newModel = parentGrid.usaHQPrefab; break;
                    case Factions.CCCP: newModel = parentGrid.cccpHQPrefab; break;
                    case Factions.ALIENS: newModel = parentGrid.alienHQPrefab; break;
                }
                break;

            case CellType.DIFFTERRAIN: newModel = parentGrid.difficTerrainCellPrefab; break;

            case CellType.CITYMATS: newModel = parentGrid.materialCityPrefab; break;

            case CellType.CITYENERGY: newModel = parentGrid.energyCityPrefab; break;

            case CellType.CITYMOON: newModel = parentGrid.moonCityPrefab; break;

            case CellType.UNWALKABLE: newModel = parentGrid.unwalkCellPrefab; break;
        }

        HexCell newCell = Instantiate(newModel).GetComponent<HexCell>();
        newCell.transform.parent = transform.parent;
        newCell.transform.position = transform.position;

        newCell.coordinates = coordinates;
        newCell.indexInParent = indexInParent;
        newCell.parentGrid = parentGrid;

        newCell.name = newModel.name + "[" + coordinates.X + ", " + coordinates.Y + ", " + coordinates.Z + "]";
        newCell.type = type;
        parentGrid.cells[indexInParent] = newCell;
        Selection.objects = new UnityEngine.Object[] { newCell.gameObject };

        DestroyImmediate(gameObject);
    }

    public static HexCellData TurnIntoCellData(HexCell cell)
    {
        if (cell == null) return new HexCellData(null, false);
        return new HexCellData(cell);
    }
}

[Serializable]
public struct HexCellData
{
    public HexCoordinates coordinates;
    public float pfWeight;
    public Factions faction;
    public HexCell.CellType type;
    public bool valid;

    public HexCellData(HexCell cell, bool isValid = true)
    {
        if (isValid){
            coordinates = cell.coordinates;
            pfWeight = cell.pfWeight;
            faction = cell.faction;
            type = cell.Type;
        }
        else
        {
            coordinates = new HexCoordinates(-50, -50);
            pfWeight = float.PositiveInfinity;
            faction = Factions.ALIENS;
            type = HexCell.CellType.PLAIN;
        }
        
        valid = isValid;
    }

}