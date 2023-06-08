// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;



namespace CityPlannerFrontend
{
   
   public sealed partial class MapView : Page
   {
      public static API Interface { get; set; }
      public static MapTools MapTool;
      private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

      private bool _pause = false;
        
      private string _mapElementCount;
      private string _satisfaction;
      private string _avarageBuildingLevel;
      private string _population;
      private string _generationCount;
      private string _lastNewMap;



      public MapView()
      {
         this.InitializeComponent();

         

         Task task = new(() => { _ = BackendLoopAsync(); });
         task.Start();
      }

      private Task BackendLoopAsync()
      {
         Debug.WriteLine("entered BackendLoop");
         if (Interface == null) return Task.CompletedTask;
         while (!_pause)
         {
            Debug.WriteLine("Next Generation");
            Interface.nextGeneration();
            Debug.WriteLine(Interface.existsNewMap());


            // saved in variable before because of multithreading, makes dispatchers execution time shorter and less likely to fail / show wrong or old values
            _generationCount = Interface.getGeneration().ToString();

            _dispatcherQueue.TryEnqueue(() =>
            {
               // update UI elements with the updated variable values
               Generation.Text = _generationCount;
            });

            if (!Interface.existsNewMap()) continue;
            Debug.WriteLine("New Map");

            // saved in variable before because of multithreading, makes dispatchers execution time shorter and less likely to fail / show wrong or old values
            _mapElementCount = Interface.getPlacedBuildings().ToString();
            _satisfaction = Interface.getSatisfaction().ToString();
            _avarageBuildingLevel = Interface.getAverageBuildLevel().ToString(CultureInfo.InvariantCulture);
            _population = Interface.getPopulation().ToString();
            _lastNewMap = _generationCount;

            _dispatcherQueue.TryEnqueue(() =>
            {
               // update UI elements with the updated variable values
               MapGridScrollViewer.Content = MapTool.MapGenerator(Interface.getMapToFrontend()); // for MapGrid it's not possible to prepare the updated grid in advance because it's a nested object
               GridCount.Text = _mapElementCount;
               Satisfaction.Text = _satisfaction;
               AvarageBuildingLevel.Text = _avarageBuildingLevel;
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
         Frame.Navigate(typeof(Settings));
      }
    }
}