// @author: Maximilian Koch
// Object wrapper class for passing data from page settings to page map editor, because only one parameter can be passed

namespace CityPlannerFrontend.UiPassing
{
    internal class SettingsToMapEditor : UiPassing
    {
        private readonly MapTools _mapTool;


        public SettingsToMapEditor(int sizeX, int sizeY, int population, int importQuota, int numberAgents, double mutationChance, MapTools mapTool) : base(sizeX, sizeY, population, importQuota, numberAgents, mutationChance)
        {
            _mapTool = mapTool;
        }


        public MapTools GetMapTool()
        {
            return _mapTool;
        }
    }
}