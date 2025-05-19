using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HotelManagementSystem.App.Views
{
    /// <summary>
    /// User control for creating and editing reservations.
    /// </summary>
    public partial class ReservationForm : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationForm"/> class.
        /// </summary>
        public ReservationForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the XAML components of the control.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
} 