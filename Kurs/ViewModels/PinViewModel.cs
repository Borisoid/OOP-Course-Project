using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;

using Kurs.Models;
using Kurs.Views;
using Kurs.Commands;

namespace Kurs.ViewModels
{
    public class PinViewModel : ViewModelBase
    {
        #region Constructors

        public PinViewModel(Pin pin)
        {
            if (pin == null)
                throw new ArgumentException("PinViewModel recieved null pin param.");

            Pin = pin;

            //PinView = new PinView((Pin is InputPin) ? PinView.PinType.Input : PinView.PinType.Output);

            connectionViewMoedels = new BindingList<ConnectionViewModel>();
        }

        #endregion

        #region Properties

        public PinView PinView { get { return _pinView; } set { _pinView = value; } }    //IT IS USED
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        #endregion

        #region Methods

        public void AssignConnection(ConnectionViewModel cvm)
        {
            Pin.AssignConnection(cvm.connection);
            connectionViewMoedels.Add(cvm);
        }

        public void BreakConnection(ConnectionViewModel con)
        {
            Pin.Connections.Remove(con.connection);
            connectionViewMoedels.Remove(con);
        }

        #endregion

        #region Data

        public readonly Pin Pin;
        [NonSerialized]
        public PinView _pinView;    //IT IS USED
        public BindingList<ConnectionViewModel> connectionViewMoedels;
        public bool _isSelected;

        #endregion

        #region Commands

        private DelegateCommand SelectCommand;

        public ICommand selectCommand
        {
            get
            {
                if (SelectCommand == null)
                {
                    SelectCommand = new DelegateCommand(Select);
                }
                return SelectCommand;
            }
        }
        public void Select()
        {
            IsSelected = true;
        }

        #endregion
    }
}
