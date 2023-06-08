//Author: Kevin Kern, Sander Stella, Antoni Paul

using System.Diagnostics;
using CityPlanner.Grid;

namespace CityPlanner;

public class Map : ICloneable
{
    private readonly GridElement[,] _map;
    private int _population;
    private readonly int _targetPopulation;
    private int _score;

    public readonly int SizeX;
    public readonly int SizeY;
    
    private int _populationScore = 0;
    private int _industryRatioScore = 0;
    private int _commercialScore = 0;


    public Map(int x, int y, int targetPopulation)
    {
        _targetPopulation = targetPopulation;
        SizeX = x;
        SizeY = y;
        _map = new GridElement[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                _map[i, j] = new GridElement();
            }
        }
    }


    private static GridElement NewGridElement(Data.GridType gridType, GridElement old)
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

    //calculates Dependencies for the whole Map and every mapelement except for predefined elements and streets
    private void CalculateDependencies()
    {
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                GridElement gridElement = GetGridElement(i, j)!;
                if (gridElement.GetGridType() == Data.GridType.Street
                    || gridElement.GetGridType() == Data.GridType.Blocked
                    || gridElement.GetGridType() == Data.GridType.Highway) continue;
                if (gridElement.IsInRangeOfStreet() )
                {
                    AddDependenciesFor(i, j);
                }
                else 
                {
                    _map[i, j] = new GridElement();
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
                GridType   =  GetGridElement(x, y)!.GetGridType()
            }
        );
    }

    private void AddDependenciesFor(Move move)
    {
        int range = (int)Math.Ceiling(Data.GridTypeMaxRange[move.GridType]);
        for (int x = move.X - range; x < move.X + range; x++)
        {
            for (int y = move.Y - range; y < move.Y + range; y++)
            {
                double distance = Math.Sqrt(Math.Pow(move.X - x, 2) + Math.Pow(move.Y - y, 2));
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
        int globalScore = 0;
        int industryAmount = 0;
        int commercialAmount = 0;
        
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
            }
        }

        //Population Scoring
        int populationDiff = _population - _targetPopulation;
        _populationScore = (int)(-0.05 * populationDiff * populationDiff + 1000);
        globalScore += _populationScore;

        //Import quota
        int industryDiff = industryAmount - Data.OptimalIndustryAmount;
        _industryRatioScore = -(industryDiff * industryDiff + 10) * 7000;
        globalScore += _industryRatioScore;

        //commercial quota
        int commercialDiff = commercialAmount - (_targetPopulation / 550);
        _commercialScore = -(commercialDiff * commercialDiff + 10) * 4000;
        globalScore += _commercialScore;

        _score = globalScore;
        return globalScore;
    }

    public int GetPeople()
    {
        return _population;
    }

    //returns true if move is valid for placing a street
    public bool ValidateStreet(Move move)
    {
        return GetGridElement(move)!.IsValidStreet();
    }
    
    public GridElement? GetGridElement(Move coordinates)
    {
        return GetGridElement(coordinates.X, coordinates.Y);
    }

    public GridElement? GetGridElement(int x, int y)
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
        Map clone = new Map(SizeX, SizeY, _targetPopulation);
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
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



    //for backend testing only
    public void Display()
    {
        Console.Write("------------------------------\n");
        Console.WriteLine("score:" + _score);
        Console.WriteLine("People:" + _population);
        Console.WriteLine("poulation Dif Score:" + _populationScore);
        Console.WriteLine("industryRatioScore:" + _industryRatioScore);
        Console.WriteLine("comertialScore:" + _commercialScore);


        for (int y = 0; y < SizeY; y++)
        {
            for (int x = 0; x < SizeX; x++)
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
                }
            }

            Console.ResetColor();
            Console.Write("\n");
        }
    }
}