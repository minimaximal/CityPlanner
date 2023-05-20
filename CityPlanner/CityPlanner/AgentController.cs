

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

        public AgentController((int x, int y) mapSize, (int x, int y)[] startingPoints, int targetPopulation,
            int agentAmount)
        {
            _mapSize = mapSize;
            _stratingPoints = startingPoints;
            _targetPopulation = targetPopulation;
            _agentAmount = agentAmount < 6 ? 6 : agentAmount;
            _defaultMap = CreateNewMap();
        }

        public Agent ExecuteEvolutionStep() //didnt know better name, basically goes through one generation of agents
        {
            if (_agents.Count == 0)
            {
                CreateNewAgents(_agentAmount);
            }
            else
            {
                CreateNewAgents(_agentAmount, _agents);
            }

            int currentLargestPopulation = 0;
            List<Agent> finishedAgents = new();
            bool lastRun=false;
            while (_agents.Count > 0)
            {
                int moveLimit = (_targetPopulation - currentLargestPopulation) / 200;
         
                if (moveLimit > _agents[0].GetMaxRemainingMoves())
                {
                    moveLimit = _agents[0].GetMaxRemainingMoves();
                    lastRun = true;
                }

                for (int moveNumber = 0; moveNumber < moveLimit; moveNumber++)
                {
                    _agents.AsParallel().ForAll(agent => agent.MakeOneMove());
                    //_agents[0].MakeOneMove();
                }

                for (int i = 0; i < _agents.Count(); i++)
                {
                    if (_agents[i].NoMoreValidStreet || _agents[i].Population > _targetPopulation || lastRun)
                    {
                        finishedAgents.Add(_agents[i]);
                        _agents.Remove(_agents[i]);
                    }
                }
            }

            _agents = finishedAgents;

            return GetBestThreeAgents(finishedAgents)[0];
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
            _agents.Clear();
            (int firstAgent, int secondAgent)[] combinations = new (int, int)[]
                { (0, 1), (0, 2), (1, 2), (1, 0), (2, 0), (2, 1) };
            for (int i = 0; i < amount; i++)
            {
                Map map = (Map)_defaultMap.Clone();
                _agents.Add(new Agent(map, bestThreeAgents[combinations[i % 6].firstAgent],
                    bestThreeAgents[combinations[i % 6].secondAgent], (i + 1) /(double) (amount + 1)));
            }
        }

        private List<Agent> GetBestThreeAgents(IEnumerable<Agent> precedingAgents)
        {
            return precedingAgents.OrderByDescending(agent => agent.Score).Take(3).ToList();
        }

        private Map CreateNewMap()
        {
            Map map = new Map(_mapSize.x, _mapSize.y);
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