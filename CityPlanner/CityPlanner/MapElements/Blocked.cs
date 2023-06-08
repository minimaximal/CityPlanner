// @author: Kevin Kern

namespace CityPlanner.MapElements;

public class Blocked : MapElement
{
   public Blocked(MapElement mapElement) : base(mapElement)
   {
   }

   public override Data.GridType GetGridType()
   {
      return Data.GridType.Blocked;
   }

   public override int CalculateScore()
   {
      return 0;
   }

   public override MapElement Clone()
   {
      return new Blocked(this);
   }
}