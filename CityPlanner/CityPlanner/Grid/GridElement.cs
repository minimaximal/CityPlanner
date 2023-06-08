//Author: Kevin Kern, Sander Stella

namespace CityPlanner.Grid;

public class GridElement
{
    protected int Score;
    protected int Level = 1;
    //holds Lists that contain distances to the next Mapelement
    protected IDictionary<Data.GridType, List<double>> Dependency = new Dictionary<Data.GridType, List<double>>();

    //Basic Constructor for initializing map
    public GridElement()
    {
        foreach (Data.GridType gridType in (Data.GridType[])Enum.GetValues(typeof(Data.GridType)))
        {
            Dependency.Add(gridType, new List<double>());
        }
    }

    //Advanced Constructor for replacing existing Mapelements with new ones
    protected GridElement(GridElement oldGridElement)
    {
        foreach (var dependency in oldGridElement.Dependency)
        {
            Dependency.Add(dependency.Key, oldGridElement.Dependency[dependency.Key].ToArray().ToList());
        }

        Score = oldGridElement.Score;
        Level = oldGridElement.Level;
    }
    
    public int GetScore()
    {
        return Score;
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

    //checks if Mapelement could possibly be a street
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
    
    //returns probability for a Mapelement becoming a street instead
    public double GetProbability()
    {
        //Returns value between 0 and 1 

        Dependency[Data.GridType.Street].Sort();
        int counter = 0;
        while (Dependency[Data.GridType.Street].Count>counter && Dependency[Data.GridType.Street][counter] <= 1)
        {
            counter++;
        }
        
        return 0.8-0.2*(counter );
    }
    
    //checks if there is a street nearby according to the rules for the specific Mapelement
    public virtual bool IsInRangeOfStreet()
    {
        return false;
    }
}