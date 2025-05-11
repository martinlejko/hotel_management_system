using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using HotelManagementSystem.Core.Validation;
using System;

namespace HotelManagementSystem.App.Views
{
    public partial class RoomForm : UserControl
    {
        private TextBox? _roomNumberTextBox;
        private TextBlock? _roomNumberErrorText;
        private Button? _saveButton;
        
        private bool _isRoomNumberValid = true;
        
        public RoomForm()
        {
            InitializeComponent();
            
            _roomNumberTextBox = this.FindControl<TextBox>("RoomNumberTextBox");
            _roomNumberErrorText = this.FindControl<TextBlock>("RoomNumberErrorText");
            _saveButton = this.FindControl<Button>("SaveButton");
            
            if (_roomNumberTextBox != null)
            {
                _roomNumberTextBox.TextChanged += (s, e) => ValidateRoomNumber();
                _roomNumberTextBox.LostFocus += (s, e) => ValidateRoomNumber();
            }
            
            ValidateRoomNumber();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void ValidateRoomNumber()
        {
            if (_roomNumberTextBox == null || _roomNumberErrorText == null) return;
            
            string roomNumber = _roomNumberTextBox.Text ?? string.Empty;
            
            bool isValid = !string.IsNullOrWhiteSpace(roomNumber) && ValidationHelper.ContainsOnlyDigits(roomNumber);
            
            _isRoomNumberValid = isValid;
            
            if (isValid)
            {
                _roomNumberTextBox.Classes.Remove("invalid");
                ToolTip.SetTip(_roomNumberTextBox, null);
                _roomNumberErrorText.Text = string.Empty;
            }
            else
            {
                _roomNumberTextBox.Classes.Add("invalid");
                string errorMessage = "Room number must contain only digits";
                ToolTip.SetTip(_roomNumberTextBox, errorMessage);
                _roomNumberErrorText.Text = errorMessage;
            }
            
            UpdateSaveButtonState();
        }
        
        private void UpdateSaveButtonState()
        {
            if (_saveButton == null) return;
            
            _saveButton.IsEnabled = _isRoomNumberValid;
            
            if (!_isRoomNumberValid)
            {
                ToolTip.SetTip(_saveButton, "Please correct the validation errors before saving");
            }
            else
            {
                ToolTip.SetTip(_saveButton, null);
            }
        }
    }
} 