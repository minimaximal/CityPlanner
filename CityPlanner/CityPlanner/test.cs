using CityPlanner.Grid;


namespace CityPlanner;

using CityPlanner;

public class test
{
    bool tsetCopyofGrid_Deep()
    {
        GridElement n1 = new GridElement();
        Housing haus = new Housing(n1);
        haus.AddDependency(Data.GridType.Street,1);
        GridElement n2 = n1.Clone();
        Industry industry = new Industry(n2);

        
       // haus.Dependency[Data.GridType.Street].Count==1;
       // industry.Dependency[Data.GridType.Street].Count==0;

        return true;

    }
}