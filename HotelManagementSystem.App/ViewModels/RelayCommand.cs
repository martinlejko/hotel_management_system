using System;
using System.Windows.Input;
using Avalonia.Threading;

namespace HotelManagementSystem.App.ViewModels
{
    /// <summary>
    /// Implementation of the <see cref="ICommand"/> interface to relay command execution to delegate methods.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The delegate to execute when the command is invoked.</param>
        /// <param name="canExecute">The delegate to check if the command can be executed. Can be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when the execute delegate is null.</exception>
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
            
            CommandManager.RequerySuggested += (s, e) => RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Event that is raised when conditions for command execution have changed.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            Dispatcher.UIThread.Post(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty));
        }

        /// <summary>
        /// Determines whether the command can be executed in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data, this parameter can be null.</param>
        /// <returns>True if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data, this parameter can be null.</param>
        public void Execute(object? parameter) => _execute(parameter);
    }
    
    /// <summary>
    /// Manages command states and provides a way to notify commands when their ability to execute might have changed.
    /// </summary>
    public static class CommandManager
    {
        /// <summary>
        /// Event that is raised when commands should check if they can execute.
        /// </summary>
        public static event EventHandler? RequerySuggested;
        
        /// <summary>
        /// Raises the <see cref="RequerySuggested"/> event.
        /// </summary>
        public static void InvalidateRequerySuggested()
        {
            RequerySuggested?.Invoke(null, EventArgs.Empty);
        }
    }
} 