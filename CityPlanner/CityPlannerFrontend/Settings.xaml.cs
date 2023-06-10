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

       private readonly MapTools _mapTool = new();


      public Settings()
      {
         this.InitializeComponent();
      }

      protected override void OnNavigatedTo(NavigationEventArgs e)
      {
          // Copy variables from map editor or view page into settings own variables
          if (e.Parameter is ToSettings toSettings)
          {
              SizeX.Value = toSettings.GetSizeX();
              SizeY.Value = toSettings.GetSizeY();
              Population.Value = toSettings.GetPopulation();
              ImportQuota.Value = toSettings.GetImportQuota();
              NumberAgents.Value = toSettings.GetNumberAgents();
              MutationChance.Value = toSettings.GetMutationChance();
          }
          // If no variables are passed, set default values
          else
          {
              SizeX.Value = DefaultSizeX;
              SizeY.Value = DefaultSizeY;
              Population.Value = DefaultPopulation;
              ImportQuota.Value = DefaultImportQuota;
              NumberAgents.Value = DefaultNumberAgents;
              MutationChance.Value = DefaultMutationChance;
          }
          base.OnNavigatedTo(e);
      }

      private void BtnResetParameter(object sender, RoutedEventArgs e)
      {
          SizeX.Value = DefaultSizeX;
          SizeY.Value = DefaultSizeY;
          Population.Value = DefaultPopulation;
          ImportQuota.Value = DefaultImportQuota;
          NumberAgents.Value = DefaultNumberAgents;
          MutationChance.Value = DefaultMutationChance;
      }

      private void BtnMainPage(object sender, RoutedEventArgs e)
      {
         Frame.Navigate(typeof(MainPage));
      }

      private void BtnMapView(object sender, RoutedEventArgs e)
      {
         if (!InputValidation()) return;
         var api = new Api((int)Population.Value, new byte[(int)SizeX.Value, (int)SizeY.Value], (int)ImportQuota.Value, (int)NumberAgents.Value, MutationChance.Value);
         var toMapView = new ToMapView((int)SizeX.Value, (int)SizeY.Value, (int)Population.Value, (int)ImportQuota.Value, (int)NumberAgents.Value, MutationChance.Value, _mapTool, api);
         Frame.Navigate(typeof(MapView), toMapView);
      }

      private void BtnMapEditor(object sender, RoutedEventArgs e)
      {
         if (!InputValidation()) return;
         var settingsToMapEditor = new SettingsToMapEditor((int)SizeX.Value, (int)SizeY.Value, (int)Population.Value, (int)ImportQuota.Value, (int)NumberAgents.Value, MutationChance.Value, _mapTool);
         Frame.Navigate(typeof(MapEditor), settingsToMapEditor);
      }

      private bool InputValidation()
      {
         if (SizeX.Value >= 5 && SizeY.Value >= 5)
         {
            return true;
         }

         Maptoosmall.IsOpen = true;
         return false;
      }
   }
}
