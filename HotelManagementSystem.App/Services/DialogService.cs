using Avalonia.Controls;
using System;
using System.Threading.Tasks;

namespace HotelManagementSystem.App.Services
{
    public class DialogService
    {
        private readonly Window _parentWindow;

        public DialogService(Window parentWindow)
        {
            _parentWindow = parentWindow ?? throw new ArgumentNullException(nameof(parentWindow));
        }

        public async Task<bool> ShowConfirmationAsync(string title, string message)
        {
            var dialog = new Window
            {
                Title = title,
                Width = 350,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Content = new StackPanel
                {
                    Margin = new Avalonia.Thickness(20),
                    Children =
                    {
                        new TextBlock
                        {
                            Text = message,
                            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                            Margin = new Avalonia.Thickness(0, 0, 0, 20)
                        },
                        new StackPanel
                        {
                            Orientation = Avalonia.Layout.Orientation.Horizontal,
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                            Spacing = 10,
                            Children =
                            {
                                new Button
                                {
                                    Content = "Cancel",
                                    Width = 100,
                                    Tag = false
                                },
                                new Button
                                {
                                    Content = "Delete",
                                    Width = 100,
                                    Background = Avalonia.Media.Brushes.Red,
                                    Foreground = Avalonia.Media.Brushes.White,
                                    Tag = true
                                }
                            }
                        }
                    }
                }
            };

            var result = false;

            // Add event handlers to buttons
            foreach (var child in ((StackPanel)((StackPanel)dialog.Content!).Children[1]).Children)
            {
                if (child is Button button)
                {
                    button.Click += (sender, e) =>
                    {
                        if (sender is Button clickedButton && clickedButton.Tag is bool boolValue)
                        {
                            result = boolValue;
                            dialog.Close();
                        }
                    };
                }
            }

            // Show dialog
            await dialog.ShowDialog(_parentWindow);
            return result;
        }
    }
} 