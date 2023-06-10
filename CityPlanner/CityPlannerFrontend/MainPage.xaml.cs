// @author: Leo Schnüll
// Main page of the application, which is the first page the user sees when starting the application

using Microsoft.UI.Xaml;

namespace CityPlannerFrontend
{
   public sealed partial class MainPage
   {
      public MainPage()
      {
         this.InitializeComponent();
      }

      private void BtnSettings(object sender, RoutedEventArgs e)
      {
         Frame.Navigate(typeof(Settings));
      }
   }
}
