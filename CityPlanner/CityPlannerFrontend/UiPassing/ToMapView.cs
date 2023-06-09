// @author: Maximilian Koch

using CityPlanner;

namespace CityPlannerFrontend.UiPassing
{
    internal class ToMapView
    {
        private readonly Api _api;
        private readonly MapTools _mapTool;
        private readonly int _population;
        private readonly int _importQuota;
        private readonly int _sizeX;
        private readonly int _sizeY;

        public ToMapView(Api api, MapTools mapTool, int population, int importQuota, int sizeX, int sizeY)
        {
            _api = api;
            _mapTool = mapTool;
            _population = population;
            _importQuota = importQuota;
            _sizeX = sizeX;
            _sizeY = sizeY;
        }

        public Api GetApi()
        {
            return _api;
        }

        public MapTools GetMapTool()
        {
            return _mapTool;
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
