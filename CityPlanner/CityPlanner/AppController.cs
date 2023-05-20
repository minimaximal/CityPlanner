namespace CityPlanner;

public class AppController
{
	
	public AppController()
	{
	}

	public void Start()
	{
		AgentController agentController = new AgentController((50,20), new (int,int)[]{(5,5)}, 500, 10);
		for (int i = 0; i < 100; i++)
		{			
			Agent bestAgent = agentController.ExecuteEvolutionStep();

			if (i % 10 != 0) continue;
			Console.WriteLine("iteration:" +i);
			bestAgent.Display();


		}
	}
}