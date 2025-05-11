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
                var options = CreateDatabaseOptions();

                InitializeDatabase(options);

                var mainWindow = new MainWindow();
                
                var viewModel = new MainWindowViewModel(options);
                
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