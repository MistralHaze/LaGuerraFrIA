public class CellGoal
{

    public HexCell cell;

    //De 1 a 10 siendo uno el máximo de prioridad.
    public int weight;

    public CellGoal(HexCell cell, int requestedWeight)
    {
        this.cell = cell;
        weight = requestedWeight;

    }
}

