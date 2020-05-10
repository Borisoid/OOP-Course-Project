using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using Kurs.Models;
using Kurs.Views;

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

            PinView = new PinView((Pin is InputPin) ? PinView.PinType.Input : PinView.PinType.Output);

            connectionViewMoedels = new BindingList<ConnectionViewModel>();
        }

        #endregion

        #region Methods

        public void AssignConnection(ConnectionViewModel cvm)
        {
            Pin.AssignConnection(cvm.connection);
            connectionViewMoedels.Add(cvm);
        }

        #endregion

        #region Data

        public readonly Pin Pin;
        public readonly PinView PinView;
        public BindingList<ConnectionViewModel> connectionViewMoedels;

        #endregion
    }
}
