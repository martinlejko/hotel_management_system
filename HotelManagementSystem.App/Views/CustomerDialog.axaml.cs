using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HotelManagementSystem.App.ViewModels;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.App.Views
{
    public partial class CustomerDialog : Window
    {
        public CustomerDialog()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public CustomerDialog(Customer? customer = null) : this()
        {
            DataContext = new CustomerDialogViewModel(this, customer);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
} 