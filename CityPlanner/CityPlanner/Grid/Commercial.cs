namespace CityPlanner.Grid;

public class Commercial : GridElement
{
    

    public override int CalculateScore()
    {
        return Score;
    }

    public override Data.GridType GetGridType()
    {
        return Data.GridType.Commercial;
    }
    
}