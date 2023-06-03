// See https://aka.ms/new-console-template for more information




//var test = new test();

//Console.WriteLine(test.testCopyofGrid_Deep());
//Console.WriteLine(test.testMoveSort());


using CityPlanner;

AppController appController = new AppController(100000, 50, 20 , 10);
Map map;
for (int j = 0; j < 1000000000; j++)
{
    map =  appController.nextGeneration();

    if(j%100 !=0 ) continue;
    
    Console.WriteLine("gen:" + j );

    map.NewDisplay();
}

int i = 0; //somehow required to start programm or no Main method is found



