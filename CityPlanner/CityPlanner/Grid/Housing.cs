namespace CityPlanner.Grid;

public class Housing : GridElement
{
    private int people;

    public Housing(GridElement gridElement) : base(gridElement)
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
        */

        foreach (double Commercial in Dependency[Data.GridType.Commercial])
        {
            if (Commercial <= 4.9)
            {
                Score += 5;
            }
        }

        Dependency[Data.GridType.Street].Sort();
        if (Dependency[Data.GridType.Street].Count() > 0)
        {
            // Street in Range
            Score += (int)(5 * (2 * Math.Sin(1.1 * (Dependency[Data.GridType.Street][0]) - 0.6)));
        }
        else
        {
            //no Street in Range
            Score  = -500;
        }
        
        foreach (double Industry in Dependency[Data.GridType.Industry])
        {
            if (Industry <= 4.9)
            {
                Score -= 15;
            }
        }
        
        //base cost
        Score -= 5;

        //according Level
        if (Score <= 0)
        {
            Level = 1;
            people = 0;
        }
        if (Score <= 25)
        {
            Level = 1;
            people = 8;
        }
        else if (Score is > 25 and <= 50)
        {
            Level = 2;
            people = 95;
        }
        else if (Score > 50)
        {
            Level = 3;
            people = 200;
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
    public override Housing Clone()
    {
        return new Housing(this);
    }
}