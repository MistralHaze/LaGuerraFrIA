using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class PathFinding : MonoBehaviour
{
    //if triangle size is changed this needs to be changed
    public const int triangleSize = 325;

    //Grid grid;
    public HexGrid hg;
    private HexCell[] cells;
    public Node[] nodesBlueprint, unitMapVision;
    Node neighbour;

    public HexCell START, END;
    public List<Node> pathGizmos;
    public bool displayPath = false;


    [Header("Unit Vision Values")]
    public float objectiveMultiplier = 0.8f;

    void Start()
    {
        cells = hg.GetComponent<HexGrid>().cells;
        nodesBlueprint = new Node[cells.Length];

        for (int i = 0; i < triangleSize; i++)
        {
            if (cells[i] == null) continue;
            nodesBlueprint[i] = new Node(cells[i].walkable, cells[i]);
        }

    }

    public void SetUnitMap(Unit unit, List<CellGoal> goals)
    {
        //Aquí se genera el mapa específico de la visión de la unidad,
        //que altera sus pesos dependiendo de si se es enemigo u objetivo

        unitMapVision = new Node[nodesBlueprint.Length];

        //Nos copiamos el grid original haciendo los cambios de pesos necesarios
        for (int i = 0; i < nodesBlueprint.Length; i++)
        {
            if (nodesBlueprint == null) continue;
            unitMapVision[i] = SetNodeWeightForUnit(nodesBlueprint[i], goals);
        }

        //Aquí hacemos convolución
        //unitMapVision = CONVOLUTEAMEESTA(unitMapVision);
        unitMapVision = Convolution(unitMapVision);
    }

    private Node[] Convolution(Node[] unitMapVision)
    {
        Node[] newMap = new Node[unitMapVision.Length];

        //Las hexoMat empiezan por el centro y van desde la 
        //celda de arriba en sentido horario
        float[] hexoMat = new float[] { 3, 1, 1, 1, 1, 1, 1 };

        for(int i=0; i<unitMapVision.Length; i++)
        {
            Node node = unitMapVision[i];
            if (node == null || float.IsInfinity(node.ownWeight)) continue;

            //Recorremos la propia celda y sus vecinas multiplicando los pesos de
            //la matriz de convolución y contándolos para posteriormente dividir
            float evaluated = hexoMat[0],
                value = node.ownWeight * hexoMat[0];

            HexCell[] neighbors = unitMapVision[i].myCell.GetAllNeighbors();
            for(int j = 0; j<6; j++)//Por lógica, un hexágono tiene 6 vecinos
            {
                if (neighbors[j] == null) continue;

                Node neighbor = unitMapVision[neighbors[j].indexInParent];
                if (float.IsInfinity(neighbor.ownWeight)) continue;

                value += neighbor.ownWeight * hexoMat[j + 1];
                evaluated += hexoMat[j + 1];
            }

            newMap[i] = new Node(node.walkable, node.myCell);
            newMap[i].ownWeight = value / evaluated;
        }

        return newMap;
    }

    private Node SetNodeWeightForUnit(Node node, List<CellGoal> goals)
    {
        Node newNode = new Node(node.walkable, node.myCell);

        bool isGoal = false;
        //Buscamos si el nodo es un goal
        foreach (CellGoal goal in goals)
            if (goal.cell == node.myCell)
            {
                isGoal = true;
                break;
            }
        if (isGoal) newNode.ownWeight *=objectiveMultiplier;

        return newNode;
    }

    public List<Node> FindPathHexagonalAI(HexCell startPos, HexCell targetPos)
    {
        List<Node> path = new List<Node>();
        //Como las dos listas son iguales, averiguamos el nodo que es gracias al índice de la hexcell.
        Node startNode = unitMapVision[startPos.indexInParent];
        Node targetNode = unitMapVision[targetPos.indexInParent];

        Heap<Node> openSet = new Heap<Node>(cells.Length);
        HashSet<Node> closeSet = new HashSet<Node>();

        openSet.Add(startNode);


        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            //print("Meto a closed el nodo: " + currentNode.myCell.name);
            closeSet.Add(currentNode);

            if (currentNode == targetNode)      //path found
            {
                //print("Path found " + sw.ElapsedMilliseconds + " ms");
                path = RetracePath(startNode, targetNode);
                return path;
            }
            /* foreach neighbour of the current node
                if neighbour is null or not traversable or neighbour is in closed
                    skip to the next neighbour
            */
            foreach (HexCell neighbourAux in currentNode.myCell.GetAllNeighbors())
            {
                if (neighbourAux == null)
                    continue;
                else
                {
                    neighbour = unitMapVision[neighbourAux.indexInParent];
                    if (!neighbour.walkable || closeSet.Contains(neighbour))
                    {
                        //print("me salto el nodo: " + neighbourAux.name);
                        continue;           //skips this iteration of the bucle
                    }
                }



                /* if (neighbourAux == null || !neighbour.walkable || closeSet.Contains(neighbour))
                 {
                     continue;           //skips this iteration of the bucle
                 }*/
                //if the new path to the neighbour is shorter OR neighbour is not in open
                /*
                set fcost of neighbour
                set parent of neighbour to current
                if neighbour is not in open
                    add neighbour to open    
                */

                //print("Soy el vecino llamado " + neighbourAux.name + " y mi indice es:  " + neighbourAux.indexInParent + "y me llama el nodo " + currentNode.myCell.name);
                neighbour = unitMapVision[neighbourAux.indexInParent];

                float newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) * currentNode.ownWeight;
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))       //si nunca lo has visitado o llegas por un camino más óptimo
                {
                    neighbour.gCost = newMovementCostToNeighbour;

                    //Heurística de movimiento
                    neighbour.hCost = GetDistance(neighbour, targetNode);

                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbour);
                    }

                }
            }
        }

        return null;
    }

    public List<Node> FindPathHexagonal(HexCell startPos, HexCell targetPos)
    {
        List<Node> path = new List<Node>();
        //Como las dos listas son iguales, averiguamos el nodo que es gracias al índice de la hexcell.
        Node startNode = nodesBlueprint[startPos.indexInParent];
        Node targetNode = nodesBlueprint[targetPos.indexInParent];

        Heap<Node> openSet = new Heap<Node>(cells.Length);
        HashSet<Node> closeSet = new HashSet<Node>();

        openSet.Add(startNode);


        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            //print("Meto a closed el nodo: " + currentNode.myCell.name);
            closeSet.Add(currentNode);

            if (currentNode == targetNode)      //path found
            {
                //print("Path found " + sw.ElapsedMilliseconds + " ms");
                path = RetracePath(startNode, targetNode);
                return path;
            }
            /* foreach neighbour of the current node
                if neighbour is null or not traversable or neighbour is in closed
                    skip to the next neighbour
            */
            foreach (HexCell neighbourAux in currentNode.myCell.GetAllNeighbors())
            {
                if (neighbourAux == null)
                    continue;
                else
                {
                    neighbour = nodesBlueprint[neighbourAux.indexInParent];
                    if (!neighbour.walkable || closeSet.Contains(neighbour))
                    {
                        //print("me salto el nodo: " + neighbourAux.name);
                        continue;           //skips this iteration of the bucle
                    }
                }



                /* if (neighbourAux == null || !neighbour.walkable || closeSet.Contains(neighbour))
                 {
                     continue;           //skips this iteration of the bucle
                 }*/
                //if the new path no the neighbour is shorter OR neighbour is not in open
                /*
                set fcost of neighbour
                set parent of neighbour to current
                if neighbour is not in open
                    add neighbour to open    
                */

                //print("Soy el vecino llamado " + neighbourAux.name + " y mi indice es:  " + neighbourAux.indexInParent + "y me llama el nodo " + currentNode.myCell.name);
                neighbour = nodesBlueprint[neighbourAux.indexInParent];

                float newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) * currentNode.ownWeight;
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))       //si nunca lo has visitado o llegas por un camino más óptimo
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbour);
                    }

                }
            }
        }

        return null;
    }


    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        //print("CAMINO ");
        while (currentNode != startNode)
        {
            //print(currentNode.myCell.name + "siguiente ");
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        //print("FINAL xd");
        path.Reverse();
        pathGizmos = path;
        displayPath = true;
        return path;
        /*
        List<Node> positions = new List<Node>();

        int size = path.Count;

        for (int i = 0; i < size; i++)
        {
            //UnityEngine.Debug.Log(size);
            positions.Add(path[i].worldPosition);
            //positions[i] = path[i].worldPosition;
        }

        grid.path = path;

        return positions;*/
    }

    float GetDistance(Node nodeA, Node nodeB)     //magic
    {
        Transform coordA, coordB;
        coordA = nodeA.myCell.transform;
        coordB = nodeB.myCell.transform;
        float distance = Vector3.Distance(coordA.position, coordB.position);
        //print("distace" + distance);
        return distance;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(START.transform.position, 5);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(END.transform.position, 5);
        if (displayPath)
        {
            Gizmos.color = Color.white;
            foreach (Node i in pathGizmos)
            {
                Gizmos.DrawSphere(i.myCell.transform.position, 5);
            }

        }
    }
}
