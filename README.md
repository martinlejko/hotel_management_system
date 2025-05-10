# 🏨 Hotel Management System

A cross-platform desktop application for hotel management built with C# and Avalonia UI.

## ✨ Features

- **🔍 Dashboard**: View current hotel occupancy rates and forecast
- **🛏️ Room Management**: Add, edit, and view hotel rooms with different types and rates
- **👥 Customer Management**: Track customer information, contact details, and stay history
- **📅 Reservation System**: Create and manage reservations with check-in/check-out dates
- **🔎 Search Functionality**: Find reservations by customer name, room number, or date range

## 🛠️ Technology Stack

- **🔷 C#** - Core programming language
- **🖌️ Avalonia UI 11.3.0** - Cross-platform UI framework (works on Windows, macOS, and Linux)
- **🗃️ Entity Framework Core 9.0.4** - ORM for database operations
- **💾 SQLite** - Embedded database

## 📁 Project Structure

- **🧩 HotelManagementSystem.Core** - Contains data models, repositories, and business logic
- **🖥️ HotelManagementSystem.App** - Avalonia UI application with views and view models

## 📥 Installation Guide

### 📋 Prerequisites

- .NET SDK 8.0 or later

### ⚙️ Installation Steps

1. Clone the repository
   ```
   git clone https://github.com/martinlejko/hotel_management_system.git
   cd hotel_management_system
   ```

2. Restore NuGet packages
   ```
   dotnet restore
   ```
   
   This command will download all required NuGet packages including:
   - Avalonia UI (v11.3.0) for cross-platform UI
   - Avalonia.Controls.DataGrid (v11.3.0) for data display
   - Entity Framework Core (v9.0.4) for database operations
   - Microsoft.EntityFrameworkCore.Sqlite (v9.0.4) for SQLite support

3. Build the solution
   ```
   dotnet build
   ```

5. Run the application
   ```
   dotnet run --project HotelManagementSystem.App
   ```
### 🗄️ Database Information

The application uses SQLite for data storage. The database file is automatically created in your local application data folder:

- **🪟 Windows**: `%LOCALAPPDATA%\HotelManagementSystem.db`
- **🍎 macOS**: `~/Library/Application Support/HotelManagementSystem.db`
- **🐧 Linux**: `~/.local/share/HotelManagementSystem.db`

The database is automatically seeded with sample data on first run, including:
- 🛏️ Hotel rooms of different types (Standard, Deluxe, Suite)
- 👤 Sample customers
- 📝 Example reservations

## 📱 Using the Application

### 📊 Dashboard
- View current occupancy rate and statistics
- See upcoming reservations and check-outs
- Monitor revenue and key performance indicators

### 🛏️ Room Management
- Add new rooms with details like room number, type, rate, and amenities
- View room availability and status (occupied, vacant, maintenance)
- Filter rooms by various criteria

### 📅 Reservation System
- Create new reservations by selecting customer, room, and dates
- Modify existing reservations (extend stays, change rooms)
- Process check-ins and check-outs

### 👥 Customer Management
- Add and edit customer information
- View customer history and preferences
- Search for customers by name or ID

## 💻 Development

The project follows MVVM architecture:
- **📋 Models** - Defined in the Core project (Customer, Room, Reservation)
- **🎨 Views** - XAML files in the App project
- **🔄 ViewModels** - Connect the models to the views, located in the App project

## ❓ Troubleshooting

### Common Issues and Solutions
- **Missing NuGet Packages**: If you see errors about missing packages, run `dotnet restore` again
- **Database Errors**: Delete the database file if corrupted - it will be recreated on next run
- **UI Rendering Issues**: Make sure you have the latest graphics drivers installed
- **Build Errors**: Ensure you have the correct .NET SDK version installed (8.0+)

If you encounter persistent issues, please submit an issue on the GitHub repository.

## 📄 License

MIT License