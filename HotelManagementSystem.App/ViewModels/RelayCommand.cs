using System;
using System.Windows.Input;
using Avalonia.Threading;

namespace HotelManagementSystem.App.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
            
            // Register for CommandManager's RequerySuggested event to ensure
            // command state is reevaluated when it should be
            CommandManager.RequerySuggested += (s, e) => RaiseCanExecuteChanged();
        }

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            // Use Avalonia's Dispatcher to ensure we're on the UI thread
            Dispatcher.UIThread.Post(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty));
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object? parameter) => _execute(parameter);
    }
    
    // Simple CommandManager implementation since Avalonia doesn't have one
    public static class CommandManager
    {
        public static event EventHandler? RequerySuggested;
        
        public static void InvalidateRequerySuggested()
        {
            RequerySuggested?.Invoke(null, EventArgs.Empty);
        }
    }
} 