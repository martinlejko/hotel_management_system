using Avalonia.Controls;
using HotelManagementSystem.Core.Models;
using System;
using System.Windows.Input;

namespace HotelManagementSystem.App.ViewModels
{
    public class CustomerDialogViewModel : ViewModelBase
    {
        private readonly Window _dialog;
        private readonly Customer? _originalCustomer;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;
        private string? _phoneNumber;
        private string? _address;
        private DateTime? _dateOfBirth;
        
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }
        
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
        
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
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
        
        public CustomerDialogViewModel(Window dialog, Customer? customer = null)
        {
            _dialog = dialog;
            _originalCustomer = customer;
            
            // If editing an existing customer, populate the fields
            if (customer != null)
            {
                FirstName = customer.FirstName;
                LastName = customer.LastName;
                Email = customer.Email;
                PhoneNumber = customer.PhoneNumber;
                Address = customer.Address;
                DateOfBirth = customer.DateOfBirth;
            }
            
            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());
        }
        
        private void Save()
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(Email))
            {
                // In a real app, show an error message
                return;
            }
            
            // Create or update the customer
            Customer customer = _originalCustomer ?? new Customer();
            customer.FirstName = FirstName;
            customer.LastName = LastName;
            customer.Email = Email;
            customer.PhoneNumber = PhoneNumber;
            customer.Address = Address;
            customer.DateOfBirth = DateOfBirth;
            
            // Close the dialog with the customer as result
            _dialog.Close(customer);
        }
        
        private void Cancel()
        {
            _dialog.Close(null);
        }
    }
} 