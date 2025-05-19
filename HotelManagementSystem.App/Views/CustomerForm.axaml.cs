using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using HotelManagementSystem.Core.Validation;

namespace HotelManagementSystem.App.Views
{
    /// <summary>
    /// User control for creating and editing customer information.
    /// Provides input validation for customer properties.
    /// </summary>
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
        
        private bool _isEmailValid = true;
        private bool _isPhoneValid = true;
        private bool _isFirstNameValid = true;
        private bool _isLastNameValid = true;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerForm"/> class.
        /// Sets up control references and event handlers for input validation.
        /// </summary>
        public CustomerForm()
        {
            InitializeComponent();
            
            _emailTextBox = this.FindControl<TextBox>("EmailTextBox");
            _phoneTextBox = this.FindControl<TextBox>("PhoneTextBox");
            _firstNameTextBox = this.FindControl<TextBox>("FirstNameTextBox");
            _lastNameTextBox = this.FindControl<TextBox>("LastNameTextBox");
            
            _emailErrorText = this.FindControl<TextBlock>("EmailErrorText");
            _phoneErrorText = this.FindControl<TextBlock>("PhoneErrorText");
            _firstNameErrorText = this.FindControl<TextBlock>("FirstNameErrorText");
            _lastNameErrorText = this.FindControl<TextBlock>("LastNameErrorText");
            
            _saveButton = this.FindControl<Button>("SaveButton");
            
            if (_emailTextBox != null)
            {
                _emailTextBox.TextChanged += (s, e) => ValidateEmail();
            }
            
            if (_phoneTextBox != null)
            {
                _phoneTextBox.TextChanged += (s, e) => ValidatePhone();
            }
            
            if (_firstNameTextBox != null)
            {
                _firstNameTextBox.TextChanged += (s, e) => ValidateFirstName();
            }
            
            if (_lastNameTextBox != null)
            {
                _lastNameTextBox.TextChanged += (s, e) => ValidateLastName();
            }
            
            ValidateAllFields();
        }

        /// <summary>
        /// Initializes the XAML components of the control.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        /// <summary>
        /// Validates the email input.
        /// Email must be a properly formatted email address.
        /// </summary>
        private void ValidateEmail()
        {
            if (_emailTextBox == null || _emailErrorText == null) return;
            
            string email = _emailTextBox.Text ?? string.Empty;
            bool isValid = !string.IsNullOrWhiteSpace(email) && ValidationHelper.IsValidEmail(email);
            _isEmailValid = isValid;
            
            UpdateValidationVisuals(_emailTextBox, _emailErrorText, isValid, "Please enter a valid email address");
            UpdateSaveButtonState();
        }
        
        /// <summary>
        /// Validates the phone number input.
        /// Phone number must be properly formatted if provided, but is optional.
        /// </summary>
        private void ValidatePhone()
        {
            if (_phoneTextBox == null || _phoneErrorText == null) return;
            
            string phone = _phoneTextBox.Text ?? string.Empty;
            
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
        
        /// <summary>
        /// Validates the first name input.
        /// First name is required and cannot be empty.
        /// </summary>
        private void ValidateFirstName()
        {
            if (_firstNameTextBox == null || _firstNameErrorText == null) return;
            
            string firstName = _firstNameTextBox.Text ?? string.Empty;
            bool isValid = !string.IsNullOrWhiteSpace(firstName);
            _isFirstNameValid = isValid;
            
            UpdateValidationVisuals(_firstNameTextBox, _firstNameErrorText, isValid, "First name is required");
            UpdateSaveButtonState();
        }
        
        /// <summary>
        /// Validates the last name input.
        /// Last name is required and cannot be empty.
        /// </summary>
        private void ValidateLastName()
        {
            if (_lastNameTextBox == null || _lastNameErrorText == null) return;
            
            string lastName = _lastNameTextBox.Text ?? string.Empty;
            bool isValid = !string.IsNullOrWhiteSpace(lastName);
            _isLastNameValid = isValid;
            
            UpdateValidationVisuals(_lastNameTextBox, _lastNameErrorText, isValid, "Last name is required");
            UpdateSaveButtonState();
        }
        
        /// <summary>
        /// Validates all input fields in the form.
        /// </summary>
        private void ValidateAllFields()
        {
            ValidateFirstName();
            ValidateLastName();
            ValidateEmail();
            ValidatePhone();
        }
        
        /// <summary>
        /// Updates the visual state of an input field based on validation result.
        /// Adds or removes the "invalid" class and sets appropriate error messages.
        /// </summary>
        /// <param name="textBox">The text box to update.</param>
        /// <param name="errorText">The error text block to update.</param>
        /// <param name="isValid">Whether the input is valid.</param>
        /// <param name="errorMessage">The error message to display if invalid.</param>
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
        
        /// <summary>
        /// Updates the save button's enabled state based on the validation state of all inputs.
        /// Disables the button and shows tooltip if there are validation errors.
        /// </summary>
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