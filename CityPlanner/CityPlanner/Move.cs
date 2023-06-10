// @author: Sander Stella

namespace CityPlanner;

public class Move : IComparable
{
   public Data.GridType GridType;
   public readonly int X;
   public readonly int Y;
   public Move(int x, int y)
   {
      X = x;
      Y = y;
   }

   public Move(Move oldMove)
   {
      X = oldMove.X;
      Y = oldMove.Y;
      GridType = oldMove.GridType;
   }

   // Each move gets its own number according to the coordinates in order to identify holes in Agent.cs
   public int IndexNumber()
   {
      return Y * (Data.SizeX) + X;
   }

   public double DistanceToCenter()
   {
      double minDistance = 1000;
      foreach (var (x, y) in Data.InitialStreets)
      {
         var a = X - x;
         var b = Y - y;
         var distance = Math.Sqrt(a * a + b * b);
         if (distance < minDistance)
            minDistance = distance;

      }

      return minDistance;
   }

   public int CompareTo(object? obj)
   {
      var comp = (Move)obj!;

      return IndexNumber() - comp.IndexNumber();
   }
}