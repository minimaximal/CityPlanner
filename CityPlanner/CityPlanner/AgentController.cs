namespace CityPlanner
{
    public class AgentController
    {
        private List<Agent> _agents = new();
        private readonly int _agentAmount;
        private readonly (int x, int y) _mapSize;
        private readonly (int x, int y)[] _stratingPoints;
        private readonly int _targetPopulation;
        private readonly Map _defaultMap;
        private Agent _bestOfAllTime;
        private int _moveLimitEstimate;

        private bool _includeBestOfAllTime = true;

        public AgentController((int x, int y) mapSize, (int x, int y)[] startingPoints, int targetPopulation,
            int agentAmount)
        {
            Data.SizeX = mapSize.x;
            _mapSize = mapSize;
            _stratingPoints = startingPoints;
            _targetPopulation = targetPopulation;
            _agentAmount = agentAmount < 6 ? 6 : agentAmount;
            _defaultMap = CreateNewMap();
            Data.InitialStreets = startingPoints.ToList();
        }

        public Agent ExecuteEvolutionStep()
        {
            if (_agents.Count == 0)
            {
                CreateNewAgents(_agentAmount);
            }
            else
            {
                CreateNewAgents(_agentAmount, _agents);
            }

            _includeBestOfAllTime = true;


            _agents.AsParallel().ForAll(agent => agent.MakeNMoves(_moveLimitEstimate));
            _agents.AsParallel().ForAll(agent => agent.CalculateScore());

            Agent generationalBestAgent = _agents.OrderByDescending(agent => agent.Score).First();
            if (_bestOfAllTime == null || _bestOfAllTime.Score < generationalBestAgent.Score) ;
            {
                _bestOfAllTime = generationalBestAgent;
                _includeBestOfAllTime = false;
            }
            return generationalBestAgent;
        }

        private void CreateNewAgents(int amount)
        {
            _agents.Clear();
            for (int i = 0; i < amount; i++)
            {
                Map map = (Map)_defaultMap.Clone();
                _agents.Add(new Agent(map));
            }
        }

        private void CreateNewAgents(int amount, IEnumerable<Agent> precedingAgents)
        {
            List<Agent> bestThreeAgents = GetBestThreeAgents(precedingAgents);
            if (_includeBestOfAllTime)
                bestThreeAgents[2] = _bestOfAllTime;
            _agents.Clear();
            (int firstAgent, int secondAgent)[] combinations = new (int, int)[]
                { (0, 1), (0, 2), (1, 2), (1, 0), (2, 0), (2, 1) };
            for (int i = 0; i < amount; i++)
            {
                Map map = (Map)_defaultMap.Clone();
                _agents.Add(new Agent(map, bestThreeAgents[combinations[i % 6].firstAgent],
                    bestThreeAgents[combinations[i % 6].secondAgent], (i + 1) / (double)(amount + 1)));
            }
        }

        private List<Agent> GetBestThreeAgents(IEnumerable<Agent> precedingAgents)
        {
            return precedingAgents.OrderByDescending(agent => agent.Score).Take(3).ToList();
        }

        private Map CreateNewMap()
        {
            // todo wenn das frontend eine map übergibt müssen die EMPTY hier gezählt werden
            _moveLimitEstimate = _mapSize.x * _mapSize.y - 1;

            Map map = new Map(_mapSize.x, _mapSize.y, _targetPopulation);
            foreach ((int x, int y) in _stratingPoints)
            {
                Move move = new Move(x, y)
                {
                    GridType = Data.GridType.Street
                };
                map.AddMove(move);
            }

            return map;
        }
    }
}