namespace CityPlanner.Grid;

public class Highway : GridElement
{
    
    public Highway(GridElement gridElement) : base(gridElement)
    {
    }
    
    public override void AddDependency(Data.GridType gridType, double distance)
    {
        switch (gridType)
        {
            case Data.GridType.Street:
                Dependency[gridType].Add(distance);
                break;
        }
    }
    public override Data.GridType GetGridType()
    {
        return Data.GridType.Highway;
    }

    public override int CalculateScore()
    {
       Dependency[Data.GridType.Street].Sort();
       double closest =  Dependency[Data.GridType.Street].First();
       return (int)(10 * Math.Pow(5, (5 - closest)));
    }

    public override GridElement Clone()
    {
        return new Highway(this);
    }
}