//Author: Sander Stella, Paul Antoni, Kevin Kern

namespace CityPlanner;
public static class Data
{
    public static int SizeX; // this is used in the sort move context at that point we dont have access to the map
    public static int ImportQuota;
    public static int OptimalIndustryAmount;
    public static List<(int, int)> InitialStreets;
    
    
    //Empty must be the last entry in this list otherwise Agent.cs:getRandomMove() does not work
    public enum GridType
    {
        Housing,
        Commercial,
        Industry,
        Subway,
        Sight,
        Street,
        Blocked,
        Highway,
        Empty
        
    };

    //holds the amount of grid types excluding Empty
    public static readonly int GridTypeAmount = Enum.GetValues(typeof(Data.GridType)).Length - 1;
    
    public static readonly Dictionary<GridType, double> GridTypeMaxRange = new Dictionary<GridType, double>()
    {
        { GridType.Housing, 5 },
        { GridType.Commercial, 6.5 },
        { GridType.Industry, 4.9 },
        { GridType.Subway, 4.5 },
        { GridType.Sight, 6.0 },
        { GridType.Street, 3.5 },
        { GridType.Blocked, 0 },
        { GridType.Highway, 6 },
        { GridType.Empty, 0 }
    };
    
}