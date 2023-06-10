// @author: Kevin Kern

namespace CityPlanner.MapElements;

public class Sight : MapElement
{
    public Sight(MapElement mapElement) : base(mapElement) { }

    public override void AddDependency(Data.GridType gridType, double distance)
    {
        switch (gridType)
        {
            case Data.GridType.Street:

            case Data.GridType.Housing:
            case Data.GridType.Sight:
                Dependency[gridType].Add(distance);
                break;
            case Data.GridType.Subway:
                if (distance <= 1)
                    Dependency[gridType].Add(distance);
                break;
            case Data.GridType.Commercial:
                break;
            case Data.GridType.Industry:
                break;
            case Data.GridType.Blocked:
                break;
            case Data.GridType.Highway:
                break;
            case Data.GridType.Empty:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gridType), gridType, null);
        }
    }

    public override int CalculateScore()
    {
        Score = 0;


        Score += Dependency[Data.GridType.Housing].Count * 50;
        Score += Dependency[Data.GridType.Sight].Count * -1000;
        if (Dependency[Data.GridType.Subway].Any())
            Score += 100;
            

        // Base cost
        Score += 20;

        // Level
        UpdateLevel();

        return Score;
    }

    public override bool IsInRangeOfStreet()
    {
        return Dependency[Data.GridType.Street].Any();
    }

    public override Data.GridType GetGridType()
    {
        return Data.GridType.Sight;
    }

    public override Sight Clone()
    {
        return new Sight(this);
    }
}