namespace CityPlannerFrontend.UiPassing
{
    internal class UiPassing
    {
        private readonly int _sizeX;
        private readonly int _sizeY;
        private readonly int _population;
        private readonly int _importQuota;
        
        public UiPassing(int sizeX, int sizeY, int population, int importQuota)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
            _population = population;
            _importQuota = importQuota;
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
    }
}
