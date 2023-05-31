using CityPlanner.Grid;
using NUnit.Framework;

namespace CityPlanner{
    
    public class Tests
    {
        [Test]
        public void CopyofGrid_Deep()
        {
            GridElement n1 = new GridElement();
            Housing haus = new Housing(n1);
            haus.AddDependency(Data.GridType.Street, 1);
            GridElement n2 = n1.Clone();
            Industry industry = new Industry(n2);
            GridElement industry2 = industry.Clone();
            
            Assert.AreEqual(industry.GetGridType(), industry2.GetGridType());
            Assert.AreNotSame(industry, industry2);
        }

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