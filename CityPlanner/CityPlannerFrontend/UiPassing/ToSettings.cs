// @author: Maximilian Koch

namespace CityPlannerFrontend.UiPassing
{
    internal class ToSettings
    {
        private readonly int _population;
        private readonly int _importQuota;
        private readonly int _sizeX;
        private readonly int _sizeY;

        public ToSettings(int population, int importQuota, int sizeX, int sizeY)
        {
            _population = population;
            _importQuota = importQuota;
            _sizeX = sizeX;
            _sizeY = sizeY;
        }

        public int GetPopulation()
        {
            return _population;
        }

        public int GetImportQuota()
        {
            return _importQuota;
        }

        public int GetSizeX()
        {
            return _sizeX;
        }

        public int GetSizeY()
        {
            return _sizeY;
        }
    }
}
