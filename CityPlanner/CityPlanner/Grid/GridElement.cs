namespace CityPlanner.Grid;

public class GridElement:ICloneable
{
    protected int Score = 0;
    protected int Level = 1;
    //Todo: This does not work / key cant be duplicated change to list<(key,value)>
    protected IDictionary<Data.GridType, List<double>> Dependency = new Dictionary<Data.GridType, List< double>>();
    
    public GridElement()
    {
        foreach (Data.GridType gridType in (Data.GridType[]) Enum.GetValues(typeof(Data.GridType)))
        {
            Dependency.Add(gridType,new List<double>());
        }
    }
    public GridElement(GridElement gridElement)
    {
        Dependency = gridElement.Dependency;
    }

    public virtual int CalculateScore()
    {
        return 0;
    }

    

    public void AddDependency(Data.GridType gridType, double distance)
    {
        Dependency[gridType].Add(distance);
    }

    public virtual Data.GridType GetGridType()
    {
        return Data.GridType.Empty;
    }

    public bool IsValidStreet()
    {   
        foreach (double range in Dependency[Data.GridType.Street])
        {
            if (range == 1)
            {
                return true;
            }
        }
        return false;
    }

    public int GetLevel()
    {
        return Level;
    }


    public object Clone()
    {
        return new GridElement
        {
            Dependency = new Dictionary<Data.GridType, List< double>>(this.Dependency),
            Score = this.Score,
            Level = this.Level
        };
    }
}