public class Goal{

    public enum TypeGoal
    {
        attackCCP ,attackUSA, attackAliens, defendBase, defendCities,
        conquestMaterialCities, conquestEnergyCities, conquestMoon
    }

    public TypeGoal objective;
    
    //De 1 a 10 siendo uno el máximo de prioridad.
    public int weight;

    public Goal(TypeGoal objective, int requestedWeight)
    {
        this.objective = objective;
        weight = requestedWeight;

    }
}
