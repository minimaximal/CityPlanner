﻿namespace CityPlanner.Grid;

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
                Score += 250;
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
            Score += -9999;
        }
        //base cost
        Score -= 5;
        
        return Score;
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