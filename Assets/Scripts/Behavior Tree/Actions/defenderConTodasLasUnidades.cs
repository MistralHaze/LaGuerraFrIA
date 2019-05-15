using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class defenderConTodasLasUnidades : DecisionTreeNode
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
        print("DEFENSA");
        // Implement your behaviors here
    }
}
