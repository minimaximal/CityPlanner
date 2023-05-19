namespace CityPlanner.Grid;

public class Industry : GridElement
{
    public Industry(GridElement gridElement) : base(gridElement)
    {
    }

    public override int CalculateScore()
    {
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
        foreach (double Industry in Dependency[Data.GridType.Industry])
        {
            if (Industry <= 2.5)
            {
                Score += 10;
            }
        }

        Dependency[Data.GridType.Street].Sort();

        if ( Dependency[Data.GridType.Street].Count()>0  && Dependency[Data.GridType.Street][0] > 1.0)
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
        return Data.GridType.Industry;
    }
}