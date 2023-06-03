﻿namespace CityPlanner.Grid;

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
            if (industry <= 3.5)
            {
                Score += 250;
            }
        }
        if (IsValidStreet())
        {
            // Street in Range
            Score += 70;
        }
        else
        {
            //no Street in Range
            Score += -9999;
        }
        
        //base cost
        Score -= 20;

        return Score;
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