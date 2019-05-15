using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Decision : DecisionTreeNode
{
    public Actions nodeTrue;     //Nodo al que ir si se cumple la condicion establecida P ej: Me están atacando-> Me defendere
    public Actions nodeFalse;     //Nodo al que ir si la condicion establecida no se cumple P ej: No me están atacando-> recogere recursos  

    bool cumple_condicion = false;  //Booleano del que leemos la condicion
    public string decisionATomar; // String que describe sobre lo que se toma la decision: P ee: Me estan atacando


    public override DecisionTreeNode MakeDecision()
    {
        return GetBranch();
    }

    public virtual Actions GetBranch()
    {
        if (cumple_condicion)
            return (Actions) nodeTrue.MakeDecision();

        else 
            return (Actions) nodeFalse.MakeDecision();
    }



public void setCondition(bool estadoActual)
    {
    cumple_condicion = estadoActual;
    }
        
}
