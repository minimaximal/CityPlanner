namespace CityPlanner;

public class Move : IComparable
{
    public Data.GridType GridType;
    public readonly int X;
    public readonly int Y;

    public bool ICH_BIN_EINE_DUMME_LÖSUNG = false;
    public List<(int,int)> ICH_BIN_EINE_DUMME_REFERENCE = new List<(int, int)>();


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

    public double DistanceToCenter()
    {
        double minDistance = 1000;
        double distance = 1000;
        foreach (var (x,y) in ICH_BIN_EINE_DUMME_REFERENCE)
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

        if (ICH_BIN_EINE_DUMME_LÖSUNG)
        {
            return (int) Math.Round( DistanceToCenter()*1000 - comp.DistanceToCenter()*1000);
        }
        
        return IndexNumber() - comp.IndexNumber();
    }
}