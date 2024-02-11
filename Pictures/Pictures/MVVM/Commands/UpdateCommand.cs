using System;
using System.Windows.Input;

namespace Pictures.MVVM.Commands
{
    /// <summary>
    /// Команда для взаимодействия представления с моделью
    /// </summary>
    public class UpdateCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        /// <summary>
        /// Команда обновления
        /// </summary>
        /// <param name="execute">Функция для выполнения команды</param>
        /// <param name="canExecute">Функция для определения возможности выполнения команды</param>
        public UpdateCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        /// <summary>
        /// Возможно ли выполнить команду
        /// </summary>
        /// <param name="parameter">параметр</param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }
        /// <summary>
        /// Выполение команды
        /// </summary>
        /// <param name="parameter">параметр</param>
        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
