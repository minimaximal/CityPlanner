// @author: Kevin Kern, Sander Stella

namespace CityPlanner.MapElements;

public class Commercial : MapElement
{
   public Commercial(MapElement mapElement) : base(mapElement) { }

   public override void AddDependency(Data.GridType gridType, double distance)
   {
      switch (gridType)
      {
         case Data.GridType.Street:
         case Data.GridType.Commercial:
            Dependency[gridType].Add(distance);
            break;
         case Data.GridType.Housing:
            if (distance <= 4.9)
               Dependency[gridType].Add(distance);
            break;
         case Data.GridType.Industry:
            if (distance <= 4)
               Dependency[gridType].Add(distance);
            break;
         case Data.GridType.Subway:
            if (distance <= 1.5)
               Dependency[gridType].Add(distance);
            break;
         case Data.GridType.Sight:
             break;
         case Data.GridType.Blocked:
             break;
         case Data.GridType.Highway:
             break;
         case Data.GridType.Empty:
             break;
         default:
             throw new ArgumentOutOfRangeException(nameof(gridType), gridType, null);
      }
   }

   public override int CalculateScore()
   {
      Score = 0;


      Score += Dependency[Data.GridType.Housing].Count * 2;
      Score += Dependency[Data.GridType.Industry].Count * 500;
      if (Dependency[Data.GridType.Subway].Count > 0)
      {
         Score += 150;
      }

      foreach (var commercial in Dependency[Data.GridType.Commercial])
      {
          switch (commercial)
          {
              case <= 2:
                  Score += 700;
                  break;
              case > 3.5:
                  Score -= 500;
                  break;
          }
      }

      // Base cost
      Score += 15;

      // Level
      UpdateLevel();

      return Score;
   }

   protected override void UpdateLevel()
   {
      Level = Score switch
      {
         < -550 => 1,
         < 150 => 2,
         > 150 => 3,
         _ => Level
      };
   }


   public override bool IsInRangeOfStreet()
   {
      return IsValidStreet();
   }


   public override Data.GridType GetGridType()
   {
      return Data.GridType.Commercial;
   }

   public override Commercial Clone()
   {
      return new Commercial(this);
   }
}