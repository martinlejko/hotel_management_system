using HotelManagementSystem.Core.Data;
using HotelManagementSystem.Core.Models;
using HotelManagementSystem.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Threading;

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
            set 
            { 
                if (SetProperty(ref _selectedCustomer, value))
                {
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }
        
        public Room? SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                if (SetProperty(ref _selectedRoom, value))
                {
                    CalculateTotalPrice();
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
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
                    if (CheckOutDate <= CheckInDate)
                    {
                        CheckOutDate = CheckInDate.AddDays(1);
                    }
                    
                    CalculateTotalPrice();
                    LoadAvailableRoomsAsync().ConfigureAwait(false);
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
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
                    if (CheckOutDate <= CheckInDate)
                    {
                        CheckInDate = CheckOutDate.AddDays(-1);
                    }
                    
                    CalculateTotalPrice();
                    LoadAvailableRoomsAsync().ConfigureAwait(false);
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
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
            
            if (reservation != null)
            {
                CheckInDate = reservation.CheckInDate;
                CheckOutDate = reservation.CheckOutDate;
                SelectedStatus = reservation.Status;
                TotalPrice = reservation.TotalPrice;
                SpecialRequests = reservation.SpecialRequests;
            }
            
            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => Cancel());
            
            Task.Run(async () => await LoadInitialDataAsync());
        }
        
        private async Task LoadInitialDataAsync()
        {
            await LoadCustomersAsync();
            await LoadAvailableRoomsAsync();
        }
        
        private async Task LoadCustomersAsync()
        {
            try
            {
                using (var context = new HotelDbContext((DbContextOptions<HotelDbContext>)_dbOptions))
                {
                    var customerRepo = new CustomerRepository(context);
                    var customers = await customerRepo.GetAllAsync();
                    
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        Customers.Clear();
                        foreach (var customer in customers)
                        {
                            Customers.Add(customer);
                        }
                        
                        if (_originalReservation != null && _originalReservation.CustomerId > 0)
                        {
                            SelectedCustomer = Customers.FirstOrDefault(c => c.Id == _originalReservation.CustomerId);
                        }
                        else if (Customers.Count > 0)
                        {
                            SelectedCustomer = Customers[0];
                        }
                        
                        (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading customers: {ex.Message}");
            }
        }
        
        private async Task LoadAvailableRoomsAsync()
        {
            try
            {
                using (var context = new HotelDbContext((DbContextOptions<HotelDbContext>)_dbOptions))
                {
                    var roomRepo = new RoomRepository(context);
                    var reservationRepo = new ReservationRepository(context);
                    
                    var allRooms = await roomRepo.GetAllAsync();
                    
                    var bookedRoomIds = await reservationRepo.GetBookedRoomIdsAsync(
                        CheckInDate, 
                        CheckOutDate,
                        _originalReservation?.Id ?? 0);
                    
                    var availableRooms = allRooms.Where(r => 
                        r.IsAvailable && 
                        !bookedRoomIds.Contains(r.Id)).ToList();
                    
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        AvailableRooms.Clear();
                        foreach (var room in availableRooms)
                        {
                            AvailableRooms.Add(room);
                        }
                        
                        if (_originalReservation != null && _originalReservation.RoomId > 0)
                        {
                            var currentRoom = allRooms.FirstOrDefault(r => r.Id == _originalReservation.RoomId);
                            if (currentRoom != null && !AvailableRooms.Contains(currentRoom))
                            {
                                AvailableRooms.Add(currentRoom);
                            }
                            
                            SelectedRoom = AvailableRooms.FirstOrDefault(r => r.Id == _originalReservation.RoomId);
                        }
                        else if (AvailableRooms.Count > 0)
                        {
                            SelectedRoom = AvailableRooms[0];
                        }
                        
                        CalculateTotalPrice();
                        (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading available rooms: {ex.Message}");
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
        
        private bool CanSave()
        {
            return SelectedCustomer != null && SelectedRoom != null && CheckOutDate > CheckInDate;
        }
        
        private void Save()
        {
            if (SelectedCustomer == null || SelectedRoom == null)
            {
                return;
            }
            
            Reservation reservation = _originalReservation ?? new Reservation();
            reservation.CustomerId = SelectedCustomer.Id;
            reservation.RoomId = SelectedRoom.Id;
            reservation.CheckInDate = CheckInDate;
            reservation.CheckOutDate = CheckOutDate;
            reservation.Status = SelectedStatus;
            reservation.TotalPrice = TotalPrice;
            reservation.SpecialRequests = SpecialRequests;
            
            if (_originalReservation != null)
            {
                reservation.Customer = SelectedCustomer;
                reservation.Room = SelectedRoom;
            }
            
            SaveCompleted?.Invoke(reservation);
        }
        
        private void Cancel()
        {
            CancelRequested?.Invoke();
        }
    }
} 