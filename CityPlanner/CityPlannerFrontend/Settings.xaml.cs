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
      private int _population = 50000;
      private int _importQuota; // 0 bis 100 (percent)
      private int _sizeX = 40;
      private int _sizeY = 40;
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
              _population = toSettings.GetPopulation();
              _importQuota = toSettings.GetImportQuota();
              _sizeX = toSettings.GetSizeX();
              _sizeY = toSettings.GetSizeY();
          }
          base.OnNavigatedTo(e);
      }


      private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
      {
         Frame.Navigate(typeof(MainPage));
      }

      private void Button_Click_MapView(object sender, RoutedEventArgs e)
      {
         if (!InputValidation()) return;
         
         var map = new byte[_sizeX, _sizeY];
         var appInterface = new Api(_population, map, _importQuota);
         var toMapView = new ToMapView(_sizeX, _sizeY, _population, _importQuota, _numberAgents, _mutationChance, _mapTool, appInterface);

         Frame.Navigate(typeof(MapView), toMapView);
      }


      private void Button_Click_MapEditor(object sender, RoutedEventArgs e)
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
