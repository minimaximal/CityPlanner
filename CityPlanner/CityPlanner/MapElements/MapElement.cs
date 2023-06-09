// @author: Kevin Kern, Sander Stella

namespace CityPlanner.MapElements;

public class MapElement
{
   protected int Score;
   protected int Level = 1;
   // Holds Lists that contain distances to the next MapElement
   protected readonly IDictionary<Data.GridType, List<double>> Dependency = new Dictionary<Data.GridType, List<double>>();

   // Basic Constructor for initializing map
   public MapElement()
   {
      foreach (Data.GridType gridType in (Data.GridType[])Enum.GetValues(typeof(Data.GridType)))
      {
         Dependency.Add(gridType, new List<double>());
      }
   }

   // Advanced Constructor for replacing existing MapElements with new ones
   protected MapElement(MapElement oldMapElement)
   {
      foreach (var dependency in oldMapElement.Dependency)
      {
         Dependency.Add(dependency.Key, oldMapElement.Dependency[dependency.Key].ToArray().ToList());
      }

      Score = oldMapElement.Score;
      Level = oldMapElement.Level;
   }

   public int GetScore()
   {
      return Score;
   }

   public virtual int CalculateScore()
   {
      return -100;
   }

   protected virtual void UpdateLevel()
   {
      Level = 1;
   }

   public virtual void AddDependency(Data.GridType gridType, double distance)
   {
      if (gridType == Data.GridType.Street)
         Dependency[gridType].Add(distance);
   }

   public virtual Data.GridType GetGridType()
   {
      return Data.GridType.Empty;
   }

   // Checks if MapElement could possibly be a street
   public bool IsValidStreet()
   {
      Dependency[Data.GridType.Street].Sort();
      return Dependency[Data.GridType.Street].Any() && Dependency[Data.GridType.Street][0] <= 1.0;
   }

   public int GetLevel()
   {
      return Level;
   }


   public virtual MapElement Clone()
   {
      return new MapElement(this);
   }

   // Returns probability for a MapElement becoming a street instead
   public double GetProbability()
   {
      // Returns value between 0 and 1 

      Dependency[Data.GridType.Street].Sort();
      int counter = 0;
      while (Dependency[Data.GridType.Street].Count > counter && Dependency[Data.GridType.Street][counter] <= 1)
      {
         counter++;
      }

      if (Dependency[Data.GridType.Street].Exists(x =>x is > 1.3 and < 1.5) && counter ==2)
      {
         //if map looks kile this (S = street C= to check)
         // SS
         // SC
         //dont build a street
         return 0.0;
      }
      
      return 1.3 - 0.5 * (counter);
   }

   // Checks if there is a street nearby according to the rules for the specific MapElement
   public virtual bool IsInRangeOfStreet()
   {
      return false;
   }
}