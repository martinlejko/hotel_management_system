using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HotelManagementSystem.App.ViewModels;
using HotelManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.App.Views
{
    public partial class ReservationDialog : Window
    {
        public ReservationDialog()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public ReservationDialog(DbContextOptions dbOptions, Reservation? reservation = null) : this()
        {
            DataContext = new ReservationDialogViewModel(this, dbOptions, reservation);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
} 