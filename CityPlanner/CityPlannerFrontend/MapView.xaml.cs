using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace CityPlannerFrontend
{
    public sealed partial class MapView
    {
      public static API Interface { get; set; }
      public static MapTools MapTool { get; internal set; }

      private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

      private bool _pause;

      

      private string _gridCount;
      private string _satisfaction;
      private string _averageBuildingLevel;
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
            Interface.NextGeneration();
            Debug.WriteLine(Interface.existsNewMap());


            // saved in variable before because of multithreading, makes dispatchers execution time shorter and less likely to fail / show wrong or old values
            _generationCount = Interface.GetGeneration().ToString();

            _dispatcherQueue.TryEnqueue(() =>
            {
               // update UI elements with the updated variable values
               Generation.Text = _generationCount;
            });

            if (!Interface.existsNewMap()) continue;
            Debug.WriteLine("New Map");

            // saved in variable before because of multithreading, makes dispatchers execution time shorter and less likely to fail / show wrong or old values
            _gridCount = Interface.getPlacedBuildings().ToString();
            _satisfaction = Interface.GetSatisfaction().ToString();
            _averageBuildingLevel = Interface.GetAverageBuildLevel().ToString("0.00");
            _population = Interface.GetPopulation().ToString();
            _lastNewMap = _generationCount;

            _dispatcherQueue.TryEnqueue(() =>
            {
               // update UI elements with the updated variable values
               MapGridScrollViewer.Content = MapTool.MapGenerator(Interface.GetMapToFrontend()); // for MapGrid it's not possible to prepare the updated grid in advance because it's a nested object
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
         Frame.Navigate(typeof(Settings));
      }
   }
}