using Avalonia;
using System;

namespace HotelManagementSystem.App
{
    /// <summary>
    /// Entry point class for the Hotel Management System application.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        /// <summary>
        /// Builds and configures the Avalonia application.
        /// </summary>
        /// <returns>The configured AppBuilder instance.</returns>
        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
        }
    }
}
