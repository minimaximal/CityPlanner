namespace CityPlanner.Grid;

public class GridElement
{
    protected int Score;
    protected int Level = 1;
    protected IDictionary<Data.GridType, List<double>> Dependency = new Dictionary<Data.GridType, List<double>>();

    public GridElement()
    {
        foreach (Data.GridType gridType in (Data.GridType[])Enum.GetValues(typeof(Data.GridType)))
        {
            Dependency.Add(gridType, new List<double>());
        }
    }
    
    public int getScore()
    {
        return Score;
    }

    protected GridElement(GridElement oldGridElement)
    {
        foreach (var dependency in oldGridElement.Dependency)
        {
            Dependency.Add(dependency.Key, oldGridElement.Dependency[dependency.Key].ToArray().ToList());
        }

        Score = oldGridElement.Score;
        Level = oldGridElement.Level;
    }

    public virtual int CalculateScore()
    {
        return -100;
    }

    protected virtual void UpdateLevel()
    {
        Level = 1;
    }


    public virtual void AddDependency(Data.GridType gridType, double distance)
    {
        if (gridType == Data.GridType.Street)
            Dependency[gridType].Add(distance);
    }

    public virtual Data.GridType GetGridType()
    {
        return Data.GridType.Empty;
    }

    public bool IsValidStreet()
    {
        Dependency[Data.GridType.Street].Sort();
        return Dependency[Data.GridType.Street].Any() && Dependency[Data.GridType.Street][0] <= 1.0;
    }

    public int GetLevel()
    {
        return Level;
    }


    public virtual GridElement Clone()
    {
        return new GridElement(this);
    }
    
    private double fn2(double x)
    {
        return  0.8-0.2*(x );
    }
    
    public double getwarscheinlichkeit()
    {
        //Retuns valu beeween 0 and 1 

        Dependency[Data.GridType.Street].Sort();
        int counter = 0;
        while (Dependency[Data.GridType.Street].Count>counter && Dependency[Data.GridType.Street][counter] <= 1)
        {
            counter++;
        }
        
        return fn2(counter);
    }

    public virtual bool isInRangeOfStreet()
    {
        return false;
    }
}