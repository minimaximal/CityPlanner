namespace CityPlanner;

public class AppController
{
	
	public AppController(int importQuota)
	{
		Data.ImportQuota = importQuota;
	}

	public void Start()
	{
		AgentController agentController = new AgentController((50,20), new (int,int)[]{(9,9)}, 500000, 10);
		
		for (int i = 0; i < 100000; i++)
		{			
			Agent bestAgent = agentController.ExecuteEvolutionStep();

			if (i % 10 != 0) continue;
			Console.WriteLine("iteration:" +i);
			bestAgent.Display();


		}
	}
}