//Author: Sander Stella, Kevin Kern

namespace CityPlanner.Grid;

public class Street : GridElement
{
    public Street(GridElement gridElement) : base(gridElement)
    {
    }

    public override int CalculateScore()
    {
        Score = 100;
        
        Score -= Dependency[Data.GridType.Street].Count * 10;
        foreach (double street in Dependency[Data.GridType.Street])
        {
            if (street <= 1)
            {
                Score +=10;
            }
        }
        
        if (Dependency[Data.GridType.Highway].Count>0)
        {
            Dependency[Data.GridType.Highway].Sort();
            double closest = Dependency[Data.GridType.Highway].First();
          
            Score += (int) Math.Pow(5,(6-closest));
        }
        return Score;
    }

    public override Data.GridType GetGridType()
    {
        return Data.GridType.Street;
    }

    public override Street Clone()
    {
        return new Street(this);
    }
}