namespace CityPlanner.Grid;

public class Street : GridElement
{
    public Street(GridElement gridElement) : base(gridElement) {}
    public override int CalculateScore()
    {
        // todo change score caculation 
        // idee: prositiv starten und exponetiell schlimmer deto mehr straßen in der nähe sind
        Score = -10;
        return Score;
    }

    public override Data.GridType GetGridType()
    {
        return Data.GridType.Street;
    }
    public override Street Clone()
    {
        return new Street(this);
    }
    
}