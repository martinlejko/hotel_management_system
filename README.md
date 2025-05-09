# Hotel Management System

A cross-platform desktop application for hotel management built with C# and Avalonia UI.

## Features

- **Dashboard**: View current hotel occupancy rates and forecast with interactive charts
- **Room Management**: Add, edit, and view hotel rooms
- **Customer Management**: Track customer information and history
- **Reservation System**: Create and manage reservations for rooms
- **Search Functionality**: Find reservations by customer name or room number
- **Data Visualization**: Charts to visualize occupancy rates

## Technology Stack

- **C#** - Core programming language
- **Avalonia UI** - Cross-platform UI framework (works on Windows, macOS, and Linux)
- **Entity Framework Core** - ORM for database operations
- **SQLite** - Embedded database
- **OxyPlot** - Charting library for data visualization

## Project Structure

- **HotelManagementSystem.Core** - Contains data models, repositories, and business logic
- **HotelManagementSystem.App** - Avalonia UI application with views and view models

## Setup Instructions

### Prerequisites

- .NET SDK 8.0 or later

### Installation

1. Clone the repository
   ```
   git clone https://github.com/yourusername/hotel_management_system.git
   cd hotel_management_system
   ```

2. Build the solution
   ```
   dotnet build
   ```

3. Run the application
   ```
   dotnet run --project HotelManagementSystem.App
   ```

## Development

The project follows MVVM architecture:
- **Models** - Defined in the Core project (Customer, Room, Reservation)
- **Views** - XAML files in the App project
- **ViewModels** - Connect the models to the views, located in the App project

## License

MIT License