namespace CityPlanner.Grid;

public class Commercial : GridElement
{
    public Commercial(GridElement gridElement) : base(gridElement) {}

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
                    if (dependency.Value <= 4.9)
                    {
                        Score += 2;
                    }
                    break;
                case Data.GridType.Commercial:
                    if (dependency.Value <= 2)
                    {
                        Score += 20;
                    }
                    else if (dependency.Value > 3.5)
                    {
                        Score -= 16;
                    }
                    break;
                case Data.GridType.Industry:  
                    if (dependency.Value <= 6)
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
        if (closestStreet <= 1.5)
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
        return Data.GridType.Commercial;
    }
    
}