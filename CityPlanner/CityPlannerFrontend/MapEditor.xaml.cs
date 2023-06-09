// @author: Maximilian Koch

using CityPlanner;
using CityPlannerFrontend.UiPassing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace CityPlannerFrontend
{

    public sealed partial class MapEditor
   {
       private int _population;
       private int _importQuota;
       private int _sizeX;
       private int _sizeY;
       private MapTools _mapTool;

       private static byte[,] _map;
       private static Grid _grid; 
       private byte _selectedMapElement;

       private readonly Button[] _btnList;


      public MapEditor()
      {
         this.InitializeComponent();
         _btnList = new[] { Btn0, Btn11, Btn21, Btn32 };
      }

      protected override void OnNavigatedTo(NavigationEventArgs e)
      {
          if (e.Parameter is SettingsToMapEditor settingsToMapEditor)
          {
              _population = settingsToMapEditor.GetPopulation();
              _importQuota = settingsToMapEditor.GetImportQuota();
              _sizeX = settingsToMapEditor.GetSizeX();
              _sizeY = settingsToMapEditor.GetSizeY();
              _mapTool = settingsToMapEditor.GetMapTool();
          }
          base.OnNavigatedTo(e);

          _map = new byte[_sizeX, _sizeY];
          _grid = GridGenerator(_map);
          MapGridScrollViewer.Content = _grid;
      }


      public void GridTile_Click(object sender, RoutedEventArgs e)
      {
         var row = Grid.GetRow((FrameworkElement)sender);
         var col = Grid.GetColumn((FrameworkElement)sender);


         _map[row, col] = _selectedMapElement;

         _grid.Children.Remove((FrameworkElement)sender);
         MapGridScrollViewer.Content = ChangeSingleMapElement(_selectedMapElement, _grid, row, col);
      }


      public Grid GridGenerator(byte[,] map)
      {
         var grid = new Grid();
         var rows = map.GetLength(0);
         var cols = map.GetLength(1);


         // 1. Prepare RowDefinitions
         for (var i = 0; i < rows; i++)
         {
            var row = new RowDefinition
            {
               Height = new GridLength(0, GridUnitType.Auto)
            };
            grid.RowDefinitions.Add(row);
         }

         // 2. Prepare ColumnDefinitions
         for (var j = 0; j < cols; j++)
         {
            var column = new ColumnDefinition
            {
               Width = new GridLength(0, GridUnitType.Auto)
            };
            grid.ColumnDefinitions.Add(column);
         }

         // 3. Add each item and set row and column
         for (var i = 0; i < rows; i++)
         {
            for (var j = 0; j < cols; j++)
            {
               grid = ChangeSingleMapElement(map[i, j], grid, i, j);
            }
         }
         return grid;
      }


      private Grid ChangeSingleMapElement(byte mapElementId, Grid grid, int row, int col)
      {
         var img = new Image
         {
            Source = _mapTool.GetTextureBitmapImages()[mapElementId]
         };
         var tile = new Button
         {
            Content = img,
            Padding = new Thickness(0),
            BorderThickness = new Thickness(0),
            CornerRadius = new CornerRadius(0),
            ClickMode = ClickMode.Press
         };
         tile.Click += GridTile_Click;


         grid.Children.Add(tile);
         Grid.SetRow(tile, row);
         Grid.SetColumn(tile, col);

         return grid;
      }

      private void MapElement_Click(object sender, RoutedEventArgs e)
      {
         var btn = byte.Parse(((Button)sender).Tag.ToString() ?? "0");
         _selectedMapElement = btn;

         foreach (var button in _btnList)
         {
            button.BorderThickness = new Thickness(0);
            button.BorderBrush = new SolidColorBrush(Colors.Transparent);
         }

         // Switch over all button tags
         switch (btn)
         {
            case 0:
               Btn0.BorderThickness = new Thickness(3);
               Btn0.BorderBrush = new SolidColorBrush(Colors.Red);
               break;
            case 11:
               Btn11.BorderThickness = new Thickness(3);
               Btn11.BorderBrush = new SolidColorBrush(Colors.Red);
               break;
            case 21:
               Btn21.BorderThickness = new Thickness(3);
               Btn21.BorderBrush = new SolidColorBrush(Colors.Red);
               break;
            case 32:
               Btn32.BorderThickness = new Thickness(3);
               Btn32.BorderBrush = new SolidColorBrush(Colors.Red);
               break;
         }
      }

      private void Button_Click_MapView(object sender, RoutedEventArgs e)
      {
         var appInterface = new Api(_population, _map, _importQuota);
         var toMapView = new ToMapView(_sizeX, _sizeY, _population, _importQuota, _mapTool, appInterface);
         Frame.Navigate(typeof(MapView), toMapView);
      }


      private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
      {
         var toSettings = new ToSettings(_sizeX, _sizeY, _population, _importQuota);
         Frame.Navigate(typeof(Settings), toSettings);
      }

   }
}
