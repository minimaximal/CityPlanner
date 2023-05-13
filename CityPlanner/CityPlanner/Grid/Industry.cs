﻿namespace CityPlanner.Grid;

public class Industry : GridElement
{
    public Industry(GridElement gridElement) : base(gridElement) {}

    public override int CalculateScore()
    {
        return Score;
    }

    public override Data.GridType GetGridType()
    {
        return Data.GridType.Industry;
    }
    
}