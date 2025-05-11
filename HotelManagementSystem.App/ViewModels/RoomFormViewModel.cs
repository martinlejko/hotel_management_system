using HotelManagementSystem.Core.Models;
using System.Windows.Input;

namespace HotelManagementSystem.App.ViewModels
{
    public class RoomFormViewModel : ViewModelBase
    {
        private readonly Room? _originalRoom;
        private string _roomNumber = string.Empty;
        private RoomType _selectedRoomType = RoomType.Single;
        private int _capacity = 1;
        private decimal _pricePerNight = 100;
        private string? _description;
        private bool _isAvailable = true;
        
        public event Action<Room>? SaveCompleted;
        public event Action? CancelRequested;
        
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
        
        public RoomType SelectedRoomType
        {
            get => _selectedRoomType;
            set => SetProperty(ref _selectedRoomType, value);
        }
        
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
        
        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(RoomNumber) && Capacity >= 1 && PricePerNight >= 0;
        }
        
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
        
        private void Cancel()
        {
            CancelRequested?.Invoke();
        }
    }
} 