// @author: Kevin Kern, Sander Stella

namespace CityPlanner.MapElements;

public class Industry : MapElement
{
    public Industry(MapElement mapElement) : base(mapElement) { }

    public override void AddDependency(Data.GridType gridType, double distance)
    {
        switch (gridType)
        {

            case Data.GridType.Street:
                Dependency[gridType].Add(distance);
                break;
            case Data.GridType.Industry:
                if (distance <= 3.5)
                    Dependency[gridType].Add(distance);
                break;
            case Data.GridType.Housing:
                break;
            case Data.GridType.Commercial:
                break;
            case Data.GridType.Subway:
                break;
            case Data.GridType.Sight:
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
        Score += Dependency[Data.GridType.Industry].Count * 450;

        // Base cost
        Score += 50;
        UpdateLevel();

        return Score;
    }

    protected override void UpdateLevel()
    {
        Level = Score switch
        {
            < 70 => 1,
            < 570 => 2,
            > 570 => 3,
            _ => Level
        };
    }

    public override bool IsInRangeOfStreet()
    {
        return IsValidStreet();
    }

    public override Data.GridType GetGridType()
    {
        return Data.GridType.Industry;
    }
    public override Industry Clone()
    {
        return new Industry(this);
    }
}