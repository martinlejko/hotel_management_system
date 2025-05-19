using HotelManagementSystem.Core.Data;
using HotelManagementSystem.Core.Models;
using HotelManagementSystem.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Threading;

namespace HotelManagementSystem.App.ViewModels
{
    /// <summary>
    /// View model for the reservation form.
    /// Handles the data binding and logic for creating or editing reservation information.
    /// </summary>
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
        
        /// <summary>
        /// Event raised when a reservation is successfully saved.
        /// </summary>
        public event Action<Reservation>? SaveCompleted;
        
        /// <summary>
        /// Event raised when the user cancels the operation.
        /// </summary>
        public event Action? CancelRequested;
        
        /// <summary>
        /// Gets or sets the collection of customers available for selection.
        /// </summary>
        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set => SetProperty(ref _customers, value);
        }
        
        /// <summary>
        /// Gets or sets the collection of available rooms for the selected dates.
        /// </summary>
        public ObservableCollection<Room> AvailableRooms
        {
            get => _availableRooms;
            set => SetProperty(ref _availableRooms, value);
        }
        
        /// <summary>
        /// Gets or sets the selected customer for the reservation.
        /// </summary>
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
        
        /// <summary>
        /// Gets or sets the selected room for the reservation.
        /// </summary>
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
        
        /// <summary>
        /// Gets or sets the check-in date for the reservation.
        /// Ensures that check-out date is after check-in date.
        /// </summary>
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
        
        /// <summary>
        /// Gets or sets the check-out date for the reservation.
        /// Ensures that check-in date is before check-out date.
        /// </summary>
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
        
        /// <summary>
        /// Gets or sets the status of the reservation.
        /// </summary>
        public ReservationStatus SelectedStatus
        {
            get => _selectedStatus;
            set => SetProperty(ref _selectedStatus, value);
        }
        
        /// <summary>
        /// Gets or sets the total price for the reservation.
        /// </summary>
        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }
        
        /// <summary>
        /// Gets or sets any special requests for the reservation.
        /// </summary>
        public string? SpecialRequests
        {
            get => _specialRequests;
            set => SetProperty(ref _specialRequests, value);
        }
        
        /// <summary>
        /// Gets all possible reservation status values.
        /// </summary>
        public Array ReservationStatuses => Enum.GetValues(typeof(ReservationStatus));
        
        /// <summary>
        /// Gets the command to save the reservation.
        /// </summary>
        public ICommand SaveCommand { get; }
        
        /// <summary>
        /// Gets the command to cancel the form operation.
        /// </summary>
        public ICommand CancelCommand { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationFormViewModel"/> class.
        /// </summary>
        /// <param name="dbOptions">Database context options for data access.</param>
        /// <param name="reservation">Optional existing reservation for editing. If null, a new reservation will be created.</param>
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
            
            Task.Run(LoadInitialDataAsync);
        }
        
        /// <summary>
        /// Loads all initial data needed for the form, including customers and available rooms.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task LoadInitialDataAsync()
        {
            await LoadCustomersAsync();
            await LoadAvailableRoomsAsync();
        }
        
        /// <summary>
        /// Loads all customers from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
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
        
        /// <summary>
        /// Loads rooms that are available for the selected date range.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
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
        
        /// <summary>
        /// Calculates the total price based on the selected room and stay duration.
        /// </summary>
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
        
        /// <summary>
        /// Determines whether the save command can be executed.
        /// </summary>
        /// <returns>True if all required fields have valid values; otherwise, false.</returns>
        private bool CanSave()
        {
            return SelectedCustomer != null && SelectedRoom != null && CheckOutDate > CheckInDate;
        }
        
        /// <summary>
        /// Saves the reservation information.
        /// Creates a new reservation object or updates an existing one and raises the SaveCompleted event.
        /// </summary>
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
        
        /// <summary>
        /// Cancels the form operation.
        /// </summary>
        private void Cancel()
        {
            CancelRequested?.Invoke();
        }
    }
} 