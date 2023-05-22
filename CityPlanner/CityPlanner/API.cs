﻿//über den namen darf disgutiert werden

using CityPlanner;
using CityPlanner.Grid;

public class API
{
    //start 18:22 -10min
    // end 19:02

    public Byte[,] map;
    public int Score;
    public int People;
    public Dictionary<Data.GridType, int> stats = new Dictionary<Data.GridType, int>();


    public API(Map inMap)
    {
        // do one time setup on start of alication
        // pull map ans set size 

        foreach (Data.GridType gridType in (Data.GridType[])Enum.GetValues(typeof(Data.GridType)))
        {
            stats.Add(gridType, 0);
        }
        map = new byte[inMap.SizeX, inMap.SizeY];
        Score = 0;
        People = 0;

    }

    // function call looks like : API.ToFrontend(map)
    public void ToFrontend(Map inMap)
    {
        Score = inMap.CalculateScore();// todo vl zwichen speichern stadt fukction call
        People = inMap.GetPeople(); 
        foreach (var stat in stats.Keys)
        {
            stats[stat] = 0;
        }
        
        
        for (int x = 0; x < inMap.SizeX; x++)
        {
            for (int y = 0; y < inMap.SizeY; y++)
            {
                map[x, y] = transform(inMap.GetGridElement(x, y));
            }
        }
    }


    public Byte transform(GridElement input)
    {
        switch (input.GetGridType())
        {
            case Data.GridType.Housing:
                stats[Data.GridType.Housing]++;
                switch (input.GetLevel())
                {
                    case 1:
                        return 11;
                    case 2:
                        return 12;
                    case 3:
                        return 13;
                }
                break;
            case Data.GridType.Commercial:
                stats[Data.GridType.Commercial]++;

                switch (input.GetLevel())
                {
                    case 1:
                        return 21;
                    case 2:
                        return 22;
                    case 3:
                        return 23;
                }
                break;
            case Data.GridType.Industry:
                stats[Data.GridType.Industry]++;
                switch (input.GetLevel())
                {
                    case 1:
                        return 31;
                    case 2:
                        return 32;
                    case 3:
                        return 33;
                }
                break;
            case Data.GridType.Street:
                stats[Data.GridType.Street]++;
                return 1;
            case Data.GridType.Empty:
                stats[Data.GridType.Empty]++;
                return 0;
        }

        return 255;
    }
}