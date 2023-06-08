using Microsoft.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;


namespace CityPlannerFrontend
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            var mAppWindow = GetAppWindowForCurrentWindow();
            mAppWindow.Title = "City Planner";
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            MyButton.Content = "Clicked";
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            var hWnd = WindowNative.GetWindowHandle(this);
            var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }
    }
}