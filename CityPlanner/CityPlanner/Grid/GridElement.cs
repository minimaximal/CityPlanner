namespace CityPlanner.Grid;

public abstract class GridElement
{
    private int Score;
    private int Level = 1;



    public abstract bool addScore(Data.GridType gridType, int distance)
    {
        return 1;
    }

    public int getScore()
    {
        return Score;
    }
    
}