using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class nosAtacan : DecisionTreeNode
{
    public atacanBase nodeTrue;
    public recogerRecursos nodeFalse;

    bool cumple_condicion = true;
    public string decisionATomar; // me ataca el enemigo ?


    public override DecisionTreeNode MakeDecision()
    {
        return GetBranch();
    }

    public virtual Actions GetBranch()
    {
        if (cumple_condicion)
            return (Actions)nodeTrue.MakeDecision();

        else
            return (Actions)nodeFalse.MakeDecision();
    }



    public void setCondition(bool estadoActual)
    {
        cumple_condicion = estadoActual;
    }

    /*
        cumple_condicion = GameController.gamecontroller.getIfBeingAttacked(){
            
    }
    
    */

}