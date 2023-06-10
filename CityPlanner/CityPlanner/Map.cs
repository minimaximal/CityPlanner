// @author: Kevin Kern, Sander Stella, Antoni Paul

using CityPlanner.MapElements;

namespace CityPlanner;

public class Map : ICloneable
{
    private readonly MapElement[,] _map;
    private int _population;
    private readonly int _targetPopulation;
    private int _score;

    public readonly int SizeX;
    public readonly int SizeY;

    private int _populationScore;
    private int _industryRatioScore;
    private int _commercialScore;


    public Map(int x, int y, int targetPopulation)
    {
        _targetPopulation = targetPopulation;
        SizeX = x;
        SizeY = y;
        _map = new MapElement[x, y];
        for (var i = 0; i < x; i++)
        {
            for (var j = 0; j < y; j++)
            {
                _map[i, j] = new MapElement();
            }
        }
    }


    private static MapElement NewGridElement(Data.GridType gridType, MapElement old)
    {
        return gridType switch
        {
            Data.GridType.Housing => new Housing(old),
            Data.GridType.Industry => new Industry(old),
            Data.GridType.Street => new Street(old),
            Data.GridType.Commercial => new Commercial(old),
            Data.GridType.Subway => new Subway(old),
            Data.GridType.Sight => new Sight(old),
            Data.GridType.Empty => old,
            Data.GridType.Blocked => new Blocked(old),
            Data.GridType.Highway => new Highway(old),
            _ => throw new Exception("This Switch case must be exhaustive!")
        };
    }

    // Calculates Dependencies for the whole Map and every mapElement except for predefined elements and streets
    private void CalculateDependencies()
    {
        for (var i = 0; i < SizeX; i++)
        {
            for (var j = 0; j < SizeY; j++)
            {
                var mapElement = GetGridElement(i, j)!;
                if (mapElement.GetGridType() == Data.GridType.Street
                    || mapElement.GetGridType() == Data.GridType.Blocked
                    || mapElement.GetGridType() == Data.GridType.Highway) continue;
                if (mapElement.IsInRangeOfStreet())
                {
                    AddDependenciesFor(i, j);
                }
                else
                {
                    _map[i, j] = new MapElement();
                }
            }
        }
    }

    public void AddMove(Move move)
    {
        _map[move.X, move.Y] = NewGridElement(move.GridType, GetGridElement(move)!);
        if (move.GridType == Data.GridType.Street)
        {
            AddDependenciesFor(move);
        }
    }


    private void AddDependenciesFor(int x, int y)
    {
        AddDependenciesFor(new Move(x, y)
            {
                GridType = GetGridElement(x, y)!.GetGridType()
            }
        );
    }

    private void AddDependenciesFor(Move move)
    {
        var range = (int)Math.Ceiling(Data.GridTypeMaxRange[move.GridType]);
        for (var x = move.X - range; x < move.X + range; x++)
        {
            for (var y = move.Y - range; y < move.Y + range; y++)
            {
                var distance = Math.Sqrt(Math.Pow(move.X - x, 2) + Math.Pow(move.Y - y, 2));
                if (distance <= Data.GridTypeMaxRange[move.GridType] && !(move.X == x && move.Y == y))
                {
                    GetGridElement(x, y)?.AddDependency(move.GridType, distance);
                }
            }
        }
    }


    public int CalculateScore()
    {
        _population = 0;
        var globalScore = 0;
        var industryAmount = 0;
        var commercialAmount = 0;

        CalculateDependencies();

        foreach (var gridElement in _map)
        {
            globalScore += gridElement.CalculateScore();
            switch (gridElement.GetGridType())
            {
                case Data.GridType.Housing:
                    _population += ((Housing)gridElement).GetPeople();
                    break;
                case Data.GridType.Industry:
                    industryAmount++;
                    break;
                case Data.GridType.Commercial:
                    commercialAmount++;
                    break;
                case Data.GridType.Subway:
                    break;
                case Data.GridType.Sight:
                    break;
                case Data.GridType.Street:
                    break;
                case Data.GridType.Blocked:
                    break;
                case Data.GridType.Highway:
                    break;
                case Data.GridType.Empty:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //Population Scoring
        const int factor = 100000;
        _populationScore = -2 * factor * 3 / _targetPopulation * Math.Abs(_population - _targetPopulation) + factor * 3;
        globalScore += _populationScore;

        //Import quota
        if (Data.OptimalIndustryAmount != 0)
        {
            _industryRatioScore =
                -2 * factor / Data.OptimalIndustryAmount * Math.Abs(industryAmount - Data.OptimalIndustryAmount) +
                factor;
            globalScore += _industryRatioScore;
        }

        //commercial quota
        var targetCommercial = (_targetPopulation / 550);
        _commercialScore = -2 * factor / targetCommercial * Math.Abs(commercialAmount - targetCommercial) + factor;
        globalScore += _commercialScore;

        _score = globalScore;
        return globalScore;
    }

    public int GetPeople()
    {
        return _population;
    }

    // Returns true if move is valid for placing a street
    public bool ValidateStreet(Move move)
    {
        return GetGridElement(move)!.IsValidStreet();
    }

    public MapElement? GetGridElement(Move coordinates)
    {
        return GetGridElement(coordinates.X, coordinates.Y);
    }

    public MapElement? GetGridElement(int x, int y)
    {
        if (x >= 0 && x < SizeX && y >= 0 && y < SizeY)
        {
            return _map[x, y];
        }
        else
        {
            return null;
        }
    }

    public object Clone()
    {
        var clone = new Map(SizeX, SizeY, _targetPopulation);
        for (var x = 0; x < SizeX; x++)
        {
            for (var y = 0; y < SizeY; y++)
            {
                clone._map[x, y] = _map[x, y].Clone();
            }
        }

        clone._population = _population;
        return clone;
    }

    public int GetScore()
    {
        return _score;
    }
    public int GetTargetPopulation()
    {
        return _score;
    }



    // For backend testing only
    public void Display()
    {
        Console.Write("------------------------------\n");
        Console.WriteLine("score:" + _score);
        Console.WriteLine("People:" + _population);
        Console.WriteLine("Population Dif Score:" + _populationScore);
        Console.WriteLine("industryRatioScore:" + _industryRatioScore);
        Console.WriteLine("CommercialScore:" + _commercialScore);


        for (var y = 0; y < SizeY; y++)
        {
            for (var x = 0; x < SizeX; x++)
            {
                switch (_map[x, y].GetGridType())
                {
                    case Data.GridType.Commercial:
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write("C");

                        break;
                    case Data.GridType.Industry:
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write("I");

                        break;
                    case Data.GridType.Housing:
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write("H");

                        break;
                    case Data.GridType.Subway:
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        Console.Write("S");

                        break;
                    case Data.GridType.Sight:
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.Write("*");

                        break;
                    case Data.GridType.Street:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write("+");

                        break;
                    case Data.GridType.Blocked:
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.Write("#");
                        break;
                    case Data.GridType.Highway:
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.Write("$");
                        break;
                    case Data.GridType.Empty:
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("_");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Console.ResetColor();
            Console.Write("\n");
        }
    }
}