namespace CityPlanner.Grid;

public class Housing : GridElement
{
    public Housing(GridElement gridElement) : base(gridElement) {}
    public override int CalculateScore()
    {
        return Score;
    }

    public override Data.GridType GetGridType()
    {
        return Data.GridType.Housing;
    }
}