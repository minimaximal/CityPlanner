namespace CityPlanner;

public class AppController
{
	
	public AppController()
	{
	}

	public void Start()
	{
		AgentController agentController = new AgentController((10,10), new (int,int)[]{(5,5)}, 500, 10);
		while (true)
		{
			Agent bestAgent = agentController.ExecuteEvolutionStep();
			bestAgent.Display();
		}
	}
}