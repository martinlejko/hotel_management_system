using HotelManagementSystem.Core.Models;
using System;
using System.Windows.Input;

namespace HotelManagementSystem.App.ViewModels
{
    public class CustomerFormViewModel : ViewModelBase
    {
        private readonly Customer? _originalCustomer;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;
        private string? _phoneNumber;
        private string? _address;
        private DateTime? _dateOfBirth;
        
        public event Action<Customer>? SaveCompleted;
        public event Action? CancelRequested;
        
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
        
        public string? PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }
        
        public string? Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
        
        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set => SetProperty(ref _dateOfBirth, value);
        }
        
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        
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
        
        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(FirstName) && 
                   !string.IsNullOrWhiteSpace(LastName) && 
                   !string.IsNullOrWhiteSpace(Email);
        }
        
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
        
        private void Cancel()
        {
            CancelRequested?.Invoke();
        }
    }
} 