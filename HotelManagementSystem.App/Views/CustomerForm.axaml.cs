using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using HotelManagementSystem.Core.Validation;
using System;

namespace HotelManagementSystem.App.Views
{
    public partial class CustomerForm : UserControl
    {
        private TextBox? _emailTextBox;
        private TextBox? _phoneTextBox;
        private TextBox? _firstNameTextBox;
        private TextBox? _lastNameTextBox;
        
        private TextBlock? _emailErrorText;
        private TextBlock? _phoneErrorText;
        private TextBlock? _firstNameErrorText;
        private TextBlock? _lastNameErrorText;
        
        private Button? _saveButton;
        
        // Track validation state for each field
        private bool _isEmailValid = true;
        private bool _isPhoneValid = true;
        private bool _isFirstNameValid = true;
        private bool _isLastNameValid = true;
        
        public CustomerForm()
        {
            InitializeComponent();
            
            // Find controls after initialization
            _emailTextBox = this.FindControl<TextBox>("EmailTextBox");
            _phoneTextBox = this.FindControl<TextBox>("PhoneTextBox");
            _firstNameTextBox = this.FindControl<TextBox>("FirstNameTextBox");
            _lastNameTextBox = this.FindControl<TextBox>("LastNameTextBox");
            
            _emailErrorText = this.FindControl<TextBlock>("EmailErrorText");
            _phoneErrorText = this.FindControl<TextBlock>("PhoneErrorText");
            _firstNameErrorText = this.FindControl<TextBlock>("FirstNameErrorText");
            _lastNameErrorText = this.FindControl<TextBlock>("LastNameErrorText");
            
            _saveButton = this.FindControl<Button>("SaveButton");
            
            // Set up validation
            if (_emailTextBox != null)
            {
                _emailTextBox.TextChanged += (s, e) => ValidateEmail();
                _emailTextBox.LostFocus += (s, e) => ValidateEmail();
            }
            
            if (_phoneTextBox != null)
            {
                _phoneTextBox.TextChanged += (s, e) => ValidatePhone();
                _phoneTextBox.LostFocus += (s, e) => ValidatePhone();
            }
            
            if (_firstNameTextBox != null)
            {
                _firstNameTextBox.TextChanged += (s, e) => ValidateFirstName();
                _firstNameTextBox.LostFocus += (s, e) => ValidateFirstName();
            }
            
            if (_lastNameTextBox != null)
            {
                _lastNameTextBox.TextChanged += (s, e) => ValidateLastName();
                _lastNameTextBox.LostFocus += (s, e) => ValidateLastName();
            }
            
            // Perform initial validation
            ValidateAllFields();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void ValidateEmail()
        {
            if (_emailTextBox == null || _emailErrorText == null) return;
            
            string email = _emailTextBox.Text ?? string.Empty;
            bool isValid = !string.IsNullOrWhiteSpace(email) && ValidationHelper.IsValidEmail(email);
            _isEmailValid = isValid;
            
            UpdateValidationVisuals(_emailTextBox, _emailErrorText, isValid, "Please enter a valid email address");
            UpdateSaveButtonState();
        }
        
        private void ValidatePhone()
        {
            if (_phoneTextBox == null || _phoneErrorText == null) return;
            
            string phone = _phoneTextBox.Text ?? string.Empty;
            
            // Phone is optional, so empty is valid
            if (string.IsNullOrWhiteSpace(phone))
            {
                UpdateValidationVisuals(_phoneTextBox, _phoneErrorText, true, string.Empty);
                _isPhoneValid = true;
                UpdateSaveButtonState();
                return;
            }
            
            bool isValid = ValidationHelper.IsValidPhoneNumber(phone);
            _isPhoneValid = isValid;
            
            UpdateValidationVisuals(_phoneTextBox, _phoneErrorText, isValid, "Please enter a valid phone number");
            UpdateSaveButtonState();
        }
        
        private void ValidateFirstName()
        {
            if (_firstNameTextBox == null || _firstNameErrorText == null) return;
            
            string firstName = _firstNameTextBox.Text ?? string.Empty;
            bool isValid = !string.IsNullOrWhiteSpace(firstName);
            _isFirstNameValid = isValid;
            
            UpdateValidationVisuals(_firstNameTextBox, _firstNameErrorText, isValid, "First name is required");
            UpdateSaveButtonState();
        }
        
        private void ValidateLastName()
        {
            if (_lastNameTextBox == null || _lastNameErrorText == null) return;
            
            string lastName = _lastNameTextBox.Text ?? string.Empty;
            bool isValid = !string.IsNullOrWhiteSpace(lastName);
            _isLastNameValid = isValid;
            
            UpdateValidationVisuals(_lastNameTextBox, _lastNameErrorText, isValid, "Last name is required");
            UpdateSaveButtonState();
        }
        
        private void ValidateAllFields()
        {
            ValidateFirstName();
            ValidateLastName();
            ValidateEmail();
            ValidatePhone();
        }
        
        private void UpdateValidationVisuals(TextBox textBox, TextBlock errorText, bool isValid, string errorMessage)
        {
            if (isValid)
            {
                textBox.Classes.Remove("invalid");
                ToolTip.SetTip(textBox, null);
                errorText.Text = string.Empty;
            }
            else
            {
                textBox.Classes.Add("invalid");
                ToolTip.SetTip(textBox, errorMessage);
                errorText.Text = errorMessage;
            }
        }
        
        private void UpdateSaveButtonState()
        {
            if (_saveButton == null) return;
            
            bool isFormValid = _isFirstNameValid && _isLastNameValid && _isEmailValid && _isPhoneValid;
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