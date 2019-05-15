using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTreeNode : MonoBehaviour
{
    /*
    Clase padre, las demás hacen override y esta clase sufre la soledad*/

    public virtual DecisionTreeNode MakeDecision()
    {
        return null;
    }
}
