using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(HexGrid))]
public class HexGridDrawer : Editor
{

    HexGrid grid;

    HexCell selected;

    bool showingPrefabs = false;

    static string extension = "mapInfo";

    public override void OnInspectorGUI()
    {
        grid = target as HexGrid;


        if (showingPrefabs)
        {
            if (GUILayout.Button("Hide Values"))
            {
                showingPrefabs = false;
            }

            GUILayout.Label("Prefabs");

            //Enseña los prefabs
            DrawDefaultInspector();
        }
        else if (GUILayout.Button("Show Values"))
        {
            showingPrefabs = true;
        }
        /*
        if (selected != null)
        {
        
        }*/

        if (GUILayout.Button("Generate Map"))
        {
            Undo.RecordObject(grid, "Generate Map");
            grid.Generate();
            EditorUtility.SetDirty(grid);
        }

        if (GUILayout.Button("Remove Map"))
        {
            Undo.RecordObject(grid, "Remove Map");
            grid.RemoveMap();
            EditorUtility.SetDirty(grid);
        }

        if (GUILayout.Button("Save Map"))
        {
            var path = EditorUtility.SaveFilePanel("Save map", Application.dataPath, "Mapachemandoni", extension);
            grid.SaveMap(path);
        }

        if (GUILayout.Button("Load Map"))
        {
            var path = EditorUtility.OpenFilePanel("OpenMap", Application.dataPath, extension);
            grid.LoadMap(path);
            EditorUtility.SetDirty(grid);
        }
    }

    private void OnSceneGUI()
    {
        grid = target as HexGrid;

        DrawGrid(grid);
    }

    void DrawGrid(HexGrid grid)
    {
        for (int z = 0, i = 0; z < grid.side; z++)
        {
            for (int x = 0; x < grid.side - z; x++)
            {
                if (IsCenter(x, z))
                    Handles.color = Color.red;
                else
                    Handles.color = Color.blue;

                DrawCell(x, z, i++);
            }
        }
    }

    private bool IsCenter(int x, int z)
    {
        if(x > grid.side / 2f && x > (grid.side+1) / 2f 
            && z > grid.side / 2f && z > (grid.side + 1) / 2f)
            return true;

        return false;
    }

    private void DrawCell(int x, int z, int v)
    {

        Vector3 center;
        center.x = (x + z * 0.5f) * (HexMetrics.innerRadius * 2f);
        center.y = 0f;
        center.z = z * (HexMetrics.outerRadius * 1.5f);

        Vector3 p0, p1;

        for (int i = 0; i < 6; i++)
        {
            p0 = HexMetrics.corners[i] + center;
            p1 = HexMetrics.corners[i + 1] + center;
            Handles.DrawLine(p0, p1);

        }
    }
}
