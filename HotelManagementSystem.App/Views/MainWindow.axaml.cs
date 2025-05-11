using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HotelManagementSystem.App.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void DataGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
        }
    }
} 