using HotelManagementSystem.Core.Models;
using System.Windows.Input;

namespace HotelManagementSystem.App.ViewModels
{
    /// <summary>
    /// View model for the room form.
    /// Handles the data binding and logic for creating or editing room information.
    /// </summary>
    public class RoomFormViewModel : ViewModelBase
    {
        private readonly Room? _originalRoom;
        private string _roomNumber = string.Empty;
        private RoomType _selectedRoomType = RoomType.Single;
        private int _capacity = 1;
        private decimal _pricePerNight = 100;
        private string? _description;
        private bool _isAvailable = true;
        
        /// <summary>
        /// Event raised when a room is successfully saved.
        /// </summary>
        public event Action<Room>? SaveCompleted;
        
        /// <summary>
        /// Event raised when the user cancels the operation.
        /// </summary>
        public event Action? CancelRequested;
        
        /// <summary>
        /// Gets or sets the room number.
        /// </summary>
        public string RoomNumber
        {
            get => _roomNumber;
            set 
            { 
                if (SetProperty(ref _roomNumber, value))
                {
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the selected room type.
        /// </summary>
        public RoomType SelectedRoomType
        {
            get => _selectedRoomType;
            set => SetProperty(ref _selectedRoomType, value);
        }
        
        /// <summary>
        /// Gets or sets the room capacity (number of guests).
        /// Enforces a minimum value of 1.
        /// </summary>
        public int Capacity
        {
            get => _capacity;
            set 
            { 
                int validValue = value < 1 ? 1 : value;
                if (SetProperty(ref _capacity, validValue))
                {
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the price per night for the room.
        /// </summary>
        public decimal PricePerNight
        {
            get => _pricePerNight;
            set 
            { 
                if (SetProperty(ref _pricePerNight, value))
                {
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the description of the room.
        /// </summary>
        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether the room is available for booking.
        /// </summary>
        public bool IsAvailable
        {
            get => _isAvailable;
            set => SetProperty(ref _isAvailable, value);
        }
        
        /// <summary>
        /// Gets a collection of all available room types for selection.
        /// </summary>
        public IEnumerable<RoomType> RoomTypes => Enum.GetValues<RoomType>();
        
        /// <summary>
        /// Gets the command to save the room information.
        /// </summary>
        public ICommand SaveCommand { get; }
        
        /// <summary>
        /// Gets the command to cancel the form operation.
        /// </summary>
        public ICommand CancelCommand { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RoomFormViewModel"/> class.
        /// </summary>
        /// <param name="room">Optional existing room for editing. If null, a new room will be created.</param>
        public RoomFormViewModel(Room? room = null)
        {
            _originalRoom = room;
            
            if (room != null)
            {
                RoomNumber = room.RoomNumber;
                SelectedRoomType = room.Type;
                Capacity = room.Capacity < 1 ? 1 : room.Capacity;
                PricePerNight = room.PricePerNight;
                Description = room.Description;
                IsAvailable = room.IsAvailable;
            }
            
            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => Cancel());
        }
        
        /// <summary>
        /// Determines whether the save command can be executed.
        /// </summary>
        /// <returns>True if all required fields have valid values; otherwise, false.</returns>
        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(RoomNumber) && Capacity >= 1 && PricePerNight >= 0;
        }
        
        /// <summary>
        /// Saves the room information.
        /// Creates a new room object or updates an existing one and raises the SaveCompleted event.
        /// </summary>
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(RoomNumber) || Capacity < 1)
            {
                return;
            }
            
            Room room = _originalRoom ?? new Room();
            room.RoomNumber = RoomNumber;
            room.Type = SelectedRoomType;
            room.Capacity = Capacity;
            room.PricePerNight = PricePerNight;
            room.Description = Description;
            room.IsAvailable = IsAvailable;
            
            SaveCompleted?.Invoke(room);
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