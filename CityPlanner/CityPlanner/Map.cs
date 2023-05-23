using CityPlanner.Grid;

namespace CityPlanner;

public class Map : ICloneable
{
    private GridElement[,] map;
    private int _globalPeople;

    public readonly int SizeX;
    public readonly int SizeY;

    public Map(int x, int y)
    {
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

    public void AddMove(Move move)
    {
        
        if (GetGridElement(move) ==null 
            || map[move.X, move.Y].GetGridType() != Data.GridType.Empty)
        {
          // Console.Write("Fuck");
          return;
        }

        map[move.X, move.Y] = NewGridElement(move.GridType, GetGridElement(move)!);
        int range = (int)Math.Ceiling( Data.GridTypeMax[move.GridType]);
        for (int x = move.X - range; x < move.X + range; x++)
        {
            for (int y = move.Y - range; y < move.Y + range; y++)
            {
                double distance = Math.Sqrt(Math.Pow(move.X - x, 2) + Math.Pow(move.Y - y, 2));
                if (distance <= Data.GridTypeMax[move.GridType] && !(move.X ==x &&move.Y ==y))
                {
                    GetGridElement(x, y)?.AddDependency(move.GridType, distance);
                }
            }
        }
    }

    public int CalculateScore()
    {
        _globalPeople = 0;
        int globalScore = 0;
        foreach (var gridElement in map)
        {
            globalScore += gridElement.CalculateScore();

            if (gridElement.GetGridType() == Data.GridType.Housing)
            {
                _globalPeople += ((Housing)gridElement).GetPeople();
            }
        }

        return globalScore;
    }

    public int GetPeople()
    {
        return _globalPeople;
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
        return  GetGridElement(move)!.IsValidStreet();
    }

    public object Clone()
    {
        Map clone = new Map(SizeX, SizeY);
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
              //todo this drops the type and does not work  
                clone.map[x, y] = map[x, y].Clone();

              //  NewGridElement(map[x,y].GetGridType(), map[x, y]);
            }
        }
        clone._globalPeople = _globalPeople;
        return clone;
    }


    public void NewDisplay()
    {            Console.Write("------------------------------\n");

        for (int y = 0; y< SizeY; y++)
        {
            for (int x = 0; x < SizeX; x++)
            {
                switch (map[x,y].GetGridType())
                {
                    case Data.GridType.Commercial:
                        Console.Write("C");

                        break;
                    case Data.GridType.Industry:
                        Console.Write("I");

                        break;
                    case Data.GridType.Housing:
                        Console.Write("H");

                        break;
                    case Data.GridType.Street:
                        Console.Write("+");

                        break;
                    case Data.GridType.Empty:
                        Console.Write("_");
                        break;
                }
            }
            /*
            Console.Write("\t\t");

            for (int x = 0; x < SizeX; x++)
            {
                Console.Write(map[x, y].getSore() +"|");
            }
*/
            Console.Write("\n");

        }
    }
    
    //for backend testing only
    public void Display()
    {
        for (int i = 0; i < SizeY*3; i++)
        {
            Console.Write("-");
           
        }
        Console.Write("\n");
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                Console.Write("|");

                if (map[i, j] is Housing)
                {
                    Console.Write("H");
                    Console.Write(map[i, j].GetLevel());
                }
                else if (map[i, j] is Commercial)
                {
                    Console.Write("C");
                    Console.Write(map[i, j].GetLevel());
                }
                else if (map[i, j] is Industry)
                {
                    Console.Write("I");
                    Console.Write(map[i, j].GetLevel());
                }
                else if (map[i, j] is Street)
                {
                    Console.Write("S");
                    Console.Write(map[i, j].GetLevel());
                }
                else if (map[i, j].GetGridType() == Data.GridType.Empty)
                {
                    Console.Write("_");
                    Console.Write("_");
                }
                else
                {
                    Console.Write("X");
                    Console.Write("X");
                }
            }

            Console.Write("|\n");
            for (int x = 0; x < SizeY*3; x++)
            {
                Console.Write("-");
            }

            Console.Write("\n");
        }
    }
}