// @author: Leo Schnüll

using Microsoft.UI.Xaml;

namespace CityPlannerFrontend
{
   public sealed partial class MainPage
   {
      public MainPage()
      {
         this.InitializeComponent();
      }

      private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
      {
         Frame.Navigate(typeof(Settings));
      }
   }
}
