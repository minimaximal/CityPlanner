namespace CityPlanner.Grid;

public class Housing : GridElement
{
    private int people;
    public Housing(GridElement gridElement) : base(gridElement) {}
    public override int CalculateScore()
    {
        //berechne Score
        if (Score <= 25)
        {
            Level = 1;
            people = 8;
        } else if(Score is > 25 and <= 50)
        {
            Level = 2;
            people = 95;
        } else if(Score > 50)
        {
            Level = 3;
            people = 200 ;
        }
        return Score;
    }
    
    public int GetPeople()
    {
        return people;
    }

    public override Data.GridType GetGridType()
    {
        return Data.GridType.Housing;
    }
}