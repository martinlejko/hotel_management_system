# Introduction to Hotel Management System

The Hotel Management System is a desktop application built with Avalonia UI and .NET 8.0. It provides functionality for managing hotel operations, including customer management, room management, and reservation handling.

## Architecture

The application is built using the MVVM (Model-View-ViewModel) architectural pattern:

- **Models** - Data entities representing business objects like Rooms, Customers, and Reservations
- **Views** - Avalonia UI components that display the user interface
- **ViewModels** - Classes that handle presentation logic and interact with the models

## Key Components

### Core Components

The `HotelManagementSystem.Core` project contains:

- **Models** - Business entities (Room, Customer, Reservation)
- **Repositories** - Data access layer
- **Services** - Business logic services
- **Validation** - Validation rules and utilities

### Application Components

The `HotelManagementSystem.App` project contains:

- **ViewModels** - Classes for binding UI to data
- **Views** - Avalonia UI components
- **Converters** - Value converters for UI binding

## MVVM Implementation

The application implements MVVM using:

- **ViewModelBase** - Base class for all view models with property change notification
- **RelayCommand** - Command implementation to bind UI actions to view model methods

## Getting Started

To start using the application, see the [User Guide](user-guide.md). 