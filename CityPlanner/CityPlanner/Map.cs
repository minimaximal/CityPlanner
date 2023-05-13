using CityPlanner.Grid;

namespace CityPlanner;

public class Map
{
    private GridElement[,] map;

    public int SizeX { get; }
    public int SizeY { get; }
    public Map(int x, int y)
    {
        SizeX = x;
        SizeY = y;
        map = new GridElement[x, y];
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
        map[move.X, move.Y] = newGridElement(move.GridType, GetGridElement(move));
        //update nearby Gridelements
    }

    public int CalculateScore()
    {
        int globalScore = 0;
        foreach (var gridElement in map)
        {
            globalScore += gridElement.CalculateScore();
        }

        return globalScore;
    }

    public GridElement GetGridElement(Move coordinates)
    {
        return GetGridElement(coordinates.X, coordinates.Y);
    }
    public GridElement GetGridElement(int x , int y)
    {
        return map[x,y];
    }
}