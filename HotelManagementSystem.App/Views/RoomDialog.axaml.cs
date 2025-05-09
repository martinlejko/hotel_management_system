using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HotelManagementSystem.App.ViewModels;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.App.Views
{
    public partial class RoomDialog : Window
    {
        public RoomDialog()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public RoomDialog(Room? room = null) : this()
        {
            DataContext = new RoomDialogViewModel(this, room);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
} 