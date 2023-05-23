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

    public int CompareTo(object? obj)
    {
        Move comp = (Move)obj!;
        return IndexNumber() - comp.IndexNumber();
    }
}