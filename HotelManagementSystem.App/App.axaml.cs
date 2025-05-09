using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HotelManagementSystem.App.Services;
using HotelManagementSystem.App.ViewModels;
using HotelManagementSystem.App.Views;
using HotelManagementSystem.Core.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

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
                using (var db = new HotelDbContext(options))
                {
                    db.Database.EnsureCreated();
                }

                // Create main window
                var mainWindow = new MainWindow();
                
                // Create view model
                var viewModel = new MainWindowViewModel(options);
                
                // Initialize dialog service
                viewModel.DialogService = new DialogService(mainWindow);
                
                // Set data context
                mainWindow.DataContext = viewModel;
                
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
} 