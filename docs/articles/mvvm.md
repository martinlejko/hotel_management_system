# MVVM Architecture

The Hotel Management System implements the Model-View-ViewModel (MVVM) architectural pattern, which separates the user interface (View) from the business logic and data (Model) via a mediator (ViewModel).

## Components

### ViewModelBase

The `ViewModelBase` class provides the foundation for all view models in the application. It implements the `INotifyPropertyChanged` interface, which is essential for UI data binding. Key features include:

- Property change notification via the `PropertyChanged` event
- Helper methods for property setters with change notification

### RelayCommand

The `RelayCommand` class implements the `ICommand` interface to provide a way to bind UI elements to methods in view models. Key features include:

- Command execution via delegate methods
- Command enabling/disabling logic
- Change notification for command execution state

### ViewModel Classes

The application includes several view models for different screens:

- `MainWindowViewModel` - Main window orchestration
- `RoomFormViewModel` - Room creation and editing
- `CustomerFormViewModel` - Customer creation and editing
- `ReservationFormViewModel` - Reservation creation and management

## Data Binding

ViewModels expose properties and commands that views can bind to:

```xml
<TextBox Text="{Binding CustomerName}" />
<Button Command="{Binding SaveCommand}" />
```

When a ViewModel property changes, the UI is automatically updated through the `INotifyPropertyChanged` implementation.