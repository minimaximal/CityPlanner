namespace CityPlanner.Grid;

public class Industry : GridElement
{
    public Industry(GridElement gridElement) : base(gridElement)
    {
    }

    public override int CalculateScore()
    {
        Score = 0;

        /*
        foreach (double Housing in Dependency[Data.GridType.Housing])
        {
            //who knows...
        }
        foreach (double Commercial in Dependency[Data.GridType.Commercial])
        {
            //who knows...
        }
        */
        foreach (double industry in Dependency[Data.GridType.Industry])
        {
            if (industry <= 2.5)
            {
                Score += 10;
            }
        }
        if (IsValidStreet())
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
        Score -= 50;

        return Score;
    }

    public override Data.GridType GetGridType()
    {
        return Data.GridType.Industry;
    }
}