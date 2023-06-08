namespace CityPlanner.Grid;

public class Blocked : GridElement
{
    public Blocked(GridElement gridElement) : base(gridElement)
    {
    }
    
    public override Data.GridType GetGridType()
    {
        return Data.GridType.Blocked;
    }

    public override int CalculateScore()
    {
        return 0;
    }

    public override GridElement Clone()
    {
        return new Blocked(this);
    }
}