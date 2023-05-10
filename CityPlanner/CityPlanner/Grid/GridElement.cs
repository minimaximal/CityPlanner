namespace CityPlanner.Grid;

public abstract class GridElement
{
    private int Score;
    private int Level = 1;



    public void addScore(Data.GridType gridType)
    {
        return 1;
    }

    private void updateEnvironment()
    {

    }
    public int getScore()
    {
        return Score;
    }
    
}