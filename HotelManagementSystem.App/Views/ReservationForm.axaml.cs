using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HotelManagementSystem.App.Views
{
    public partial class ReservationForm : UserControl
    {
        public ReservationForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
} 