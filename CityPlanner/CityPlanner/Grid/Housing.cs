namespace CityPlanner.Grid;

public class Housing : GridElement
{
    
    public override int CalculateScore()
    {
        return Score;
    }

    public override Data.GridType GetGridType()
    {
        return Data.GridType.Housing;
    }
}