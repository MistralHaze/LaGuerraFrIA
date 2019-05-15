using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{

    public int side = 6;
    public int realSize = 0;

    public Color defaultColor = Color.white;

    public Text cellLabelPrefab;

    public bool createLabels = false, paintCenter = false;

    public UnwalkableCell unwalkCellPrefab;
    public PlainCell plainCellPrefab;
    public HQCell usaHQPrefab, cccpHQPrefab, alienHQPrefab;
    public DifficTerrainCell difficTerrainCellPrefab;

    public CityCell energyCityPrefab, materialCityPrefab, moonCityPrefab;
    public Canvas gridCanvas;

    public HexCell[] cells;
    public Text[] labels;


    //int triangulate = 0;

    public void Generate()
    {
        RemoveMap();

        cells = new HexCell[side * side];
        if (createLabels) labels = new Text[side * side];

        //triangulate = 0;

        for (int z = 0, i = 0; z < side; z++)
        {
            for (int x = 0; x < side - z; x++)
            {
                CreateCell(x, z, i++);
            }
        }

        InitializeAllNeighbors();
    }


    /*public void ColorCell (Vector3 position, Color color) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		HexCell cell = cells[index];
		cell.color = color;
		hexMesh.Triangulate(cells);
	}*/

    public HexCell GetCellInPosition(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * side + coordinates.Z / 2;
        //¿Aquí no faltaría una condición que evite el OutOfRange?
        return cells[index];
    }

    public void SaveMap(string path)
    {
        HexCellData[] cellsData = new HexCellData[cells.Length];

        for (int i = 0; i < cells.Length; i++)
        {
            HexCellData data = HexCell.TurnIntoCellData(cells[i]);
            cellsData[i] = data;
        }

        HexGridData mapData = new HexGridData(cellsData);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.OpenOrCreate);

        bf.Serialize(file, mapData);
        file.Close();

        Debug.Log("Save ended, you can now change scenes");
    }

    public void LoadMap(string path)
    {
        if (!File.Exists(path)) return;//Redundante pero por si acaso

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);
        HexGridData mapData = (HexGridData)bf.Deserialize(file);
        file.Close();

        RemoveMap();

        int cellAmount = mapData.cellData.Length;

        cells = new HexCell[cellAmount];
        if (createLabels) labels = new Text[side * side];

        for (int i = 0; i < cellAmount; i++)
        {
            HexCellData cellData = mapData.cellData[i];
            if (!cellData.valid) continue;
            int x = cellData.coordinates.X, z = cellData.coordinates.Z;
            CreateCell(x, z, i);
            cells[i].pfWeight = cellData.pfWeight;
            cells[i].faction = cellData.faction;
            cells[i].Type = cellData.type;
            //Debug.Log(cellData.type);
        }

        InitializeAllNeighbors();
    }

    public void RemoveMap()
    {
        if (cells != null && cells.Length > 0)
        {
            foreach (HexCell cell in cells)
            {
                if (cell != null)
                    DestroyImmediate(cell.gameObject);
            }

            foreach (Text label in labels)
            {
                if (label != null) DestroyImmediate(label.gameObject);
            }
        }

        cells = new HexCell[0];
        labels = new Text[0];
    }

    void InitializeAllNeighbors()
    {
        foreach(HexCell cell in cells)
        {
            if (cell != null) cell.InitializeNeighbors(cells);
        }
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f /*- z / 2*/) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(plainCellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.indexInParent = i;
        //cell.name = plainCellPrefab.name + "[" + cell.coordinates.X + ", " + cell.coordinates.Y + ", " + cell.coordinates.Z + "]";
        cell.Parent = this;
        cell.name = cell.coordinates.ToString();
        //cell.Color = defaultColor;
        realSize++;


        //Setting Neighbours
        //cell.InitializeNeighbors(cells);

        if (createLabels)
        {
            Text label = Instantiate<Text>(cellLabelPrefab);
            label.rectTransform.SetParent(gridCanvas.transform, false);
            label.rectTransform.anchoredPosition =
                new Vector2(position.x, position.z);
            label.rectTransform.sizeDelta = new Vector2(7,17);
            label.text = cell.coordinates.ToStringOnSeparateLines();
            label.color = Color.white;
            labels[i] = label;
            label.horizontalOverflow = HorizontalWrapMode.Overflow;
            label.verticalOverflow = VerticalWrapMode.Overflow;
        }

    }

    //Cell Neighbours Debugging. If later than 27_11 please erase me
    /*
    private void LateUpdate()
    {
        Debug.Log("West " + cells[12].GetNeighbor(HexDirection.W) + " SouthWest" + cells[12].GetNeighbor(HexDirection.SW) + " SouthEast" + cells[12].GetNeighbor(HexDirection.SE));
        Debug.Log("East " + cells[12].GetNeighbor(HexDirection.E) + " NorthEast" + cells[12].GetNeighbor(HexDirection.NE) + " NorthWest" + cells[12].GetNeighbor(HexDirection.NW));
    }
    */
}

[Serializable]
public struct HexGridData
{
    public HexCellData[] cellData;

    public HexGridData(HexCellData[] cellData)
    {
        this.cellData = cellData;
    }
}