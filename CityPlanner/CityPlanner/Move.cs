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

    public int indexNumber()
    {
        return X * (Data.SizeY) + Y;
    }

    public int CompareTo(object? obj)
    {
        Move comp = (Move)obj!;
        return indexNumber() - comp.indexNumber();
    }
}