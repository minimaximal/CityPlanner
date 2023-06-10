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
       private const int DefaultSizeX = 40;
       private const int DefaultSizeY = 40;
       private const int DefaultPopulation = 50000;
       private const int DefaultImportQuota = 0; // 0 bis 100 (percent)
       private const int DefaultNumberAgents = 20;
       private const double DefaultMutationChance = 0.001;


       private int _sizeX = DefaultSizeX;
       private int _sizeY = DefaultSizeY;
       private int _population = DefaultPopulation;
       private int _importQuota = DefaultImportQuota; 
       private int _numberAgents = DefaultNumberAgents;
       private double _mutationChance = DefaultMutationChance;
      
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

      // TODO: fix this method because it is not working
      private void BtnResetParameter(object sender, RoutedEventArgs e)
      {
         _sizeX = DefaultSizeX;
         _sizeY = DefaultSizeY;
         _population = DefaultPopulation;
         _importQuota = DefaultImportQuota;
         _numberAgents = DefaultNumberAgents;
         _mutationChance = DefaultMutationChance;
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
