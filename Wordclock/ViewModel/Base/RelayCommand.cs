using System;
using System.Windows.Input;

namespace Wordclock
{
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// The action to run
        /// </summary>
        private Action mAction;



        public event EventHandler CanExecuteChanged = (sender, e) => { };



        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="action"></param>
        public RelayCommand(Action action)
        {
            mAction = action;
        }



        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
           mAction();
        }
    }
}
