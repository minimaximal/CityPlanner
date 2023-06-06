// See https://aka.ms/new-console-template for more information




//var test = new test();

//Console.WriteLine(test.testCopyofGrid_Deep());
//Console.WriteLine(test.testMoveSort());


using CityPlanner;

AppController appController = new AppController(30000, 50, 20 , 0);
Map bestMap = null;
Map map = null;
for (int j = 0; j < 1000000; j++)
{
    map =  appController.nextGeneration();

    if (j % 100 == 0)
    {
        Console.WriteLine("gen:" + j);

        map.Display();
    }

    if (bestMap == null || bestMap.GetScore() < map.GetScore())
    {

        bestMap = map;

        Console.WriteLine("gen:" + j);

        map.Display();
    }
}

int i = 0; //somehow required to start programm or no Main method is found



