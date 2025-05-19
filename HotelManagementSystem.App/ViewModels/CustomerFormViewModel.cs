using HotelManagementSystem.Core.Models;
using System.Windows.Input;

namespace HotelManagementSystem.App.ViewModels
{
    /// <summary>
    /// View model for the customer form.
    /// Handles the data binding and logic for creating or editing customer information.
    /// </summary>
    public class CustomerFormViewModel : ViewModelBase
    {
        private readonly Customer? _originalCustomer;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;
        private string? _phoneNumber;
        private string? _address;
        private DateTime? _dateOfBirth;
        
        /// <summary>
        /// Event raised when a customer is successfully saved.
        /// </summary>
        public event Action<Customer>? SaveCompleted;
        
        /// <summary>
        /// Event raised when the user cancels the operation.
        /// </summary>
        public event Action? CancelRequested;
        
        /// <summary>
        /// Gets or sets the customer's first name.
        /// </summary>
        public string FirstName
        {
            get => _firstName;
            set 
            { 
                if (SetProperty(ref _firstName, value))
                {
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the customer's last name.
        /// </summary>
        public string LastName
        {
            get => _lastName;
            set 
            { 
                if (SetProperty(ref _lastName, value))
                {
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the customer's email address.
        /// </summary>
        public string Email
        {
            get => _email;
            set 
            { 
                if (SetProperty(ref _email, value))
                {
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the customer's phone number.
        /// </summary>
        public string? PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }
        
        /// <summary>
        /// Gets or sets the customer's address.
        /// </summary>
        public string? Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
        
        /// <summary>
        /// Gets or sets the customer's date of birth.
        /// </summary>
        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set => SetProperty(ref _dateOfBirth, value);
        }
        
        /// <summary>
        /// Gets the command to save the customer information.
        /// </summary>
        public ICommand SaveCommand { get; }
        
        /// <summary>
        /// Gets the command to cancel the form operation.
        /// </summary>
        public ICommand CancelCommand { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerFormViewModel"/> class.
        /// </summary>
        /// <param name="customer">Optional existing customer for editing. If null, a new customer will be created.</param>
        public CustomerFormViewModel(Customer? customer = null)
        {
            _originalCustomer = customer;
            
            if (customer != null)
            {
                FirstName = customer.FirstName;
                LastName = customer.LastName;
                Email = customer.Email;
                PhoneNumber = customer.PhoneNumber;
                Address = customer.Address;
                DateOfBirth = customer.DateOfBirth;
            }
            
            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => Cancel());
        }
        
        /// <summary>
        /// Determines whether the save command can be executed.
        /// </summary>
        /// <returns>True if all required fields have values; otherwise, false.</returns>
        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(FirstName) && 
                   !string.IsNullOrWhiteSpace(LastName) && 
                   !string.IsNullOrWhiteSpace(Email);
        }
        
        /// <summary>
        /// Saves the customer information.
        /// Creates a new customer object or updates an existing one and raises the SaveCompleted event.
        /// </summary>
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(Email))
            {
                return;
            }
            
            Customer customer = _originalCustomer ?? new Customer();
            customer.FirstName = FirstName;
            customer.LastName = LastName;
            customer.Email = Email;
            customer.PhoneNumber = PhoneNumber;
            customer.Address = Address;
            customer.DateOfBirth = DateOfBirth;
            
            if (_originalCustomer == null)
            {
                customer.CreatedAt = DateTime.Now;
            }
            
            SaveCompleted?.Invoke(customer);
        }
        
        /// <summary>
        /// Cancels the form operation.
        /// </summary>
        private void Cancel()
        {
            CancelRequested?.Invoke();
        }
    }
} 