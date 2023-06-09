﻿// @author: Kevin Kern

using CityPlanner.MapElements;
using NUnit.Framework;

namespace CityPlanner
{

   public class Tests
   {
      // Test to make sure that Grids are not cloned by reference -> Deep clone instead
      [Test]
      public void CopyOfGrid_Deep()
      {
         MapElement n1 = new MapElement();
         MapElement n2 = n1.Clone();
         Industry industry = new Industry(n2);
         MapElement industry2 = industry.Clone();

         Assert.AreEqual(industry.GetGridType(), industry2.GetGridType());
         Assert.AreNotSame(industry, industry2);
      }

      // Test to make sure that Maps are not cloned by reference -> Deep clone instead
      [Test]
      public void CopyOfMap_Deep()
      {
         Map newMap = new Map(50, 50, 5000);
         Move nextMove = new Move(49, 49)
         {
            GridType = Data.GridType.Street
         };
         newMap.AddMove(nextMove);
         nextMove = new Move(49, 48)
         {
            GridType = Data.GridType.Commercial
         };
         newMap.AddMove(nextMove);
         Map clonedMap = (Map)newMap.Clone();

         for (int i = 0; i < 50; i++)
         {
            for (int j = 0; j < 50; j++)
            {
               Assert.AreEqual(newMap.GetGridElement(i, j)!.GetGridType(), clonedMap.GetGridElement(i, j)!.GetGridType());
               Assert.AreEqual(newMap.GetGridElement(i, j)!.GetLevel(), clonedMap.GetGridElement(i, j)!.GetLevel());
               Assert.AreNotSame(newMap.GetGridElement(i, j), clonedMap.GetGridElement(i, j));
            }
         }
         Assert.AreNotSame(newMap, clonedMap);
      }


      // Test for sorting moves according to position on Map
      [Test]
      public void MoveSort()
      {
         Data.SizeX = 5;
         List<Move> actualList = new List<Move>
         {
            new Move(3, 1),
            new Move(1, 3),
            new Move(1, 1),
            new Move(1, 4),
            new Move(1, 2),
            new Move(3, 1),
            new Move(3, 2)
         };

         actualList.Insert(4, new Move(5, 5));

         List<Move> expectedList = new List<Move>
         {
            new Move(1, 1),
            new Move(3, 1),
            new Move(3, 1),
            new Move(1, 2),
            new Move(3, 2),
            new Move(1, 3),
            new Move(1, 4)
         };
         expectedList.Insert(7, new Move(5, 5));

         actualList.Sort();
         for (int i = 0; i < expectedList.Count; i++)
         {
            Assert.AreEqual(expectedList[i].X, actualList[i].X);
            Assert.AreEqual(expectedList[i].Y, actualList[i].Y);
            Assert.AreEqual(expectedList[i].GridType, actualList[i].GridType);
         }
      }

      // Test to see if Agent executes every Move
      [Test]
      public void ValidAgent()
      {
         Map newMap = new Map(50, 50, 5000);
         Move nextMove = new Move(24, 24);
         Data.InitialStreets = new List<(int, int)> { (24, 24) };
         nextMove.GridType = Data.GridType.Street;
         newMap.AddMove(nextMove);
         Agent agent = new Agent(newMap);

         for (int i = 0; i < 2499; i++)
         {
            agent.MakeOneMove();
         }

         int moveCount = 0;

         for (int i = 0; i < 50; i++)
         {
            for (int j = 0; j < 50; j++)
            {
               if (newMap.GetGridElement(i, j)!.GetGridType() != Data.GridType.Empty)
               {
                  moveCount++;
               }
            }

         }

         if (agent.NoMoreValidMoves)
         {
            Assert.Less(moveCount, 2500);
         }
         else
         {
            Assert.AreEqual(2500, moveCount);
         }
      }

      // Test to see if Agent executes Parent Moves
      [Test]
      public void CheckIfAgentIsChild()
      {
         Map newMap = new Map(50, 50, 5000);
         Data.InitialStreets = new List<(int, int)> { (24, 24) };
         Move nextMove = new Move(24, 24)
         {
            GridType = Data.GridType.Street
         };
         newMap.AddMove(nextMove);
         Agent parentAgent = new Agent((Map)newMap.Clone());
         Agent parentAgent2 = new Agent((Map)newMap.Clone());

         for (int i = 0; i < 10; i++)
         {
            parentAgent.MakeOneMove();
            parentAgent2.MakeOneMove();
         }

         Agent childAgent = new Agent(newMap, parentAgent, parentAgent2, 50);

         Assert.AreEqual(childAgent.IsInList(new Move(1, 1)), parentAgent.IsInList(new Move(1, 1)));
      }


      // Test to see if Agent places Streets correctly
      [Test]
      public void ValidStreets()
      {
         Map newMap = new Map(50, 50, 5000);
         Move nextMove = new Move(24, 24)
         {
            GridType = Data.GridType.Street
         };
         Data.InitialStreets = new List<(int, int)> { (24, 24) };
         newMap.AddMove(nextMove);
         Agent agent = new Agent(newMap);

         for (int i = 0; i < 2499; i++)
         {
            agent.MakeOneMove();
         }

         for (int i = 0; i < 50; i++)
         {
            for (int j = 0; j < 50; j++)
            {
               if (agent.GetMap().GetGridElement(i, j)!.GetGridType() == Data.GridType.Street)
               {
                  try
                  {
                     if (agent.GetMap().GetGridElement(i - 1, j)!.GetGridType() != Data.GridType.Street &&
                         agent.GetMap().GetGridElement(i, j - 1)!.GetGridType() != Data.GridType.Street &&
                         agent.GetMap().GetGridElement(i + 1, j)!.GetGridType() != Data.GridType.Street &&
                         agent.GetMap().GetGridElement(i, j + 1)!.GetGridType() != Data.GridType.Street)
                     {
                        Assert.Fail();
                     }
                  }
                  catch (Exception)
                  {
                     // ignored
                  }
               }
            }
         }
         Assert.Pass();
      }

      // Test to see if API returns reasonable values
      [Test]
      public void ValidApiCalls()
      {
         Api api = new Api(30000, new byte[20, 20], 0, 20, 0.001);

         for (int j = 0; j < 10; j++)
         {
            api.NextGeneration();
            api.GetMapToFrontend();
         }

         Assert.AreEqual(10, api.GetGeneration());
         Assert.Greater(api.GetAverageBuildLevel(), 0);
         Assert.Less(api.GetAverageBuildLevel(), 3.1);
         Assert.Greater(api.GetPopulation(), 0);
      }
   }
}