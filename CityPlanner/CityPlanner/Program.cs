// @author: Paul Antoni, Sander Stella, Kevin Kern

// This program is only used for backend testing purposes as it is much faster than starting the frontend

using CityPlanner;

Map map = new Map(20, 20, 30000);
Move move = new Move(10, 10);
move.GridType = Data.GridType.Street;
map.AddMove(move);
AppController appController = new AppController(30000, map, 0);
Map bestMap = null;

for (int j = 0; j < 1000000; j++)
{
   Map nextMap = appController.NextGeneration();

   if (j % 100 == 0)
   {
      Console.WriteLine("gen:" + j);

      nextMap.Display();
   }

   if (bestMap == null || bestMap.GetScore() < nextMap.GetScore())
   {

      bestMap = nextMap;

      Console.WriteLine("gen:" + j);

      nextMap.Display();
   }
}



