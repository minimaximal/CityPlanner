using CityPlanner.Grid;

namespace CityPlanner;

public class Map : ICloneable
{
    private GridElement[,] map;
    private const int Range = 5;
    private int globalPeople = 0;

    public int SizeX { get; }
    public int SizeY { get; }

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

    GridElement newGridElement(Data.GridType gridType, GridElement old)
    {
        switch (gridType)
        {
            case Data.GridType.Housing:
                return new Housing(old);
            case Data.GridType.Industry:
                return new Industry(old);
            case Data.GridType.Street:
                return new Street(old);
            case Data.GridType.Commercial:
                return new Commercial(old);
            case Data.GridType.Empty:
                return old;
        }

        throw new Exception("In dem Switch case sollten alle GridTypes abgedeckt sein");
    }

    public void AddMove(Move move)
    {
        if (map[move.X, move.Y].GetGridType() != Data.GridType.Empty)
        {
            Console.Write("Fuck");
        }

        map[move.X, move.Y] = newGridElement(move.GridType, GetGridElement(move));
        for (int x = move.X - 5; x < move.X + 5; x++)
        {
            for (int y = move.Y - 5; y < move.Y + 5; y++)
            {
                double distance = Math.Sqrt(Math.Pow(move.X - x, 2) + Math.Pow(move.Y - y, 2));
                if (distance <= Range)
                {
                    GetGridElement(x, y).AddDependency(move.GridType, distance);
                }
            }
        }
    }

    public int CalculateScore()
    {
        globalPeople = 0;
        int globalScore = 0;
        foreach (var gridElement in map)
        {
            globalScore += gridElement.CalculateScore();

            if (gridElement.GetGridType() == Data.GridType.Housing)
            {
                globalPeople += ((Housing)gridElement).GetPeople();
            }
        }

        return globalScore;
    }

    public int GetPeople()
    {
        return globalPeople;
    }

    public GridElement GetGridElement(Move coordinates)
    {
        return GetGridElement(coordinates.X, coordinates.Y);
    }

    public GridElement GetGridElement(int x, int y)
    {
        if (x > 0 && x < SizeX && y > 0 && y < SizeY)
        {
            return map[x, y];
        }
        else
        {
            return new GridElement();
        }
    }

    public bool validateStreet(Move move)
    {
        return GetGridElement(move.X - 1, move.Y).GetGridType() == Data.GridType.Street |
               GetGridElement(move.X, move.Y - 1).GetGridType() == Data.GridType.Street |
               GetGridElement(move.X + 1, move.Y).GetGridType() == Data.GridType.Street |
               GetGridElement(move.X, move.Y + 1).GetGridType() == Data.GridType.Street;
    }

    public object Clone()
    {
        return new Map(this.SizeX, this.SizeY)
        {
            globalPeople = this.globalPeople,
            map = (GridElement[,])map.Clone()
        };
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