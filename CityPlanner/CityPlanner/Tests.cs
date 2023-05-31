using CityPlanner.Grid;
using NUnit.Framework;

namespace CityPlanner{
    
    public class Tests
    { 
        //Test to make sure that Grids are not cloned by reference -> Deep clone instead
        [Test]
        public void CopyofGrid_Deep()
        {
            GridElement n1 = new GridElement();
            GridElement n2 = n1.Clone();
            Industry industry = new Industry(n2);
            GridElement industry2 = industry.Clone();
            
            Assert.AreEqual(industry.GetGridType(), industry2.GetGridType());
            Assert.AreNotSame(industry, industry2);
        }
        
        //Test to make sure that Maps are not cloned by reference -> Deep clone instead
        [Test]
        public void CopyofMap_Deep()
        {
            Map newMap = new Map(50, 50, 5000);
            Move nextMove = new Move(49, 49);
            nextMove.GridType = Data.GridType.Street;
            newMap.AddMove(nextMove);
            nextMove = new Move(49,48);
            nextMove.GridType = Data.GridType.Commercial;
            newMap.AddMove(nextMove);
            Map clonedMap = (Map) newMap.Clone();

            for(int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    Assert.AreEqual(newMap.GetGridElement(i,j)!.GetGridType(), clonedMap.GetGridElement(i,j)!.GetGridType());
                    Assert.AreEqual(newMap.GetGridElement(i,j)!.GetLevel(), clonedMap.GetGridElement(i,j)!.GetLevel());
                    Assert.AreNotSame(newMap.GetGridElement(i,j), clonedMap.GetGridElement(i,j));
                }
            }
            Assert.AreNotSame(newMap, clonedMap);
        }
        
        
        //Test for sorting moves according to position on Map
        [Test]
        public void MoveSort()
        {
            Data.SizeX = 5;
            List<Move> actualList = new List<Move>();

            actualList.Add(new Move(3, 1));
            actualList.Add(new Move(1, 3));
            actualList.Add(new Move(1, 1));
            actualList.Add(new Move(1, 4));
            actualList.Add(new Move(1, 2));
            actualList.Add(new Move(3, 1));
            actualList.Add(new Move(3, 2));
            actualList.Insert(4, new Move(5, 5));

            List<Move> expectedList = new List<Move>();
            expectedList.Add(new Move(1, 1));
            expectedList.Add(new Move(3, 1));
            expectedList.Add(new Move(3, 1));
            expectedList.Add(new Move(1, 2));
            expectedList.Add(new Move(3, 2));
            expectedList.Add(new Move(1, 3));
            expectedList.Add(new Move(1, 4));
            expectedList.Insert(7, new Move(5, 5));

            actualList.Sort();
            for (int i = 0; i < expectedList.Count; i++)
            {
                Assert.AreEqual(expectedList[i].X, actualList[i].X);
                Assert.AreEqual(expectedList[i].Y, actualList[i].Y);
                Assert.AreEqual(expectedList[i].GridType, actualList[i].GridType);
            }
            
        }
    }
}