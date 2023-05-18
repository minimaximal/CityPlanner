namespace CityPlanner.Grid;

public class Housing : GridElement
{
    private int people;
    public Housing(GridElement gridElement) : base(gridElement) {}
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
                    if (dependency.Value <= 4.9)
                    {
                        Score += 5;
                    }
                    break;
                case Data.GridType.Industry:  
                    if (dependency.Value <= 4.9)
                    {
                        Score -= 15;
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
        if (closestStreet >= 5)
        {
            //no Street in Range
            Score = 0;
        }
        else
        {
            Score += (int)(5 * (2 * Math.Sin(closestStreet - 0.5)));
        }
            
        //base cost
        Score -= 5;
        
        //according Level
        if (Score <= 25)
        {
            Level = 1;
            people = 8;
        } else if(Score is > 25 and <= 50)
        {
            Level = 2;
            people = 95;
        } else if(Score > 50)
        {
            Level = 3;
            people = 200 ;
        }
        return Score;
    }
    
    public int GetPeople()
    {
        return people;
    }

    public override Data.GridType GetGridType()
    {
        return Data.GridType.Housing;
    }
}