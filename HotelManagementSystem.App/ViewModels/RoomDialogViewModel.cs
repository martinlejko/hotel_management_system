using Avalonia.Controls;
using HotelManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace HotelManagementSystem.App.ViewModels
{
    public class RoomDialogViewModel : ViewModelBase
    {
        private readonly Window _dialog;
        private readonly Room? _originalRoom;
        private string _roomNumber = string.Empty;
        private RoomType _selectedRoomType = RoomType.Single;
        private int _capacity = 1;
        private decimal _pricePerNight = 100;
        private string? _description;
        private bool _isAvailable = true;
        
        public string RoomNumber
        {
            get => _roomNumber;
            set => SetProperty(ref _roomNumber, value);
        }
        
        public RoomType SelectedRoomType
        {
            get => _selectedRoomType;
            set => SetProperty(ref _selectedRoomType, value);
        }
        
        public int Capacity
        {
            get => _capacity;
            set => SetProperty(ref _capacity, value);
        }
        
        public decimal PricePerNight
        {
            get => _pricePerNight;
            set => SetProperty(ref _pricePerNight, value);
        }
        
        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        
        public bool IsAvailable
        {
            get => _isAvailable;
            set => SetProperty(ref _isAvailable, value);
        }
        
        public IEnumerable<RoomType> RoomTypes => Enum.GetValues<RoomType>();
        
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        
        public RoomDialogViewModel(Window dialog, Room? room = null)
        {
            _dialog = dialog;
            _originalRoom = room;
            
            // If editing an existing room, populate the fields
            if (room != null)
            {
                RoomNumber = room.RoomNumber;
                SelectedRoomType = room.Type;
                Capacity = room.Capacity;
                PricePerNight = room.PricePerNight;
                Description = room.Description;
                IsAvailable = room.IsAvailable;
            }
            
            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());
        }
        
        private void Save()
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(RoomNumber))
            {
                // In a real app, show an error message
                return;
            }
            
            // Create or update the room
            Room room = _originalRoom ?? new Room();
            room.RoomNumber = RoomNumber;
            room.Type = SelectedRoomType;
            room.Capacity = Capacity;
            room.PricePerNight = PricePerNight;
            room.Description = Description;
            room.IsAvailable = IsAvailable;
            
            // Close the dialog with the room as result
            _dialog.Close(room);
        }
        
        private void Cancel()
        {
            _dialog.Close(null);
        }
    }
} 