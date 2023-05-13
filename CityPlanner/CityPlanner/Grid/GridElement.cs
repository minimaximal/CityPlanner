namespace CityPlanner.Grid;

public abstract class GridElement
{
    protected int Score = 0;
    protected int Level = 1;
    protected IDictionary<Data.GridType, double> Dependency = new Dictionary<Data.GridType, double>();

    public GridElement()
    {
        
    }
    public GridElement(GridElement gridElement)
    {
        Dependency = gridElement.Dependency;
    }

    public virtual int CalculateScore()
    {
        return 0;
    }

    public void AddDependency(Data.GridType gridType, int distance)
    {
        Dependency.Add(gridType, distance);
    }

    public virtual Data.GridType GetGridType()
    {
        return Data.GridType.Empty;
    }

    public bool IsValidStreet()
    {
        return Dependency.Any(dependency => dependency is { Key: Data.GridType.Empty, Value: <= 1 });
    }
    
    
    
    

}