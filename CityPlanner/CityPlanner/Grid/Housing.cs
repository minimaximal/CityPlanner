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
                Score += 50;
            }
        }

        Dependency[Data.GridType.Street].Sort();
        if (isInRangeOfStreet())
        {
            // Street in Range
            Score += (int)(5 * (2 * Math.Sin(1.1 * (Dependency[Data.GridType.Street][0]) - 0.6)));
        }
        else
        {
            //no Street in Range
            Score += -9999;
        }
        
        foreach (double Industry in Dependency[Data.GridType.Industry])
        {
            if (Industry <= 3.4)
            {
                Score -= 100;
            }
        }
        
        //base cost
        Score -= 5;

        switch (Score)
        {
            //according Level
            case <= 0:
                Level = 1;
                people = 0;
                break;
            case <= 250:
                Level = 1;
                people = 8;
                break;
            case > 250 and <= 600:
                Level = 2;
                people = 95;
                break;
            case > 600:
                Level = 3;
                people = 200;
                break;
        }

        return Score;
        
    }

    public override bool isInRangeOfStreet()
    {
        return Dependency[Data.GridType.Street].Count() > 0;
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