namespace CityPlanner.Grid;

public class Industry : GridElement
{
    public Industry(GridElement gridElement) : base(gridElement) {}

    public override int CalculateScore()
    {
        IDictionary<Data.GridType, double> OrderedDependency = new Dictionary<Data.GridType, double>();
        OrderedDependency = Dependency.OrderBy(key => key.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
        double closestStreet = 5;
        foreach (var dependency in OrderedDependency)
        {
            //general dependencies
            switch (dependency.Key)
            {
                case Data.GridType.Housing:
                    //who knows...
                    break;
                case Data.GridType.Commercial:
                    //who knows...
                    break;
                case Data.GridType.Industry:  
                    if (dependency.Value <= 2.5)
                    {
                        Score += 10;
                    }
                    break;
                case Data.GridType.Street: 
                    if (dependency.Value < closestStreet)
                    {
                        closestStreet = dependency.Value;
                    }
                    break;
                case Data.GridType.Empty:
                    break;
                
            }
        }
        //availability of street
        if (closestStreet <= 1)
        {
            // Street in Range
            Score += 20;
        }
        else
        {
            //no Street in Range
            Score = 0;
        }
            
        //base cost
        Score -= 5;
        
        return Score;
    }

    public override Data.GridType GetGridType()
    {
        return Data.GridType.Industry;
    }
    
}