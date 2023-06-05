// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using CityPlanner;
using NUnit.Framework;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CityPlannerFrontend
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _mWindow = new MainWindow();
            
            // Create a Frame to act as the navigation context and navigate to the first page
            Frame rootFrame = new Frame();
            rootFrame.NavigationFailed += OnNavigationFailed;
            // Navigate to the first page, configuring the new page
            // by passing required information as a navigation parameter
            rootFrame.Navigate(typeof(MainPage), args.Arguments);

            // Place the frame in the current Window
            _mWindow.Content = rootFrame;
            _mWindow.Title = "City Planner";
            
            

            //var appWindow = AppWindow.GetFromWindowId(Microsoft.UI.Win32Interop.GetWindowIdFromWindow(WinRT.Interop.WindowNative.GetWindowHandle(this)));
            // appWindow.SetIcon("Assets/AppIcon/_AppIcon.ico");


            // Ensure the MainWindow is active
            _mWindow.Activate();
            
        }
        public static Window MainWindow { get; } = new MainWindow();

        public Window _mWindow;
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
    }
}
