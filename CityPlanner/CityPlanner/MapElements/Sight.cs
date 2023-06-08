// @author: Kevin Kern

namespace CityPlanner.MapElements;

public class Sight : MapElement
{
   public Sight(MapElement mapElement) : base(mapElement)
   {
   }

   public override void AddDependency(Data.GridType gridType, double distance)
   {
      switch (gridType)
      {
         case Data.GridType.Street:
         case Data.GridType.Commercial:
         case Data.GridType.Housing:
            Dependency[gridType].Add(distance);
            break;
      }
   }

   public override int CalculateScore()
   {
      Score = 0;


      Score += Dependency[Data.GridType.Housing].Count * 50;

      // Base cost
      Score += 20;

      // Level
      UpdateLevel();

      return Score;
   }

   public override bool IsInRangeOfStreet()
   {
      return Dependency[Data.GridType.Street].Count() > 0;
   }

   public override Data.GridType GetGridType()
   {
      return Data.GridType.Sight;
   }

   public override Sight Clone()
   {
      return new Sight(this);
   }
}