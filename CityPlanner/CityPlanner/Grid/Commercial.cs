namespace CityPlanner.Grid;

public class Commercial : GridElement
{
    public Commercial(GridElement gridElement) : base(gridElement)
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
                if (distance <= 4.9)
                    Dependency[gridType].Add(distance);
                break;
            case Data.GridType.Industry:
                if (distance <= 4)
                    Dependency[gridType].Add(distance);
                break;
        }
    }

    public override int CalculateScore()
    {
        Score = 0;
        

        Score += Dependency[Data.GridType.Housing].Count * 2;
        Score += Dependency[Data.GridType.Industry].Count * 500;

        foreach (double commercial in Dependency[Data.GridType.Commercial])
        {
            if (commercial <= 2)
            {
                Score += 700;
            }
            else if (commercial > 3.5)
            {
                Score -= 500;
            }
        }
        
        //base cost
        Score += 15;

        //Level
        UpdateLevel();

        return Score;
    }

    protected override void UpdateLevel()
    {
        Level = Score switch
        {
            < 0 => 1,
            < 200 => 2,
            > 200 => 3,
            _ => Level
        };
    }
    

    public override bool isInRangeOfStreet()
    {
        return IsValidStreet();
    }


    public override Data.GridType GetGridType()
    {
        return Data.GridType.Commercial;
    }

    public override Commercial Clone()
    {
        return new Commercial(this);
    }
}