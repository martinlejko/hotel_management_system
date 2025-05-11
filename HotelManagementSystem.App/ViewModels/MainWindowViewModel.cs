using HotelManagementSystem.Core.Data;
using HotelManagementSystem.Core.Models;
using HotelManagementSystem.Core.Repositories;
using HotelManagementSystem.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;

namespace HotelManagementSystem.App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly DbContextOptions<HotelDbContext> _dbOptions;
        private readonly IReservationRepository? _reservationRepository;
        private readonly IRoomRepository? _roomRepository;
        private readonly ICustomerRepository? _customerRepository;

        private ObservableCollection<Reservation>? _recentReservations;
        private ObservableCollection<Room>? _rooms;
        private ObservableCollection<Customer>? _customers;
        private string? _searchText;
        private DateTime _selectedDate;
        private double _occupancyRate;
        private double _occupancyAngle;
        private double _occupancyBarWidth;
        private Point _occupancyEndPoint;
        private bool _isLargeArc;
        private bool _isSmallArc;
        private Room? _selectedRoom;
        private Customer? _selectedCustomer;
        private Reservation? _selectedReservation;
        private ViewModelBase? _currentView;
        private bool _isViewingList = true;
        
        private bool _isConfirmingDelete;
        private string _deleteConfirmationTitle = string.Empty;
        private string _deleteConfirmationMessage = string.Empty;
        private DeleteItemType _deleteItemType;
        
        public ICommand AddRoomCommand { get; }
        public ICommand EditRoomCommand { get; }
        public ICommand DeleteRoomCommand { get; }
        public ICommand AddCustomerCommand { get; }
        public ICommand EditCustomerCommand { get; }
        public ICommand DeleteCustomerCommand { get; }
        public ICommand AddReservationCommand { get; }
        public ICommand EditReservationCommand { get; }
        public ICommand DeleteReservationCommand { get; }
        public ICommand RefreshDataCommand { get; }
        public ICommand BackToListCommand { get; }
        public ICommand ConfirmDeleteCommand { get; }
        public ICommand CancelDeleteCommand { get; }
        
        private enum DeleteItemType
        {
            None,
            Room,
            Customer,
            Reservation
        }
        
        public ObservableCollection<Reservation>? RecentReservations
        {
            get => _recentReservations;
            set => SetProperty(ref _recentReservations, value);
        }

        public ObservableCollection<Room>? Rooms
        {
            get => _rooms;
            set => SetProperty(ref _rooms, value);
        }

        public ObservableCollection<Customer>? Customers
        {
            get => _customers;
            set => SetProperty(ref _customers, value);
        }

        public string? SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    SearchReservationsAsync().ConfigureAwait(false);
                }
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (SetProperty(ref _selectedDate, value))
                {
                    LoadOccupancyDataAsync().ConfigureAwait(false);
                }
            }
        }

        public double OccupancyRate
        {
            get => _occupancyRate;
            set => SetProperty(ref _occupancyRate, value);
        }
        
        public double OccupancyAngle
        {
            get => _occupancyAngle;
            set => SetProperty(ref _occupancyAngle, value);
        }
        
        public double OccupancyBarWidth
        {
            get => _occupancyBarWidth;
            set => SetProperty(ref _occupancyBarWidth, value);
        }
        
        public Point OccupancyEndPoint
        {
            get => _occupancyEndPoint;
            set => SetProperty(ref _occupancyEndPoint, value);
        }
        
        public bool IsLargeArc
        {
            get => _isLargeArc;
            set => SetProperty(ref _isLargeArc, value);
        }
        
        public bool IsSmallArc
        {
            get => _isSmallArc;
            set => SetProperty(ref _isSmallArc, value);
        }
        
        public Room? SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                if (SetProperty(ref _selectedRoom, value))
                {
                    (EditRoomCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (DeleteRoomCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }
        
        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (SetProperty(ref _selectedCustomer, value))
                {
                    (EditCustomerCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (DeleteCustomerCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }
        
        public Reservation? SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                if (SetProperty(ref _selectedReservation, value))
                {
                    (EditReservationCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (DeleteReservationCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }
        
        public ViewModelBase? CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }
        
        public bool IsViewingList
        {
            get => _isViewingList;
            set => SetProperty(ref _isViewingList, value);
        }
        
        public bool IsConfirmingDelete
        {
            get => _isConfirmingDelete;
            set => SetProperty(ref _isConfirmingDelete, value);
        }
        
        public string DeleteConfirmationTitle
        {
            get => _deleteConfirmationTitle;
            set => SetProperty(ref _deleteConfirmationTitle, value);
        }
        
        public string DeleteConfirmationMessage
        {
            get => _deleteConfirmationMessage;
            set => SetProperty(ref _deleteConfirmationMessage, value);
        }

        public MainWindowViewModel(DbContextOptions<HotelDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
            
            using (var context = new HotelDbContext(_dbOptions))
            {
                _reservationRepository = new ReservationRepository(context);
                _roomRepository = new RoomRepository(context);
                _customerRepository = new CustomerRepository(context);
            }

            RecentReservations = new ObservableCollection<Reservation>();
            Rooms = new ObservableCollection<Room>();
            Customers = new ObservableCollection<Customer>();
            
            SelectedDate = DateTime.Today;
            SearchText = string.Empty;
            IsViewingList = true;
            
            AddRoomCommand = new RelayCommand(_ => AddRoom());
            EditRoomCommand = new RelayCommand(_ => EditRoom(), _ => SelectedRoom != null);
            DeleteRoomCommand = new RelayCommand(_ => ShowDeleteConfirmation(DeleteItemType.Room), _ => SelectedRoom != null);
            
            AddCustomerCommand = new RelayCommand(_ => AddCustomer());
            EditCustomerCommand = new RelayCommand(_ => EditCustomer(), _ => SelectedCustomer != null);
            DeleteCustomerCommand = new RelayCommand(_ => ShowDeleteConfirmation(DeleteItemType.Customer), _ => SelectedCustomer != null);
            
            AddReservationCommand = new RelayCommand(_ => AddReservation());
            EditReservationCommand = new RelayCommand(_ => EditReservation(), _ => SelectedReservation != null);
            DeleteReservationCommand = new RelayCommand(_ => ShowDeleteConfirmation(DeleteItemType.Reservation), _ => SelectedReservation != null);
            
            RefreshDataCommand = new RelayCommand(async _ => await LoadDataAsync());
            BackToListCommand = new RelayCommand(_ => ShowListView());
            
            ConfirmDeleteCommand = new RelayCommand(async _ => await ConfirmDeleteAsync());
            CancelDeleteCommand = new RelayCommand(_ => CancelDelete());

            LoadDataAsync().ConfigureAwait(false);
        }
        
        private void ShowListView()
        {
            CurrentView = null;
            IsViewingList = true;
        }

        private async Task LoadDataAsync()
        {
            await LoadReservationsAsync();
            await LoadRoomsAsync();
            await LoadCustomersAsync();
            await LoadOccupancyDataAsync();
        }

        private async Task LoadReservationsAsync()
        {
            using (var context = new HotelDbContext(_dbOptions))
            {
                var reservationRepo = new ReservationRepository(context);
                var reservations = await reservationRepo.GetAllReservationsWithDetailsAsync();
                
                RecentReservations?.Clear();
                foreach (var reservation in reservations.Take(10))
                {
                    RecentReservations?.Add(reservation);
                }
            }
        }

        private async Task LoadRoomsAsync()
        {
            using (var context = new HotelDbContext(_dbOptions))
            {
                var roomRepo = new RoomRepository(context);
                var rooms = await roomRepo.GetAllAsync();
                
                Rooms?.Clear();
                foreach (var room in rooms)
                {
                    Rooms?.Add(room);
                }
            }
        }

        private async Task LoadCustomersAsync()
        {
            using (var context = new HotelDbContext(_dbOptions))
            {
                var customerRepo = new CustomerRepository(context);
                var customers = await customerRepo.GetAllAsync();
                
                Customers?.Clear();
                foreach (var customer in customers)
                {
                    Customers?.Add(customer);
                }
            }
        }

        private async Task LoadOccupancyDataAsync()
        {
            using (var context = new HotelDbContext(_dbOptions))
            {
                var roomRepo = new RoomRepository(context);
                var reservationRepo = new ReservationRepository(context);
                var statsService = new HotelStatsService(roomRepo, reservationRepo);

                var currentStats = await statsService.GetCurrentOccupancyAsync();
                OccupancyRate = currentStats.OccupancyRate * 100;
                
                // Calculate pie chart parameters - radius is now 60px
                double angleInRadians = OccupancyRate * 3.6 * (Math.PI / 180); // Convert to radians
                
                // Calculate end point using 60px radius
                double x = 60 + 60 * Math.Sin(angleInRadians);
                double y = 60 - 60 * Math.Cos(angleInRadians);
                OccupancyEndPoint = new Point(x, y);
                
                // Determine if arc is large or small
                IsLargeArc = OccupancyRate > 50;
                IsSmallArc = OccupancyRate <= 50;
            }
        }

        private async Task SearchReservationsAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadReservationsAsync();
                return;
            }

            using (var context = new HotelDbContext(_dbOptions))
            {
                var reservationRepo = new ReservationRepository(context);
                var allReservations = await reservationRepo.GetAllReservationsWithDetailsAsync();
                
                var searchText = SearchText ?? string.Empty;
                var filteredReservations = allReservations.Where(r => 
                    (r.Customer?.FirstName?.Contains(searchText, System.StringComparison.OrdinalIgnoreCase) == true) ||
                    (r.Customer?.LastName?.Contains(searchText, System.StringComparison.OrdinalIgnoreCase) == true) ||
                    (r.Room?.RoomNumber?.Contains(searchText, System.StringComparison.OrdinalIgnoreCase) == true))
                    .Take(10);
                
                RecentReservations?.Clear();
                foreach (var reservation in filteredReservations)
                {
                    RecentReservations?.Add(reservation);
                }
            }
        }
        
        private void AddRoom()
        {
            var viewModel = new RoomFormViewModel();
            viewModel.SaveCompleted += OnRoomSaveCompleted;
            viewModel.CancelRequested += OnFormCancelled;
            
            CurrentView = viewModel;
            IsViewingList = false;
        }
        
        private void EditRoom()
        {
            if (SelectedRoom == null) return;
            
            var viewModel = new RoomFormViewModel(SelectedRoom);
            viewModel.SaveCompleted += OnRoomSaveCompleted;
            viewModel.CancelRequested += OnFormCancelled;
            
            CurrentView = viewModel;
            IsViewingList = false;
        }
        
        private async void OnRoomSaveCompleted(Room room)
        {
            using (var context = new HotelDbContext(_dbOptions))
            {
                var roomRepo = new RoomRepository(context);
                
                if (room.Id == 0)
                {
                    await roomRepo.AddAsync(room);
                }
                else
                {
                    await roomRepo.UpdateAsync(room);
                }
                
                await roomRepo.SaveChangesAsync();
            }
            
            await LoadRoomsAsync();
            
            ShowListView();
        }
        
        private void AddCustomer()
        {
            var viewModel = new CustomerFormViewModel();
            viewModel.SaveCompleted += OnCustomerSaveCompleted;
            viewModel.CancelRequested += OnFormCancelled;
            
            CurrentView = viewModel;
            IsViewingList = false;
        }
        
        private void EditCustomer()
        {
            if (SelectedCustomer == null) return;
            
            var viewModel = new CustomerFormViewModel(SelectedCustomer);
            viewModel.SaveCompleted += OnCustomerSaveCompleted;
            viewModel.CancelRequested += OnFormCancelled;
            
            CurrentView = viewModel;
            IsViewingList = false;
        }
        
        private async void OnCustomerSaveCompleted(Customer customer)
        {
            using (var context = new HotelDbContext(_dbOptions))
            {
                var customerRepo = new CustomerRepository(context);
                
                if (customer.Id == 0)
                {
                    await customerRepo.AddAsync(customer);
                }
                else
                {
                    await customerRepo.UpdateAsync(customer);
                }
                
                await customerRepo.SaveChangesAsync();
            }
            
            await LoadCustomersAsync();
            
            ShowListView();
        }
        
        private void AddReservation()
        {
            var viewModel = new ReservationFormViewModel(_dbOptions);
            viewModel.SaveCompleted += OnReservationSaveCompleted;
            viewModel.CancelRequested += OnFormCancelled;
            
            CurrentView = viewModel;
            IsViewingList = false;
        }
        
        private void EditReservation()
        {
            if (SelectedReservation == null) return;
            
            var viewModel = new ReservationFormViewModel(_dbOptions, SelectedReservation);
            viewModel.SaveCompleted += OnReservationSaveCompleted;
            viewModel.CancelRequested += OnFormCancelled;
            
            CurrentView = viewModel;
            IsViewingList = false;
        }
        
        private async void OnReservationSaveCompleted(Reservation reservation)
        {
            using (var context = new HotelDbContext(_dbOptions))
            {
                var reservationRepo = new ReservationRepository(context);
                
                if (reservation.Id == 0)
                {
                    await reservationRepo.AddAsync(reservation);
                }
                else
                {
                    await reservationRepo.UpdateAsync(reservation);
                }
                
                await reservationRepo.SaveChangesAsync();
            }
            
            await LoadReservationsAsync();
            
            await LoadOccupancyDataAsync();
            
            ShowListView();
        }
        
        private void OnFormCancelled()
        {
            ShowListView();
        }

        private void ShowDeleteConfirmation(DeleteItemType itemType)
        {
            _deleteItemType = itemType;
            
            switch (itemType)
            {
                case DeleteItemType.Room:
                    if (SelectedRoom != null)
                    {
                        DeleteConfirmationTitle = "Delete Room";
                        DeleteConfirmationMessage = $"Are you sure you want to delete room {SelectedRoom.RoomNumber}? This action cannot be undone.";
                    }
                    break;
                    
                case DeleteItemType.Customer:
                    if (SelectedCustomer != null)
                    {
                        DeleteConfirmationTitle = "Delete Customer";
                        DeleteConfirmationMessage = $"Are you sure you want to delete customer {SelectedCustomer.FirstName} {SelectedCustomer.LastName}? This action cannot be undone.";
                    }
                    break;
                    
                case DeleteItemType.Reservation:
                    if (SelectedReservation != null)
                    {
                        var customerName = SelectedReservation.Customer?.FullName ?? "Unknown";
                        var roomNumber = SelectedReservation.Room?.RoomNumber ?? "Unknown";
                        DeleteConfirmationTitle = "Delete Reservation";
                        DeleteConfirmationMessage = $"Are you sure you want to delete the reservation for {customerName} in room {roomNumber}? This action cannot be undone.";
                    }
                    break;
            }
            
            IsConfirmingDelete = true;
        }
        
        private void CancelDelete()
        {
            IsConfirmingDelete = false;
            _deleteItemType = DeleteItemType.None;
        }
        
        private async Task ConfirmDeleteAsync()
        {
            switch (_deleteItemType)
            {
                case DeleteItemType.Room:
                    await DeleteRoomInternalAsync();
                    break;
                    
                case DeleteItemType.Customer:
                    await DeleteCustomerInternalAsync();
                    break;
                    
                case DeleteItemType.Reservation:
                    await DeleteReservationInternalAsync();
                    break;
            }
            
            IsConfirmingDelete = false;
            _deleteItemType = DeleteItemType.None;
        }
        
        private async Task DeleteRoomInternalAsync()
        {
            if (SelectedRoom == null) return;
            
            using (var context = new HotelDbContext(_dbOptions))
            {
                var roomRepo = new RoomRepository(context);
                var reservationRepo = new ReservationRepository(context);
                
                var hasReservations = await reservationRepo.HasReservationsForRoomAsync(SelectedRoom.Id);
                if (hasReservations)
                {
                    DeleteConfirmationTitle = "Cannot Delete Room";
                    DeleteConfirmationMessage = $"Room {SelectedRoom.RoomNumber} has existing reservations and cannot be deleted. Please delete the reservations first.";
                    return;
                }
                
                var room = await roomRepo.GetByIdAsync(SelectedRoom.Id);
                if (room != null)
                {
                    await roomRepo.DeleteAsync(room);
                    await roomRepo.SaveChangesAsync();
                    
                    await LoadRoomsAsync();
                    IsConfirmingDelete = false;
                }
            }
        }
        
        private async Task DeleteCustomerInternalAsync()
        {
            if (SelectedCustomer == null) return;
            
            using (var context = new HotelDbContext(_dbOptions))
            {
                var customerRepo = new CustomerRepository(context);
                var reservationRepo = new ReservationRepository(context);
                
                var hasReservations = await reservationRepo.HasReservationsForCustomerAsync(SelectedCustomer.Id);
                if (hasReservations)
                {
                    DeleteConfirmationTitle = "Cannot Delete Customer";
                    DeleteConfirmationMessage = $"Customer {SelectedCustomer.FirstName} {SelectedCustomer.LastName} has existing reservations and cannot be deleted. Please delete the reservations first.";
                    return;
                }
                
                var customer = await customerRepo.GetByIdAsync(SelectedCustomer.Id);
                if (customer != null)
                {
                    await customerRepo.DeleteAsync(customer);
                    await customerRepo.SaveChangesAsync();
                    
                    await LoadCustomersAsync();
                    IsConfirmingDelete = false;
                }
            }
        }
        
        private async Task DeleteReservationInternalAsync()
        {
            if (SelectedReservation == null) return;
            
            using (var context = new HotelDbContext(_dbOptions))
            {
                var reservationRepo = new ReservationRepository(context);
                var reservation = await reservationRepo.GetByIdAsync(SelectedReservation.Id);
                if (reservation != null)
                {
                    await reservationRepo.DeleteAsync(reservation);
                    await reservationRepo.SaveChangesAsync();
                    
                    await LoadReservationsAsync();
                    await LoadOccupancyDataAsync();
                    IsConfirmingDelete = false;
                }
            }
        }
    }
} 