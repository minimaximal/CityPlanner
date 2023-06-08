namespace CityPlanner.MapElements;

public class Highway : MapElement
{
    
    public Highway(MapElement mapElement) : base(mapElement)
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

    public override MapElement Clone()
    {
        return new Highway(this);
    }
}