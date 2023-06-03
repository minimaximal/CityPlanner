using CityPlanner;
using CityPlanner.Grid;

public class API
{
    //start 18:22 -10min
    // end 19:02

    private AppController appctrl;
    private bool newMapFlag;
    private Map currentMap;
    private Byte[,] ByteMap;
    private int Score;
    private int People;
    private int placedBuildings;
    private Dictionary<Data.GridType, int> stats = new Dictionary<Data.GridType, int>();


    // do one time setup on start of application
    // pull map preferences
    public API(int population, int sizeX, int sizeY, int importQuota)
    {

        foreach (Data.GridType gridType in (Data.GridType[])Enum.GetValues(typeof(Data.GridType)))
        {
            stats.Add(gridType, 0);
        }
        ByteMap = new byte[sizeX, sizeY];
        Score = 0;
        People = 0;
        appctrl = new AppController(population, sizeX, sizeY, importQuota);
    }

    public void nextGeneration()
    {
        Map newMap = appctrl.nextGeneration();
        if(currentMap == null&&newMap!=null) { setNewMap(newMap);return; }
        if (newMap.getScore() > currentMap.getScore())
        {
            setNewMap(newMap);
        }
    }

    private void setNewMap(Map newMap)
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
    public Byte[,] getMapToFrontend()
    {
        Score = currentMap.getScore();
        People = currentMap.GetPeople(); 
        foreach (var stat in stats.Keys)
        {
            stats[stat] = 0;
        }
        
        
        for (int x = 0; x < currentMap.SizeX; x++)
        {
            for (int y = 0; y < currentMap.SizeY; y++)
            {
                ByteMap[x, y] = transform(currentMap.GetGridElement(x, y));
            }
        }

        newMapFlag = false;
        return ByteMap;
    }

    public int getSatisfaction()
    {
        return Score;
    }

    //returns average building Level, has to be called after getPlacedBuildings()
    public int getAverageBuildLevel()
    {
        int average = 0;
        foreach (var num in ByteMap)
        {
            int level = Int32.Parse(num.ToString().Substring(2, 2));
            if (level != 5)
            {
                average += level;
            }
        }

        average /= getPlacedBuildings();
        return average;
    }

    public int getPlacedBuildings()
    {
        int buildings = 0;
        foreach (var num in ByteMap)
        {
            if (num != 255)
            {
                buildings++;
            }
        }
        return buildings;
    }

    public int getPopulation()
    {
        return People;
    }


    public byte transform(GridElement input)
    {
        if (input.getScore() < -4500)
        {
          return 0;
        }
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
            case Data.GridType.Empty:
                stats[Data.GridType.Empty]++;
                return 0;
        }

        return 255;
    }
}