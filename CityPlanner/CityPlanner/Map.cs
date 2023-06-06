using System.Diagnostics;
using CityPlanner.Grid;

namespace CityPlanner;

public class Map : ICloneable
{
    private GridElement[,] map;
    private int _population;
    private readonly int _targetPopulation;
    private int Score;

    public readonly int SizeX;
    public readonly int SizeY;
    
    //debug helpers
    private int poulationScore = 0;
    private int industryRatioScore = 0;
    private int comercialScore = 0;


    public Map(int x, int y, int targetPopulation)
    {
        _targetPopulation = targetPopulation;
        SizeX = x;
        SizeY = y;
        map = new GridElement[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                map[i, j] = new GridElement();
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
            Data.GridType.Empty => old,
            _ => throw new Exception("This Switch case must be exhaustive!")
        };
    }

    public void CalculateDependencies()
    {
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                GridElement gridElement = GetGridElement(i, j)!;
                if (gridElement.GetGridType() == Data.GridType.Street) continue;
                if (gridElement.isInRangeOfStreet() )
                {
                    AddDependenciesFor(i, j);
                }
                else 
                {
                    map[i, j] = new GridElement();
                }
            }
        }
    }

    public void AddMove(Move move)
    {
        map[move.X, move.Y] = NewGridElement(move.GridType, GetGridElement(move)!);
        if (move.GridType == Data.GridType.Street)
        {
           AddDependenciesFor(move);
        }
    }


    private void AddDependenciesFor(int x, int y)
    {
        AddDependenciesFor(new Move(x, y)
            {
                GridType   =  GetGridElement(x, y).GetGridType()
            }
        );
    }

    private void AddDependenciesFor(Move move)
    {
        
        int range = (int)Math.Ceiling(Data.GridTypeMax[move.GridType]);
        for (int x = move.X - range; x < move.X + range; x++)
        {
            for (int y = move.Y - range; y < move.Y + range; y++)
            {
                double distance = Math.Sqrt(Math.Pow(move.X - x, 2) + Math.Pow(move.Y - y, 2));
                if (distance <= Data.GridTypeMax[move.GridType] && !(move.X == x && move.Y == y))
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

        foreach (var gridElement in map)
        {
            globalScore += gridElement.CalculateScore();
            switch (gridElement.GetGridType())
            {
                case Data.GridType.Housing:
                    _population += ((Housing)gridElement).GetPeople();
                    break;
                case Data.GridType.Industry:
                    if (gridElement.getScore() > -8000)
                    {
                        industryAmount++;
                    }
                    break;
                case Data.GridType.Commercial:
                    if (gridElement.getScore() > -8000)
                    {
                        commercialAmount++;
                    }
                    break;
            }
        }

        //Population Scoring
        int populationDif = _population - _targetPopulation;
        poulationScore = (int)(-0.05 * populationDif * populationDif + 1000);
        globalScore += poulationScore;

        //Importquota
        int industryDiff = industryAmount - Data.optimalIndustryAmount;
        industryRatioScore = -(industryDiff * industryDiff + 10) * 6000;
        globalScore += industryRatioScore;

        //commercialquota
        int commercialDiff = commercialAmount - (_targetPopulation / 550);
        comercialScore = -(commercialDiff * commercialDiff + 10) * 4000;
        globalScore += comercialScore;

        Score = globalScore;
        return globalScore;
    }

    public int GetPeople()
    {
        return _population;
    }

    public GridElement? GetGridElement(Move coordinates)
    {
        return GetGridElement(coordinates.X, coordinates.Y);
    }

    public GridElement? GetGridElement(int x, int y)
    {
        if (x >= 0 && x < SizeX && y >= 0 && y < SizeY)
        {
            return map[x, y];
        }
        else
        {
            return null;
        }
    }

    public bool ValidateStreet(Move move)
    {
        return GetGridElement(move)!.IsValidStreet();
    }

    public object Clone()
    {
        Map clone = new Map(SizeX, SizeY, _targetPopulation);
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                //todo this drops the type and does not work  
                //not sure on this one write a test for that
                clone.map[x, y] = map[x, y].Clone();

                //  NewGridElement(map[x,y].GetGridType(), map[x, y]);
            }
        }

        clone._population = _population;
        return clone;
    }

    public int GetScore()
    {
        return Score;
    }



    //for backend testing only
    public void Display()
    {
        Console.Write("------------------------------\n");
        Console.WriteLine("score:" + Score);
        Console.WriteLine("People:" + _population);
        Console.WriteLine("poulation Dif Score:" + poulationScore);
        Console.WriteLine("industryRatioScore:" + industryRatioScore);
        Console.WriteLine("comertialScore:" + comercialScore);


        for (int y = 0; y < SizeY; y++)
        {
            for (int x = 0; x < SizeX; x++)
            {
                /*if (map[x, y].getScore() < -8888)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(".");
                    continue;
                }*/

                switch (map[x, y].GetGridType())
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
                    case Data.GridType.Street:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write("+");

                        break;
                    case Data.GridType.Empty:
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("_");
                        break;
                }
            }

            Console.ResetColor();
            /*
            Console.Write("\t\t");

            for (int x = 0; x < SizeX; x++)
            {
                Console.Write(map[x, y].getScore() +"|");
            }
*/
            Console.Write("\n");
        }
    }
}