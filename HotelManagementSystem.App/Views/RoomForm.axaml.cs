using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HotelManagementSystem.Core.Validation;

namespace HotelManagementSystem.App.Views
{
    public partial class RoomForm : UserControl
    {
        private TextBox? _roomNumberTextBox;
        private TextBlock? _roomNumberErrorText;
        private NumericUpDown? _priceTextBox;
        private TextBlock? _priceErrorText;
        private NumericUpDown? _capacityTextBox;
        private TextBlock? _capacityErrorText;
        private Button? _saveButton;
        
        private bool _isRoomNumberValid = true;
        private bool _isPriceValid = true;
        private bool _isCapacityValid = true;
        
        public RoomForm()
        {
            InitializeComponent();
            
            _roomNumberTextBox = this.FindControl<TextBox>("RoomNumberTextBox");
            _roomNumberErrorText = this.FindControl<TextBlock>("RoomNumberErrorText");
            _priceTextBox = this.FindControl<NumericUpDown>("PriceTextBox");
            _priceErrorText = this.FindControl<TextBlock>("PriceErrorText");
            _capacityTextBox = this.FindControl<NumericUpDown>("CapacityTextBox");
            _capacityErrorText = this.FindControl<TextBlock>("CapacityErrorText");
            _saveButton = this.FindControl<Button>("SaveButton");
            
            if (_roomNumberTextBox != null)
            {
                _roomNumberTextBox.TextChanged += (s, e) => ValidateRoomNumber();
            }
            
            if (_priceTextBox != null)
            {
                _priceTextBox.ValueChanged += (s, e) => 
                {
                    if (_priceTextBox.Value == null)
                    {
                        _priceTextBox.Value = 100;
                    }
                    ValidatePrice();
                };
            }
            
            if (_capacityTextBox != null)
            {
                _capacityTextBox.ValueChanged += (s, e) => 
                {
                    if (_capacityTextBox.Value == null)
                    {
                        _capacityTextBox.Value = 1;
                    }
                    ValidateCapacity();
                };
            }
            
            ValidateRoomNumber();
            ValidatePrice();
            ValidateCapacity();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
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
        
        private void ValidatePrice()
        {
            if (_priceTextBox == null || _priceErrorText == null) return;
            
            decimal? rawValue = _priceTextBox.Value;
            decimal price = rawValue ?? 0;
            
            bool isValid = price >= 0;
            
            _isPriceValid = isValid;
            
            if (isValid)
            {
                _priceTextBox.Classes.Remove("invalid");
                ToolTip.SetTip(_priceTextBox, null);
                _priceErrorText.Text = string.Empty;
            }
            else
            {
                _priceTextBox.Classes.Add("invalid");
                string errorMessage = "Price must be a positive value";
                ToolTip.SetTip(_priceTextBox, errorMessage);
                _priceErrorText.Text = errorMessage;
                
                if (!rawValue.HasValue || price < 0)
                {
                    _priceTextBox.Value = 0;
                }
            }
            
            UpdateSaveButtonState();
        }
        
        private void ValidateCapacity()
        {
            if (_capacityTextBox == null || _capacityErrorText == null) return;
            
            decimal? rawValue = _capacityTextBox.Value;
            int capacity = rawValue.HasValue ? (int)rawValue.Value : 0;
            
            bool isValid = capacity >= 1;
            
            _isCapacityValid = isValid;
            
            if (isValid)
            {
                _capacityTextBox.Classes.Remove("invalid");
                ToolTip.SetTip(_capacityTextBox, null);
                _capacityErrorText.Text = string.Empty;
            }
            else
            {
                _capacityTextBox.Classes.Add("invalid");
                string errorMessage = "Capacity must be at least 1";
                ToolTip.SetTip(_capacityTextBox, errorMessage);
                _capacityErrorText.Text = errorMessage;
                
                if (!rawValue.HasValue)
                {
                    _capacityTextBox.Value = 1;
                }
            }
            
            UpdateSaveButtonState();
        }
        
        private void UpdateSaveButtonState()
        {
            if (_saveButton == null) return;
            
            bool isFormValid = _isRoomNumberValid && _isPriceValid && _isCapacityValid;
            _saveButton.IsEnabled = isFormValid;
            
            if (!isFormValid)
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