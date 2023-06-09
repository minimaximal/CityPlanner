// @author: Maximilian Koch
// Object wrapper class for passing data from page settings and page map editor to page map view, because only one parameter can be passed

using CityPlanner;

namespace CityPlannerFrontend.UiPassing
{
    internal class ToMapView : UiPassing
    {
        private readonly MapTools _mapTool;
        private readonly Api _api;
        

        public ToMapView(int sizeX, int sizeY, int population, int importQuota, int numberAgents, double mutationChance, MapTools mapTool, Api api) : base(sizeX, sizeY, population, importQuota, numberAgents, mutationChance)
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