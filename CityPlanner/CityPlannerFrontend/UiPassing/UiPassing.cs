// @author: Maximilian Koch
// Object wrapper base class for passing data from one page to another, because only one parameter can be passed

namespace CityPlannerFrontend.UiPassing
{
    internal class UiPassing
    {
        private readonly int _sizeX;
        private readonly int _sizeY;
        private readonly int _population;
        private readonly int _importQuota;
        private readonly int _numberAgents;
        private readonly double _mutationChance;


        public UiPassing(int sizeX, int sizeY, int population, int importQuota, int numberAgents, double mutationChance)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
            _population = population;
            _importQuota = importQuota;
            _numberAgents = numberAgents;
            _mutationChance = mutationChance;
        }


        public int GetSizeX()
        {
            return _sizeX;
        }

        public int GetSizeY()
        {
            return _sizeY;
        }

        public int GetPopulation()
        {
            return _population;
        }

        public int GetImportQuota()
        {
            return _importQuota;
        }

        public int GetNumberAgents()
        {
            return _numberAgents;
        }

        public double GetMutationChance()
        {
            return _mutationChance;
        }
    }
}

