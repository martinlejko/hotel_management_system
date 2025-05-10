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
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Set up the database
                string dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "HotelManagementSystem.db");

                var options = new DbContextOptionsBuilder<HotelDbContext>()
                    .UseSqlite($"Data Source={dbPath}")
                    .Options;

                // Create and seed the database if it doesn't exist
                InitializeDatabase(options);

                // Create main window
                var mainWindow = new MainWindow();
                
                // Create view model
                var viewModel = new MainWindowViewModel(options);
                
                // Set data context
                mainWindow.DataContext = viewModel;
                
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private async void InitializeDatabase(DbContextOptions<HotelDbContext> options)
        {
            using (var db = new HotelDbContext(options))
            {
                var seeder = new DataSeeder(db);
                await seeder.SeedAsync();
            }
        }
    }
} 