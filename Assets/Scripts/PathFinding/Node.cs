using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {

    public HexCell myCell;
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public float gCost = 0f;
    public float hCost = 0f;
    public float ownWeight = 1f;
    public Node parent;     //IMPORTANTE PONER BIEN EL PARENT
    int heapIndex;
    
    /*
    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
    }*/
    public Node(bool _walkable, Vector3 _worldPosition, HexCell _myCell)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        myCell = _myCell;
        ownWeight = _myCell.pfWeight;
    }
    public Node(bool _walkable, HexCell _myCell)
    {
        walkable = _walkable;
        worldPosition = _myCell.transform.position;
        myCell = _myCell;
        ownWeight = myCell.pfWeight;
    }

    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

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

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);     //in case of equal F cost, we compare H cost
        }

        /*we want to return 1 if the integer is lower*/
        return -compare;
    }
}
