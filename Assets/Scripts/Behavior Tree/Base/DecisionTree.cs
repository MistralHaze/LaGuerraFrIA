using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DecisionTree : DecisionTreeNode
{
    public DecisionTreeNode root; //Hay que pasarle la cima del arbol
    private Actions actionNew;
    private Actions actionOld;

    public override DecisionTreeNode MakeDecision()
    {
        return root.MakeDecision();
    }

    void Update()
    {
        actionNew.activated = false; //Desactiva la anterior accion
        actionOld = actionNew;        //Marca la anterior accion como vieja
        actionNew = root.MakeDecision() as Actions;    //Empieza la recursividad
        if (actionNew == null)        //Si por algun casual le devolvemos null, le decimos que siga haciendo la misma accion
            actionNew = actionOld;
        actionNew.activated = true;   //Empieza la nueva accion
    }
}
