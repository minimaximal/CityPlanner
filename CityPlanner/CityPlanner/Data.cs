using System;
using CityPlanner.Grid;

public static class Data
{
    //Empty must be the last in the list otherwise Agent.cs:getRandomMove() does not work
    public enum GridType
    {
        Housing,
        Commercial,
        Industry,
        Street,
        Empty
    };

    public static Dictionary<GridType, double> GridTypeMax = new Dictionary<GridType, double>()
    {
        { GridType.Housing, 5 },
        { GridType.Commercial, 9.9 },
        { GridType.Industry, 5.9 },
        { GridType.Street, 3.5 },
        { GridType.Empty, 0 }
    };
}