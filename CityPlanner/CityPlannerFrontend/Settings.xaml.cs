// @author: Leo Schnüll, Maximilian Koch

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CityPlannerFrontend
{
   public sealed partial class Settings : Page
   {
      public int Population = 10000;
      public int ImportQuota = 10; // 0 bis 100 (percent)
      public int X = 50;
      public int Y = 30;
      private readonly MapTools _gridTool = new();

      public Settings()
      {
         this.InitializeComponent();
      }

      private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
      {
         Frame.Navigate(typeof(MainPage));
      }

      private void Button_Click_MapView(object sender, RoutedEventArgs e)
      {
         if (!InputValidation()) return;
         var map = new byte[X, Y];
         var @interface = new Api(Population, map, ImportQuota);
         MapView.Interface = @interface;
         MapView.MapTool = _gridTool;
         Frame.Navigate(typeof(MapView));
      }


      private void Button_Click_MapEditor(object sender, RoutedEventArgs e)
      {
         if (!InputValidation()) return;
         MapEditorHelpers.Population = Population;
         MapEditorHelpers.ImportQuota = ImportQuota;
         MapEditorHelpers.X = X;
         MapEditorHelpers.Y = Y;
         MapEditorHelpers.MapTool = _gridTool;
         Frame.Navigate(typeof(MapEditor));
      }

      private bool InputValidation()
      {
         if (X >= 5 && Y >= 5)
         {
            return true;
         }

         Maptoosmall.IsOpen = true;
         return false;

      }
   }
}
