// @author: Maximilian Koch, Leo Schnüll
// UI class for the display of the current map and the corresponding statistics
// It is the most important page in the frontend

using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using System.Threading.Tasks;
using CityPlanner;
using CityPlannerFrontend.UiPassing;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CityPlannerFrontend
{
   public sealed partial class MapView
   {
       private int _sizeX;
       private int _sizeY;
       private int _targetPopulation;
       private int _importQuota;
       private int _numberAgents;
       private double _mutationChance;
       private static MapTools _mapTool;
       private Api _api; 
       private string _population;
       private string _satisfaction;
       private string _gridCount;
       private string _averageBuildingLevel;
       private string _generationCount;
       private string _lastNewMap;
       private bool _pause;
       private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
      

      public MapView()
      {
         this.InitializeComponent();
      }

      // Copy variables from settings or map editor page into map views own variables
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
          
          // Set ui elements to the values of the used parameters for the simulation (fixed values)
          MapSize.Text = _sizeX + " x " + _sizeY;
          TargetPopulation.Text = _targetPopulation.ToString();
          ImportQuota.Text = (_importQuota*100) + "%";
          NumberAgents.Text = _numberAgents.ToString();
          MutationChance.Text = (_mutationChance*100).ToString("0.00")  + "%";

          // Create and start backend loop, which updates the ui elements with the current values of the simulation
          Task task = new(() => { _ = BackendLoopAsync(); });
          task.Start();
      }
      
      // async background loop which updates the ui elements with the current values of the simulation
      private Task BackendLoopAsync()
      {
         Debug.WriteLine("entered backend loop");
         if (_api == null) return Task.CompletedTask;
         // Loop runs until the user presses the pause button
         while (!_pause)
         {
            Debug.WriteLine("next generation");
            _api.NextGeneration();
            Debug.WriteLine(_api.ExistsNewMap());


            // Saved in variable before because of multithreading, makes dispatchers execution time shorter and less likely to fail / show wrong or old values
            _generationCount = _api.GetGeneration().ToString();

            // Dispatchers are needed because the ui elements are not allowed to be updated from a different thread than the main thread
            _dispatcherQueue.TryEnqueue(() =>
            {
               // Update UI elements with the updated variable values
               Generation.Text = _generationCount;
            });

            if (!_api.ExistsNewMap()) continue;
            Debug.WriteLine("new map");

            // Saved in variable before because of multithreading, makes dispatchers execution time shorter and less likely to fail / show wrong or old values
            _population = _api.GetPopulation() + " / " +  _targetPopulation;
            _satisfaction = _api.GetSatisfaction().ToString();
            _gridCount = _api.GetPlacedBuildingsAmount().ToString();
            _averageBuildingLevel = _api.GetAverageBuildLevel().ToString("0.00");
            _lastNewMap = _generationCount;

            // Dispatchers are needed because the ui elements are not allowed to be updated from a different thread than the main thread
            _dispatcherQueue.TryEnqueue(() =>
            {
               // Update UI elements with the updated variable values
               Population.Text = _population;
               Satisfaction.Text = _satisfaction;
               GridCount.Text = _gridCount;
               AverageBuildingLevel.Text = _averageBuildingLevel;
               MapGridScrollViewer.Content = FillMap(_api.GetMapToFrontend()); // For MapGrid it's not possible to prepare the updated grid in advance because it's a nested object
               LastNewMap.Text = _lastNewMap;
            });
         }

         return Task.CompletedTask;
      }

      private static Grid FillMap(byte[,] map)
      {
          var rows = map.GetLength(0);
          var cols = map.GetLength(1);
          var grid = MapTools.PrepareEmptyMap(rows, cols);
          
          // 3. Add each item and set row and column
          for (var i = 0; i < rows; i++)
          {
              for (var j = 0; j < cols; j++)
              {
                  var element = new Image
                  {
                      Source = _mapTool.GetTextureBitmapImages()[map[i, j]]
                  };
                  grid.Children.Add(element);
                  Grid.SetColumn(element, j);
                  Grid.SetRow(element, i);
              }
          }
          return grid;
      }


      private void BtnPause(object sender, RoutedEventArgs e)
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

      private void BtnSettings(object sender, RoutedEventArgs e)
      {
         _pause = true;
         var toSettings = new ToSettings(_sizeX, _sizeY, _targetPopulation, _importQuota, _numberAgents, _mutationChance);
         Frame.Navigate(typeof(Settings), toSettings);
      }
   }
}