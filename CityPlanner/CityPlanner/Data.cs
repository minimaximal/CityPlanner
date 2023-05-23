using CityPlanner.Grid;

namespace CityPlanner;
public static class Data
{
    //Empty must be the last in the list otherwise Agent.cs:getRandomMove() does not work
    public enum GridType
    {
        Housing,
        Commercial,
        Industry,
        Street,
        Empty
    };

    //holds amount wthout Empty 
    public static readonly int GridTypeAmount = Enum.GetValues(typeof(Data.GridType)).Length - 1;
    
    public static readonly Dictionary<GridType, double> GridTypeMax = new Dictionary<GridType, double>()
    {
        { GridType.Housing, 5 },
        { GridType.Commercial, 9.9 },
        { GridType.Industry, 5.9 },
        { GridType.Street, 3.5 },
        { GridType.Empty, 0 }
    };

    public static int SizeX; // this is used in the sort move context // at that point we dont have acses to the map 
}