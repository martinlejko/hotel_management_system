using Avalonia.Controls;
using HotelManagementSystem.Core.Data;
using HotelManagementSystem.Core.Models;
using HotelManagementSystem.Core.Repositories;
using HotelManagementSystem.Core.Services;
using Microsoft.EntityFrameworkCore;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelManagementSystem.App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly DbContextOptions<HotelDbContext> _dbOptions;
        private readonly IHotelStatsService? _statsService;
        private readonly IReservationRepository? _reservationRepository;
        private readonly IRoomRepository? _roomRepository;
        private readonly ICustomerRepository? _customerRepository;

        private ObservableCollection<Reservation>? _recentReservations;
        private ObservableCollection<Room>? _rooms;
        private ObservableCollection<Customer>? _customers;
        private PlotModel? _occupancyPlotModel;
        private string? _searchText;
        private DateTime _selectedDate;
        private double _occupancyRate;
        private Room? _selectedRoom;
        private Customer? _selectedCustomer;
        private Reservation? _selectedReservation;
        private ViewModelBase? _currentView;
        private bool _isViewingList = true;
        
        // Command properties
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

        public PlotModel? OccupancyPlotModel
        {
            get => _occupancyPlotModel;
            set => SetProperty(ref _occupancyPlotModel, value);
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
        
        public Room? SelectedRoom
        {
            get => _selectedRoom;
            set => SetProperty(ref _selectedRoom, value);
        }
        
        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set => SetProperty(ref _selectedCustomer, value);
        }
        
        public Reservation? SelectedReservation
        {
            get => _selectedReservation;
            set => SetProperty(ref _selectedReservation, value);
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

        public MainWindowViewModel(DbContextOptions<HotelDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
            
            // Initialize repositories and services
            using (var context = new HotelDbContext(_dbOptions))
            {
                _reservationRepository = new ReservationRepository(context);
                _roomRepository = new RoomRepository(context);
                _customerRepository = new CustomerRepository(context);
                _statsService = new HotelStatsService(_roomRepository, _reservationRepository);
            }

            // Initialize collections
            RecentReservations = new ObservableCollection<Reservation>();
            Rooms = new ObservableCollection<Room>();
            Customers = new ObservableCollection<Customer>();
            
            // Initialize properties
            SelectedDate = DateTime.Today;
            SearchText = string.Empty;
            IsViewingList = true;
            
            // Initialize commands
            AddRoomCommand = new RelayCommand(_ => AddRoom());
            EditRoomCommand = new RelayCommand(_ => EditRoom(), _ => SelectedRoom != null);
            DeleteRoomCommand = new RelayCommand(async _ => await DeleteRoomAsync(), _ => SelectedRoom != null);
            
            AddCustomerCommand = new RelayCommand(_ => AddCustomer());
            EditCustomerCommand = new RelayCommand(_ => EditCustomer(), _ => SelectedCustomer != null);
            DeleteCustomerCommand = new RelayCommand(async _ => await DeleteCustomerAsync(), _ => SelectedCustomer != null);
            
            AddReservationCommand = new RelayCommand(_ => AddReservation());
            EditReservationCommand = new RelayCommand(_ => EditReservation(), _ => SelectedReservation != null);
            DeleteReservationCommand = new RelayCommand(async _ => await DeleteReservationAsync(), _ => SelectedReservation != null);
            
            RefreshDataCommand = new RelayCommand(async _ => await LoadDataAsync());
            BackToListCommand = new RelayCommand(_ => ShowListView());

            // Load initial data
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

                // Get occupancy for the next 7 days
                var startDate = SelectedDate;
                var endDate = startDate.AddDays(6);
                var occupancyStats = await statsService.GetOccupancyStatsForDateRangeAsync(startDate, endDate);

                // Update current occupancy rate
                var currentStats = await statsService.GetCurrentOccupancyAsync();
                OccupancyRate = currentStats.OccupancyRate * 100;

                // Create plot model
                var plotModel = new PlotModel { Title = "Occupancy Forecast" };
                
                var xAxis = new CategoryAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = "Date"
                };

                var yAxis = new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Occupancy Rate (%)",
                    Minimum = 0,
                    Maximum = 100
                };

                plotModel.Axes.Add(xAxis);
                plotModel.Axes.Add(yAxis);

                var occupancySeries = new BarSeries
                {
                    Title = "Occupancy Rate",
                    FillColor = OxyColor.FromRgb(0, 158, 115),
                };

                foreach (var stat in occupancyStats)
                {
                    xAxis.Labels.Add(stat.Date.ToString("MM/dd"));
                    occupancySeries.Items.Add(new BarItem(stat.OccupancyRate * 100));
                }

                plotModel.Series.Add(occupancySeries);
                OccupancyPlotModel = plotModel;
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
                
                // Only perform filtering if SearchText is not null
                var searchText = SearchText ?? string.Empty;
                var filteredReservations = allReservations.Where(r => 
                    (r.Customer?.FirstName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true) ||
                    (r.Customer?.LastName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true) ||
                    (r.Room?.RoomNumber?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true))
                    .Take(10);
                
                RecentReservations?.Clear();
                foreach (var reservation in filteredReservations)
                {
                    RecentReservations?.Add(reservation);
                }
            }
        }
        
        // Room operations
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
                    // New room
                    await roomRepo.AddAsync(room);
                }
                else
                {
                    // Existing room
                    await roomRepo.UpdateAsync(room);
                }
                
                await roomRepo.SaveChangesAsync();
            }
            
            // Refresh rooms list
            await LoadRoomsAsync();
            
            // Go back to list view
            ShowListView();
        }
        
        private async Task DeleteRoomAsync()
        {
            if (SelectedRoom == null) return;
            
            using (var context = new HotelDbContext(_dbOptions))
            {
                var roomRepo = new RoomRepository(context);
                var room = await roomRepo.GetByIdAsync(SelectedRoom.Id);
                if (room != null)
                {
                    await roomRepo.DeleteAsync(room);
                    await roomRepo.SaveChangesAsync();
                }
            }
            
            // Refresh rooms list
            await LoadRoomsAsync();
        }
        
        // Customer operations
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
                    // New customer
                    await customerRepo.AddAsync(customer);
                }
                else
                {
                    // Existing customer
                    await customerRepo.UpdateAsync(customer);
                }
                
                await customerRepo.SaveChangesAsync();
            }
            
            // Refresh customers list
            await LoadCustomersAsync();
            
            // Go back to list view
            ShowListView();
        }
        
        private async Task DeleteCustomerAsync()
        {
            if (SelectedCustomer == null) return;
            
            using (var context = new HotelDbContext(_dbOptions))
            {
                var customerRepo = new CustomerRepository(context);
                var customer = await customerRepo.GetByIdAsync(SelectedCustomer.Id);
                if (customer != null)
                {
                    await customerRepo.DeleteAsync(customer);
                    await customerRepo.SaveChangesAsync();
                }
            }
            
            // Refresh customers list
            await LoadCustomersAsync();
        }
        
        // Reservation operations
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
                    // New reservation
                    await reservationRepo.AddAsync(reservation);
                }
                else
                {
                    // Existing reservation
                    await reservationRepo.UpdateAsync(reservation);
                }
                
                await reservationRepo.SaveChangesAsync();
            }
            
            // Refresh reservations list
            await LoadReservationsAsync();
            
            // Also refresh occupancy data as it may have changed
            await LoadOccupancyDataAsync();
            
            // Go back to list view
            ShowListView();
        }
        
        private async Task DeleteReservationAsync()
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
                }
            }
            
            // Refresh reservations list
            await LoadReservationsAsync();
            // Also refresh occupancy data as it may have changed
            await LoadOccupancyDataAsync();
        }
        
        private void OnFormCancelled()
        {
            ShowListView();
        }
    }
} 