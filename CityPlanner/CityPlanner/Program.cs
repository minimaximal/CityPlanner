// See https://aka.ms/new-console-template for more information




//var test = new test();

//Console.WriteLine(test.testCopyofGrid_Deep());
//Console.WriteLine(test.testMoveSort());


using CityPlanner;

AppController appController = new AppController(10000, 50, 20 , 10);

for (int j = 0; j < 10000; j++)
{
    if(j%10 !=0 ) continue;
    Console.WriteLine("gen:" + j );
    Map  map = appController.nextGeneration();
    Console.WriteLine(map.getScore());
    Console.WriteLine(map.GetPeople());
    map.NewDisplay();
}

int i = 0; //somehow required to start programm or no Main method is found



