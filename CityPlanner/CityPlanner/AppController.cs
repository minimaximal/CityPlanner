using System;
using CityPlanner;

public class AppController
{
	
	// sgat dem algo controller ober weitermacht oder nicht 
	// kann save etc ansteuern
	// später
	public AppController()
	{
	}

	public void Start()
	{
		AgentController agentController = new AgentController((50,50), new (int,int)[]{(15,15),(35,35)}, 5000, 10);
		while (true)
		{
			Agent bestAgent = agentController.ExecuteEvolutionStep();
			bestAgent.Display();
		}
	}
}
