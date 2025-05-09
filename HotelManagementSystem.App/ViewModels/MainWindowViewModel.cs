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

            // Load initial data
            LoadDataAsync().ConfigureAwait(false);
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
    }
} 