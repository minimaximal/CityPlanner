// @author: Kevin Kern, Sander Stella

using CityPlanner.MapElements;

namespace CityPlanner;

public class Api
{

    private readonly AppController _appController;
    private bool _newMapFlag;
    private Map _currentMap;
    private readonly byte[,] _byteMap;
    private int _score;
    private int _people;
    private readonly Dictionary<Data.GridType, int> _stats = new();


    // Do one time setup on start of application
    public Api(int population, byte[,] byteMap, int importQuota, int numberAgents, double mutationChance)
    {

        foreach (var gridType in (Data.GridType[])Enum.GetValues(typeof(Data.GridType)))
        {
            _stats.Add(gridType, 0);
        }

        Data.MutationChance = mutationChance;

        _byteMap = byteMap;
        _score = 0;
        _people = 0;
        _currentMap = TransformByteArrayToObjectArray(byteMap, population);
        _appController = new AppController(_currentMap, importQuota, numberAgents);
    }

    public void NextGeneration()
    {
        var newMap = _appController.NextGeneration();
        if (GetGeneration() == 1) { SetNewMap(newMap); return; }
        if (newMap.GetScore() > _currentMap.GetScore())
        {
            SetNewMap(newMap);
        }
    }

    private void SetNewMap(Map newMap)
    {
        _currentMap = newMap;
        _newMapFlag = true;
    }

    public bool ExistsNewMap()
    {
        return _newMapFlag;
    }

    // Returns Map as ByteMap for Frontend in order to be cheaper for further processing
    public byte[,] GetMapToFrontend()
    {
        _score = _currentMap.GetScore();
        _people = _currentMap.GetPeople();
        foreach (var stat in _stats.Keys)
        {
            _stats[stat] = 0;
        }


        for (var x = 0; x < _currentMap.SizeX; x++)
        {
            for (var y = 0; y < _currentMap.SizeY; y++)
            {
                _byteMap[x, y] = TransformMapElementToByte(_currentMap.GetGridElement(x, y)!, new ArgumentOutOfRangeException());
            }
        }

        _newMapFlag = false;
        return _byteMap;
    }

    public int GetSatisfaction()
    {
        return (_score);
    }

    // Returns average building Level, has to be called after getPlacedBuildings()
    public float GetAverageBuildLevel()
    {
        float average = 0;
        var buildingAmount = 0;
        foreach (var num in _byteMap)
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

    public int GetPlacedBuildingsAmount()
    {
        return _byteMap.Cast<byte>().Count(num => num != 0);
    }

    public int GetPopulation()
    {
        return _people;
    }

    public int GetGeneration()
    {
        return _appController.GetGeneration();
    }

    // Transforms one MapElement from Object to Bytecode
    // from backend to frontend (map view)
    private byte TransformMapElementToByte(MapElement input, Exception argumentOutOfRangeException)
    {
        switch (input.GetGridType())
        {
            case Data.GridType.Housing:
                _stats[Data.GridType.Housing]++;
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
                _stats[Data.GridType.Commercial]++;

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
                _stats[Data.GridType.Industry]++;
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
                _stats[Data.GridType.Street]++;
                return 31;
            case Data.GridType.Highway:
                _stats[Data.GridType.Highway]++;
                return 32;
            case Data.GridType.Subway:
                _stats[Data.GridType.Subway]++;
                return 41;
            case Data.GridType.Sight:
                _stats[Data.GridType.Subway]++;
                return 51;
            case Data.GridType.Blocked:
                _stats[Data.GridType.Blocked]++;
                return 21;
            case Data.GridType.Empty:
                _stats[Data.GridType.Empty]++;
                return 0;
            default:
                throw argumentOutOfRangeException;
        }

        return 255;
    }

    // from frontend to backend (map editor)
    private static Map TransformByteArrayToObjectArray(byte[,] byteMap, int population)
    {
        var sizeX = byteMap.GetLength(0);
        var sizeY = byteMap.GetLength(1);
        var map = new Map(sizeX, sizeY, population);
        var hasStreet = false;
        for (var x = 0; x < sizeX; x++)
        {
            for (var y = 0; y < sizeY; y++)
            {
                switch (byteMap[x, y])
                {
                    case 11:
                        {
                            var move = new Move(x, y)
                            {
                                GridType = Data.GridType.Street
                            };
                            map.AddMove(move);
                            hasStreet = true;
                            break;
                        }
                    case 21:
                        {
                            var move = new Move(x, y)
                            {
                                GridType = Data.GridType.Blocked
                            };
                            map.AddMove(move);
                            break;
                        }
                    case 32:
                        {
                            var move = new Move(x, y)
                            {
                                GridType = Data.GridType.Highway
                            };
                            map.AddMove(move);
                            break;
                        }
                }
            }
        }

        if (hasStreet) return map;
        {
            var move = new Move(sizeX / 2, sizeY / 2)
            {
                GridType = Data.GridType.Street
            };
            map.AddMove(move);
        }

        return map;
    }
}