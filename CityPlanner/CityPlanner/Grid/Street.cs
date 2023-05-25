namespace CityPlanner.Grid;

public class Street : GridElement
{
    public Street(GridElement gridElement) : base(gridElement) {}
    public override int CalculateScore()
    {
        // todo change score caculation 
        // idee: prositiv starten und exponetiell schlimmer deto mehr straßen in der nähe sind
        Score = 0;

        int nearbyStreet = 0;
        foreach (double street in Dependency[Data.GridType.Street])
        {
            if (street <= 1.5)
            {
                nearbyStreet++;
            }
        }

        if (nearbyStreet > 4)
        {
            Score -= (nearbyStreet - 4) * 5;
        }
        else
        {
            Score += 20;
        }
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