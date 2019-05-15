using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(HexCell),true)]
public class HexCellEditor : Editor {

    HexCell cell;

    public override void OnInspectorGUI()
    {
        cell = target as HexCell;

        cell.Type = (HexCell.CellType)EditorGUILayout.EnumPopup("Cell Type", cell.Type);
        //cell.Faction = (Factions)EditorGUILayout.EnumPopup("Faction", cell.Faction); //Comentado ya que el Dini ha hecho un sistemazo de cambio de faccion
        if(cell!=null) DrawDefaultInspector();
    }

}
