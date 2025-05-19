using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HotelManagementSystem.App.Views
{
    /// <summary>
    /// Main window of the application that hosts all UI components.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the XAML components of the window.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        /// <summary>
        /// Handles the DataGrid selection changed event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void DataGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
        }
    }
} 