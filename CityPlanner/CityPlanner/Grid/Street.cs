namespace CityPlanner.Grid;

public class Street : GridElement
{
    
    public override int CalculateScore()
    {
        Score = -10;
        return Score;
    }

    public override Data.GridType GetGridType()
    {
        return Data.GridType.Street;
    }
    
}