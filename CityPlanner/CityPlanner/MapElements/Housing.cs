// @author: Kevin Kern, Sander Stella

namespace CityPlanner.MapElements;

public class Housing : MapElement
{
   private int _people;

   public Housing(MapElement mapElement) : base(mapElement)
   {
   }

   public override void AddDependency(Data.GridType gridType, double distance)
   {
      switch (gridType)
      {
         case Data.GridType.Street:
            Dependency[gridType].Add(distance);
            break;

         case Data.GridType.Commercial:
            if (distance <= 4.9)
               Dependency[gridType].Add(distance);
            break;
         case Data.GridType.Industry:
            if (distance <= 3.4)
               Dependency[gridType].Add(distance);
            break;
      }
   }

   public override int CalculateScore()
   {
      Score = 0;

      Score += Dependency[Data.GridType.Commercial].Count * 50;
      Score += (int)(5 * (2 * Math.Sin(1.1 * (Dependency[Data.GridType.Street][0]) - 0.6)));
      Score += Dependency[Data.GridType.Industry].Count * -100;
      Score += Dependency[Data.GridType.Subway].Count * 100;
      Score += Dependency[Data.GridType.Sight].Count * 200;

      // Base cost
      Score -= 5;

      UpdateLevel();

      return Score;
   }

   protected override void UpdateLevel()
   {
      switch (Score)
      {
         // According Level
         case <= 0:
            Level = 1;
            _people = 0;
            break;
         case <= 250:
            Level = 1;
            _people = 8;
            break;
         case > 250 and <= 600:
            Level = 2;
            _people = 95;
            break;
         case > 600:
            Level = 3;
            _people = 200;
            break;
      }
   }



   public override bool IsInRangeOfStreet()
   {
      return Dependency[Data.GridType.Street].Count() > 0;
   }


   public int GetPeople()
   {
      return _people;
   }

   public override Data.GridType GetGridType()
   {
      return Data.GridType.Housing;
   }

   public override Housing Clone()
   {
      return new Housing(this);
   }
}