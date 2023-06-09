// @author: Maximilian Koch
// Object wrapper class for passing data from page map editor and view to page settings, because only one parameter can be passed

namespace CityPlannerFrontend.UiPassing
{
    internal class ToSettings : UiPassing
    {
        public ToSettings(int sizeX, int sizeY, int population, int importQuota, int numberAgents, double mutationChance) : base(sizeX, sizeY, population, importQuota, numberAgents, mutationChance) {}
    }
}
