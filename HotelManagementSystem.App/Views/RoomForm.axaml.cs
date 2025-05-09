using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HotelManagementSystem.App.Views
{
    public partial class RoomForm : UserControl
    {
        public RoomForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
} 