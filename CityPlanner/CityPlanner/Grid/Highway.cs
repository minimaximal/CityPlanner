namespace CityPlanner.Grid;

public class Highway : GridElement
{
    
    public Highway(GridElement gridElement) : base(gridElement)
    {
    }
    
    public override void AddDependency(Data.GridType gridType, double distance)
    {
    
    }
    public override Data.GridType GetGridType()
    {
        return Data.GridType.Highway;
    }

    public override int CalculateScore()
    {
        return 0;
    }

    public override GridElement Clone()
    {
        return new Highway(this);
    }
}