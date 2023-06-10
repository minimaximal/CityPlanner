// @author: Sander Stella, Kevin Kern

namespace CityPlanner.MapElements;

public class Street : MapElement
{
    public Street(MapElement mapElement) : base(mapElement) { }

   public override void AddDependency(Data.GridType gridType, double distance)
   {
      switch (gridType)
      {
         case Data.GridType.Street:
         case Data.GridType.Highway :
            Dependency[gridType].Add(distance);
            break;
         case Data.GridType.Housing:
             break;
         case Data.GridType.Commercial:
             break;
         case Data.GridType.Industry:
             break;
         case Data.GridType.Subway:
             break;
         case Data.GridType.Sight:
             break;
         case Data.GridType.Blocked:
             break;
         case Data.GridType.Empty:
             break;
         default:
             throw new ArgumentOutOfRangeException(nameof(gridType), gridType, null);
      }
   }

   public override int CalculateScore()
   {
      Score = 100;

      Score -= Dependency[Data.GridType.Street].Count * 10;
      foreach (var unused in Dependency[Data.GridType.Street].Where(street => street <= 1))
      {
          Score += 10;
      }

      if (Dependency[Data.GridType.Highway].Count <= 0) return Score;
      Dependency[Data.GridType.Highway].Sort();
      var closest = Dependency[Data.GridType.Highway].First();

      Score += (int)Math.Pow(5, (6 - closest));
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