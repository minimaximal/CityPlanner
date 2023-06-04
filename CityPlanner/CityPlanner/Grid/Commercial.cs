namespace CityPlanner.Grid;

public class Commercial : GridElement
{
    public Commercial(GridElement gridElement) : base(gridElement) {}

    
    public override int CalculateScore()
    {
        Score = 0;
        foreach (double housing in Dependency[Data.GridType.Housing])
        {
            if (housing <= 4.9)
            {
                Score += 2;
            }
        }
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
        foreach (double industry in Dependency[Data.GridType.Industry])
        {
            if (industry<= 4)
            {
                Score += 500;
            }
        }
    
        if (isInRangeOfStreet())
        {
            // Street in Range
            Score += 20;
        }
        else
        {
            //no Street in Range
            Score += -9999;
        }
        //base cost
        Score -= 5;
        
        //Level
        switch (Score)
        {
            case < 250:
                Level = 1;
                break;
            case < 700:
                Level = 2;
                break;
            case > 700:
                Level = 3;
                break;

        }
        
        return Score;
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