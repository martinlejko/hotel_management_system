using HotelManagementSystem.Core.Data;
using HotelManagementSystem.Core.Models;
using HotelManagementSystem.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelManagementSystem.App.ViewModels
{
    public class ReservationFormViewModel : ViewModelBase
    {
        private readonly DbContextOptions _dbOptions;
        private readonly Reservation? _originalReservation;
        
        private ObservableCollection<Customer> _customers = new();
        private ObservableCollection<Room> _availableRooms = new();
        private Customer? _selectedCustomer;
        private Room? _selectedRoom;
        private DateTime _checkInDate = DateTime.Today;
        private DateTime _checkOutDate = DateTime.Today.AddDays(1);
        private ReservationStatus _selectedStatus = ReservationStatus.Confirmed;
        private decimal _totalPrice;
        private string? _specialRequests;
        
        public event Action<Reservation>? SaveCompleted;
        public event Action? CancelRequested;
        
        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set => SetProperty(ref _customers, value);
        }
        
        public ObservableCollection<Room> AvailableRooms
        {
            get => _availableRooms;
            set => SetProperty(ref _availableRooms, value);
        }
        
        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set => SetProperty(ref _selectedCustomer, value);
        }
        
        public Room? SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                if (SetProperty(ref _selectedRoom, value))
                {
                    CalculateTotalPrice();
                }
            }
        }
        
        public DateTime CheckInDate
        {
            get => _checkInDate;
            set
            {
                if (SetProperty(ref _checkInDate, value))
                {
                    // Ensure check-out date is after check-in date
                    if (CheckOutDate <= CheckInDate)
                    {
                        CheckOutDate = CheckInDate.AddDays(1);
                    }
                    
                    CalculateTotalPrice();
                    LoadAvailableRoomsAsync().ConfigureAwait(false);
                }
            }
        }
        
        public DateTime CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                if (SetProperty(ref _checkOutDate, value))
                {
                    // Ensure check-out date is after check-in date
                    if (CheckOutDate <= CheckInDate)
                    {
                        CheckInDate = CheckOutDate.AddDays(-1);
                    }
                    
                    CalculateTotalPrice();
                    LoadAvailableRoomsAsync().ConfigureAwait(false);
                }
            }
        }
        
        public ReservationStatus SelectedStatus
        {
            get => _selectedStatus;
            set => SetProperty(ref _selectedStatus, value);
        }
        
        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }
        
        public string? SpecialRequests
        {
            get => _specialRequests;
            set => SetProperty(ref _specialRequests, value);
        }
        
        public Array ReservationStatuses => Enum.GetValues(typeof(ReservationStatus));
        
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        
        public ReservationFormViewModel(DbContextOptions dbOptions, Reservation? reservation = null)
        {
            _dbOptions = dbOptions;
            _originalReservation = reservation;
            
            // If editing an existing reservation, populate the fields
            if (reservation != null)
            {
                CheckInDate = reservation.CheckInDate;
                CheckOutDate = reservation.CheckOutDate;
                SelectedStatus = reservation.Status;
                TotalPrice = reservation.TotalPrice;
                SpecialRequests = reservation.SpecialRequests;
            }
            
            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());
            
            // Load data
            LoadCustomersAsync().ConfigureAwait(false);
            LoadAvailableRoomsAsync().ConfigureAwait(false);
        }
        
        private async Task LoadCustomersAsync()
        {
            using (var context = new HotelDbContext((DbContextOptions<HotelDbContext>)_dbOptions))
            {
                var customerRepo = new CustomerRepository(context);
                var customers = await customerRepo.GetAllAsync();
                
                Customers.Clear();
                foreach (var customer in customers)
                {
                    Customers.Add(customer);
                }
                
                // Set selected customer if editing
                if (_originalReservation != null && _originalReservation.CustomerId > 0)
                {
                    SelectedCustomer = Customers.FirstOrDefault(c => c.Id == _originalReservation.CustomerId);
                }
                else if (Customers.Count > 0)
                {
                    SelectedCustomer = Customers[0];
                }
            }
        }
        
        private async Task LoadAvailableRoomsAsync()
        {
            using (var context = new HotelDbContext((DbContextOptions<HotelDbContext>)_dbOptions))
            {
                var roomRepo = new RoomRepository(context);
                var reservationRepo = new ReservationRepository(context);
                
                // Get all rooms
                var allRooms = await roomRepo.GetAllAsync();
                
                // Get rooms that are already booked for the selected date range
                var bookedRoomIds = await reservationRepo.GetBookedRoomIdsAsync(
                    CheckInDate, 
                    CheckOutDate,
                    _originalReservation?.Id ?? 0); // Exclude current reservation if editing
                
                // Filter available rooms
                var availableRooms = allRooms.Where(r => 
                    r.IsAvailable && 
                    !bookedRoomIds.Contains(r.Id)).ToList();
                
                AvailableRooms.Clear();
                foreach (var room in availableRooms)
                {
                    AvailableRooms.Add(room);
                }
                
                // Set selected room if editing
                if (_originalReservation != null && _originalReservation.RoomId > 0)
                {
                    // For editing, always include the original room in the list
                    if (!AvailableRooms.Any(r => r.Id == _originalReservation.RoomId))
                    {
                        var originalRoom = await roomRepo.GetByIdAsync(_originalReservation.RoomId);
                        if (originalRoom != null)
                        {
                            AvailableRooms.Add(originalRoom);
                        }
                    }
                    
                    SelectedRoom = AvailableRooms.FirstOrDefault(r => r.Id == _originalReservation.RoomId);
                }
                else if (AvailableRooms.Count > 0)
                {
                    SelectedRoom = AvailableRooms[0];
                }
            }
        }
        
        private void CalculateTotalPrice()
        {
            if (SelectedRoom != null)
            {
                int days = (CheckOutDate - CheckInDate).Days;
                if (days > 0)
                {
                    TotalPrice = SelectedRoom.PricePerNight * days;
                }
            }
        }
        
        private void Save()
        {
            // Validate input
            if (SelectedCustomer == null || SelectedRoom == null || CheckOutDate <= CheckInDate)
            {
                // In a real app, show an error message
                return;
            }
            
            // Create or update the reservation
            Reservation reservation = _originalReservation ?? new Reservation();
            reservation.CustomerId = SelectedCustomer.Id;
            reservation.RoomId = SelectedRoom.Id;
            reservation.CheckInDate = CheckInDate;
            reservation.CheckOutDate = CheckOutDate;
            reservation.Status = SelectedStatus;
            reservation.TotalPrice = TotalPrice;
            reservation.SpecialRequests = SpecialRequests;
            
            if (_originalReservation == null)
            {
                reservation.CreatedAt = DateTime.Now;
            }
            
            // Notify that save is completed
            SaveCompleted?.Invoke(reservation);
        }
        
        private void Cancel()
        {
            CancelRequested?.Invoke();
        }
    }
} 