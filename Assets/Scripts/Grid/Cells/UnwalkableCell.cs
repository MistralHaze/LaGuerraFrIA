using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnwalkableCell : HexCell {

    public override void Awake()
    {
        base.Awake();
        pfWeight = float.PositiveInfinity;
        type = CellType.UNWALKABLE;
        walkable = false;
    }

    public override void Highlight()
    {
        throw new NotImplementedException();
    }

    public override void ToneDown()
    {
        throw new NotImplementedException();
    }

    public override void TurnToRed()
    {
        throw new NotImplementedException();
    }
}
