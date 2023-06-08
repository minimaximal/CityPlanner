// Author: Kevin Kern

namespace CityPlanner.Grid;

public class Subway : GridElement
{
    public Subway(GridElement gridElement) : base(gridElement)
    {
    }

    public override void AddDependency(Data.GridType gridType, double distance)
    {
        switch (gridType)
        {
            case Data.GridType.Street:
            case Data.GridType.Commercial:
                Dependency[gridType].Add(distance);
                break;
            case Data.GridType.Housing:
                if (distance <= 4.5)
                    Dependency[gridType].Add(distance);
                break;
        }
    }

    public override int CalculateScore()
    {
        Score = 0;
        

        Score += Dependency[Data.GridType.Housing].Count * 50;
        if (Dependency[Data.GridType.Subway].Count > 0)
        {
            Score /= 2;
        }

        //base cost
        Score += 20;

        //Level
        UpdateLevel();

        return Score;
    }
    
    public override bool IsInRangeOfStreet()
    {
        return Dependency[Data.GridType.Street].Count() > 0;
    }

    public override Data.GridType GetGridType()
    {
        return Data.GridType.Subway;
    }

    public override Subway Clone()
    {
        return new Subway(this);
    }
}