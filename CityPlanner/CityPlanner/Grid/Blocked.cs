namespace CityPlanner.Grid;

public class Blocked : GridElement
{
    public override Data.GridType GetGridType()
    {
        return Data.GridType.Blocked;
    }

    public override int CalculateScore()
    {
        return 0;
    }
}