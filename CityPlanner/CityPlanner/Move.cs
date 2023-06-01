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

    public int IndexNumber()
    {
        return Y * (Data.SizeX)+X ;
    }

    public double DistanceToCenter(List<(int, int)>? initialStreets)
    {
        double minDistance = 1000;
        double distance = 1000;
        foreach (var (x,y) in initialStreets)
        {
             distance = Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2));
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