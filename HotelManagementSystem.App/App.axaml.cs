using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HotelManagementSystem.App.ViewModels;
using HotelManagementSystem.App.Views;
using HotelManagementSystem.Core.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HotelManagementSystem.App
{
    /// <summary>
    /// Main application class for the Hotel Management System.
    /// Handles application startup, initialization, and database setup.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the application and loads XAML resources.
        /// </summary>
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        /// <summary>
        /// Called when the application's framework initialization is complete.
        /// Sets up the database, initializes the main window, and configures the view model.
        /// </summary>
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var options = CreateDatabaseOptions();

                InitializeDatabase(options);

                var mainWindow = new MainWindow();
                
                var viewModel = new MainWindowViewModel(options);
                
                mainWindow.DataContext = viewModel;
                
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }

        /// <summary>
        /// Initializes the database by ensuring it exists and seeding it with initial data if necessary.
        /// </summary>
        /// <param name="options">The database context options.</param>
        private async void InitializeDatabase(DbContextOptions<HotelDbContext> options)
        {
            using (var db = new HotelDbContext(options))
            {
                var seeder = new DataSeeder(db);
                await seeder.SeedAsync();
            }
        }

        /// <summary>
        /// Creates and configures the database context options for SQLite.
        /// </summary>
        /// <returns>The configured database context options.</returns>
        private DbContextOptions<HotelDbContext> CreateDatabaseOptions()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string dataDirectory = Path.Combine(appDirectory, "Data");
            
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }
            
            string dbPath = Path.Combine(dataDirectory, "HotelManagementSystem.db");
            Console.WriteLine($"Database path: {dbPath}");

            var options = new DbContextOptionsBuilder<HotelDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;

            return options;
        }
    }
} 