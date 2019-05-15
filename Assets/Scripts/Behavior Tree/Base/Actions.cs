using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Actions : DecisionTreeNode
{
    public bool activated = false; //Si la accion la esta haciendo ahora mismo la IA esto sera true
    public override DecisionTreeNode MakeDecision()
    {
        return this;
    }

    public virtual void LateUpdate()
    {
        if (!activated)
            return;
        // Implement your behaviors here
    }
}
