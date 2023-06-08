using System.Diagnostics;
using CityPlanner;
using CityPlanner.Grid;

public class API
{
    //start 18:22 -10min
    // end 19:02
    
    private AppController appctrl;
    private bool newMapFlag;
    private Map currentMap;
    private byte[,] ByteMap;
    private int Score;
    private int People;
    private int placedBuildings;
    private Dictionary<Data.GridType, int> stats = new Dictionary<Data.GridType, int>();


    // do one time setup on start of application
    // pull map preferences
    public API(int population, byte[,] byteMap, int importQuota)
    {

        foreach (Data.GridType gridType in (Data.GridType[])Enum.GetValues(typeof(Data.GridType)))
        {
            stats.Add(gridType, 0);
        }

        ByteMap = byteMap;
        Score = 0;
        People = 0;
        currentMap = TransformByteArrayToObjectArray(byteMap, population);
        appctrl = new AppController(population, currentMap, importQuota);
    }

    public void NextGeneration()
    {
        Map newMap = appctrl.NextGeneration();
        if(GetGeneration() == 1&&newMap!=null) { SetNewMap(newMap);return; }
        if (newMap.GetScore() > currentMap.GetScore())
        {
            SetNewMap(newMap);
        }
    }

    private void SetNewMap(Map newMap)
    {
        currentMap = newMap;
        newMapFlag = true;
        //getMapToFrontend();
    }

    public bool existsNewMap()
    {
        return newMapFlag;
    }

    // function call looks like : API.ToFrontend(map)
    public byte[,] GetMapToFrontend()
    {
        Score = currentMap.GetScore();
        People = currentMap.GetPeople(); 
        foreach (var stat in stats.Keys)
        {
            stats[stat] = 0;
        }
        
        
        for (var x = 0; x < currentMap.SizeX; x++)
        {
            for (var y = 0; y < currentMap.SizeY; y++)
            {
                ByteMap[x, y] = Transform(currentMap.GetGridElement(x, y));
            }
        }

        newMapFlag = false;
        return ByteMap;
    }

    public int GetSatisfaction()
    {
        return (Score/1000 + 10000);
    }

    //returns average building Level, has to be called after getPlacedBuildings()
    public float GetAverageBuildLevel()
    {
        float average = 0;
        var buildingAmount = 0;
        foreach (var num in ByteMap)
        {
            if (num <= 100) continue;
            var level = int.Parse(num.ToString().Substring(2, 1));
            if (level == 5) continue;
            average += level;
            buildingAmount++;
        }

        average /= buildingAmount;
        return average;
    }

    public int getPlacedBuildings()
    {
        return ByteMap.Cast<byte>().Count(num => num != 0);
    }

    public int GetPopulation()
    {
        return People;
    }

    public int GetGeneration()
    {
        return appctrl.GetGeneration();
    }


    public byte Transform(GridElement input)
    {
        switch (input.GetGridType())
        {
            case Data.GridType.Housing:
                stats[Data.GridType.Housing]++;
                switch (input.GetLevel())
                {
                    case 1:
                        return 111;
                    case 2:
                        return 112;
                    case 3:
                        return 113;
                }
                break;
            case Data.GridType.Commercial:
                stats[Data.GridType.Commercial]++;

                switch (input.GetLevel())
                {
                    case 1:
                        return 121;
                    case 2:
                        return 122;
                    case 3:
                        return 123;
                }
                break;
            case Data.GridType.Industry:
                stats[Data.GridType.Industry]++;
                switch (input.GetLevel())
                {
                    case 1:
                        return 131;
                    case 2:
                        return 132;
                    case 3:
                        return 133;
                }
                break;
            case Data.GridType.Street:
                stats[Data.GridType.Street]++;
                return 31;
            case Data.GridType.Subway:
                stats[Data.GridType.Subway]++;
                return 41;
            case Data.GridType.Sight:
                stats[Data.GridType.Subway]++;
                return 51;
            case Data.GridType.Blocked:
                stats[Data.GridType.Blocked]++;
                return 21;
            case Data.GridType.Empty:
                stats[Data.GridType.Empty]++;
                return 0;
        }

        return 255;
    }

    private static Map TransformByteArrayToObjectArray(byte[,] byteMap, int population)
    {
        var SizeX = byteMap.GetLength(0);
        var SizeY = byteMap.GetLength(1);
        var map = new Map(SizeX, SizeY, population);
        var hasStreet = false;
        for (var x = 0; x < SizeX; x++)
        {
            for (var y = 0; y < SizeY; y++)
            {
                if (byteMap[x,y] == 11)
                {
                    var move = new Move(x,y)
                    {
                        GridType = Data.GridType.Street
                    };
                    map.AddMove(move);
                    hasStreet = true;
                } else if (byteMap[x,y] == 21)
                {
                    var move = new Move(x,y)
                    {
                        GridType = Data.GridType.Blocked
                    };
                    map.AddMove(move);
                }
            }
        }

        if (hasStreet) return map;
        {
            var move = new Move(SizeX/2,SizeY/2)
            {
                GridType = Data.GridType.Street
            };
            map.AddMove(move);
        }
        
        return map;
    }
}