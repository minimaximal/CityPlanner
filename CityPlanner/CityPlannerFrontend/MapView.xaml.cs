// @author: Maximilian Koch, Leo Schnüll

using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using System.Threading.Tasks;
using CityPlanner;
using CityPlannerFrontend.UiPassing;
using Microsoft.UI.Xaml.Navigation;

namespace CityPlannerFrontend
{
   public sealed partial class MapView
   {
       private Api _api; 
       private static MapTools _mapTool;
      private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
      private bool _pause;
      private string _gridCount;
      private string _satisfaction;
      private string _averageBuildingLevel;
      private string _population;
      private string _generationCount;
      private string _lastNewMap;

      private int _targetPopulation;
      private int _importQuota;
      private int _sizeX;
      private int _sizeY;
      private int _numberAgents;
      private double _mutationChance;



      public MapView()
      {
         this.InitializeComponent();
      }

      
      protected override void OnNavigatedTo(NavigationEventArgs e)
      {
          if (e.Parameter is ToMapView settingsToMapView)
          {
              _sizeX = settingsToMapView.GetSizeX();
              _sizeY = settingsToMapView.GetSizeY();
              _targetPopulation = settingsToMapView.GetPopulation();
              _importQuota = settingsToMapView.GetImportQuota();
              _numberAgents = settingsToMapView.GetNumberAgents();
              _mutationChance = settingsToMapView.GetMutationChance();
              _mapTool = settingsToMapView.GetMapTool();
              _api = settingsToMapView.GetApi();
          }
          base.OnNavigatedTo(e);
          
          Task task = new(() => { _ = BackendLoopAsync(); });
          task.Start();
      }
        

      private Task BackendLoopAsync()
      {
         Debug.WriteLine("entered backend loop");
         if (_api == null) return Task.CompletedTask;
         while (!_pause)
         {
            Debug.WriteLine("next generation");
            _api.NextGeneration();
            Debug.WriteLine(_api.ExistsNewMap());


            // Saved in variable before because of multithreading, makes dispatchers execution time shorter and less likely to fail / show wrong or old values
            _generationCount = _api.GetGeneration().ToString();

            _dispatcherQueue.TryEnqueue(() =>
            {
               // Update UI elements with the updated variable values
               Generation.Text = _generationCount;
            });

            if (!_api.ExistsNewMap()) continue;
            Debug.WriteLine("new map");

            // Saved in variable before because of multithreading, makes dispatchers execution time shorter and less likely to fail / show wrong or old values
            _gridCount = _api.GetPlacedBuildingsAmount().ToString();
            _satisfaction = _api.GetSatisfaction().ToString();
            _averageBuildingLevel = _api.GetAverageBuildLevel().ToString("0.00");
            _population = _api.GetPopulation().ToString();
            _lastNewMap = _generationCount;

            _dispatcherQueue.TryEnqueue(() =>
            {
               // Update UI elements with the updated variable values
               MapGridScrollViewer.Content = _mapTool.MapGenerator(_api.GetMapToFrontend()); // For MapGrid it's not possible to prepare the updated grid in advance because it's a nested object
               GridCount.Text = _gridCount;
               Satisfaction.Text = _satisfaction;
               AverageBuildingLevel.Text = _averageBuildingLevel;
               Population.Text = _population;
               LastNewMap.Text = _lastNewMap;
            });
         }

         return Task.CompletedTask;
      }


      private void Pause_onclick(object sender, RoutedEventArgs e)
      {
         if (_pause)
         {
            Task.Run(() => { BackendLoopAsync(); });
            MapGridScrollViewer.Opacity = 1;
            PauseButton.Content = "Pause";
         }
         else
         {
            MapGridScrollViewer.Opacity = 0.6;
            PauseButton.Content = "Fortsetzen";
         }
         _pause = !_pause;
      }


      private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
      {
         _pause = true;
         
         var toSettings = new ToSettings(_sizeX, _sizeY, _targetPopulation, _importQuota, _numberAgents, _mutationChance);
         
         Frame.Navigate(typeof(Settings), toSettings);
      }
   }
}