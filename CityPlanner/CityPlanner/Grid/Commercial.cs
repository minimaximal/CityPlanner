namespace CityPlanner.Grid;

public class Commercial : GridElement
{
    public Commercial(GridElement gridElement) : base(gridElement) {}

    
    public override int CalculateScore()
    {
        foreach (double Housing in Dependency[Data.GridType.Housing])
        {
            if (Housing <= 4.9)
            {
                Score += 2;
            }
        }
        foreach (double Commercial in Dependency[Data.GridType.Commercial])
        {
            if (Commercial <= 2)
            {
                Score += 20;
            }
            else if (Commercial > 3.5)
            {
                Score -= 16;
            }
        }
        foreach (double Industry in Dependency[Data.GridType.Industry])
        {
            if (Industry<= 6)
            {
                Score += 10;
            }
        }
        Dependency[Data.GridType.Street].Sort();
        if ( Dependency[Data.GridType.Street].Count()>0  && Dependency[Data.GridType.Street][0] > 1.0)
        {
            // Street in Range
            Score += 20;
        }
        else
        {
            //no Street in Range
            Score = 0;
        }
        //base cost
        Score -= 5;
        
        return Score;
    }
  

    public override Data.GridType GetGridType()
    {
        return Data.GridType.Commercial;
    }
    
}