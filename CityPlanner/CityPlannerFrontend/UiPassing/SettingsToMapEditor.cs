// @author: Maximilian Koch

namespace CityPlannerFrontend.UiPassing
{
    internal class SettingsToMapEditor : UiPassing
    {
        private readonly MapTools _mapTool;

        public SettingsToMapEditor(int sizeX, int sizeY, int population, int importQuota, MapTools mapTool) : base(sizeX, sizeY, population, importQuota)
        {
            _mapTool = mapTool;
        }

        public MapTools GetMapTool()
        {
            return _mapTool;
        }
    }
}