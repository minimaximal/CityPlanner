// @author: Leo Schnüll, Maximilian Koch

using CityPlanner;
using CityPlannerFrontend.UiPassing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CityPlannerFrontend
{
   public sealed partial class Settings : Page
   {
       private int _sizeX = 40;
       private int _sizeY = 40;
       private int _population = 50000;
       private int _importQuota; // 0 bis 100 (percent)
       private int _numberAgents = 20;
       private double _mutationChance = 0.001;
      
       private readonly MapTools _mapTool = new();


      public Settings()
      {
         this.InitializeComponent();
      }

      protected override void OnNavigatedTo(NavigationEventArgs e)
      {
          if (e.Parameter is ToSettings toSettings)
          {
              _sizeX = toSettings.GetSizeX();
              _sizeY = toSettings.GetSizeY();
              _population = toSettings.GetPopulation();
              _importQuota = toSettings.GetImportQuota();
              _numberAgents = toSettings.GetNumberAgents();
              _mutationChance = toSettings.GetMutationChance();
          }
          base.OnNavigatedTo(e);
      }


      private void BtnMainPage(object sender, RoutedEventArgs e)
      {
         Frame.Navigate(typeof(MainPage));
      }

      private void BtnMapView(object sender, RoutedEventArgs e)
      {
         if (!InputValidation()) return;
         var api = new Api(_population, new byte[_sizeX, _sizeY], _importQuota, _numberAgents, _mutationChance);
         var toMapView = new ToMapView(_sizeX, _sizeY, _population, _importQuota, _numberAgents, _mutationChance, _mapTool, api);
         Frame.Navigate(typeof(MapView), toMapView);
      }

      private void BtnMapEditor(object sender, RoutedEventArgs e)
      {
         if (!InputValidation()) return;
         var settingsToMapEditor = new SettingsToMapEditor(_sizeX, _sizeY, _population, _importQuota, _numberAgents, _mutationChance, _mapTool);
         Frame.Navigate(typeof(MapEditor), settingsToMapEditor);
      }

      private bool InputValidation()
      {
         if (_sizeX >= 5 && _sizeY >= 5)
         {
            return true;
         }

         Maptoosmall.IsOpen = true;
         return false;
      }
   }
}
