using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HotelManagementSystem.App.ViewModels;

namespace HotelManagementSystem.App.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void DataGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            // This method is just to ensure the selection binding works properly
            // The actual logic is handled in the view model's property setters
        }
    }
} 