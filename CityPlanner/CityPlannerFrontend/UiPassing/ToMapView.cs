// @author: Maximilian Koch

namespace CityPlannerFrontend.UiPassing
{
    internal class ToMapView : UiPassing
    {
        private readonly MapTools _mapTool;
        private readonly Api _api;
        
        public ToMapView(int sizeX, int sizeY, int population, int importQuota, MapTools mapTool, Api api) : base(sizeX, sizeY, population, importQuota)
        {
            _mapTool = mapTool;
            _api = api;
        }

        public MapTools GetMapTool()
        {
            return _mapTool;
        }

        public Api GetApi()
        {
            return _api;
        }
    }
}