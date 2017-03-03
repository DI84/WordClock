using System;
using System.Windows.Input;

namespace Wordclock
{
    public class RelayCommand : ICommand
    {
        #region Private members

        /// <summary>
        /// The action to run
        /// </summary>
        private Action mAction;

        #endregion
        
        #region Public eventhandler

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion
        
        #region Constructor

        /// <summary>
        /// defaukt constructor
        /// </summary>
        /// <param name="action"></param>
        public RelayCommand(Action action)
        {
            mAction = action;
        }

        #endregion

        #region Command methods

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
