// @author: Leo Schnüll
// Main and only window of the application. It is responsible for setting the title of the window.

using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;


namespace CityPlannerFrontend
{
    public sealed partial class MainWindow
    { 
        // Object for the current window to set the title and resize the window from other pages
        public static AppWindow MainAppWindow;

       public MainWindow()
      {
         this.InitializeComponent();

         MainAppWindow = GetAppWindowForCurrentWindow();
         MainAppWindow.Title = "City Planner";
         MainAppWindow.SetIcon("Assets/AppIcon/_AppIcon.ico");
         MainAppWindow.Resize(new Windows.Graphics.SizeInt32(1500, 800));
      }
      
      private AppWindow GetAppWindowForCurrentWindow()
      {
         var hWnd = WindowNative.GetWindowHandle(this);
         var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
         return AppWindow.GetFromWindowId(wndId);
      }
   }
}