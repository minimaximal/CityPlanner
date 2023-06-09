﻿// @author: Leo Schnüll
// @author for /Assets/: Leo Schnüll
// App is the entry point of the application. It is responsible for creating the main window and navigating to the first page.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace CityPlannerFrontend
{
    public partial class App
    {
        public Window MWindow;
        public static Window MainWindow { get; } = new MainWindow();


        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            MWindow = new MainWindow();

            // Create a Frame to act as the navigation context and navigate to the first page
            Frame rootFrame = new();
            rootFrame.NavigationFailed += OnNavigationFailed;

            // Navigate to the first page, configuring the new page by passing required information as a navigation parameter
            rootFrame.Navigate(typeof(MainPage), args.Arguments);

            // Place the frame in the current Window
            MWindow.Content = rootFrame;

            // Ensure the MainWindow is active
            MWindow.Activate();
        }

        private static void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
    }
}
