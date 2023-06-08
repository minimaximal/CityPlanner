//Author: Sander Stella

namespace CityPlanner;

public class Move : IComparable
{
    public Data.GridType GridType;
    public readonly int X;
    public readonly int Y;
    public Move(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Move(Move oldMove)
    {
        X = oldMove.X;
        Y = oldMove.Y;
        GridType = oldMove.GridType;
    }

    //each move gets its own number according to the coordinates in order to identify holes in Agent.cs
    public int IndexNumber()
    {
        return Y * (Data.SizeX)+X ;
    }

    public double DistanceToCenter()
    {
        double minDistance = 1000;
        foreach (var (x,y) in Data.InitialStreets)
        {
            int a = X - x;
            int b = Y - y;
            double distance = Math.Sqrt(a * a + b * b);
            if (distance < minDistance)
                 minDistance = distance;    

        }

        return minDistance;
    }
    
    public int CompareTo(object? obj)
    {
        Move comp = (Move)obj!;

        return IndexNumber() - comp.IndexNumber();
    }
}