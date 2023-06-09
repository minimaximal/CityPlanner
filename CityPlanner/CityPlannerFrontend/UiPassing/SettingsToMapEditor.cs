// @author: Maximilian Koch

namespace CityPlannerFrontend.UiPassing
{
    internal class SettingsToMapEditor
    {
        private readonly int _population;
        private readonly int _importQuota;
        private readonly int _sizeX;
        private readonly int _sizeY;
        private readonly MapTools _mapTool;

        public SettingsToMapEditor(int population, int importQuota, int sizeX, int sizeY, MapTools mapTool)
        {
            _population = population;
            _importQuota = importQuota;
            _sizeX = sizeX;
            _sizeY = sizeY;
            _mapTool = mapTool;
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

        public MapTools GetMapTool()
        {
            return _mapTool;
        }
    }
}