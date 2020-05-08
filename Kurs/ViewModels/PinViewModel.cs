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
        public PinViewModel(Pin pin)
        {
            if (pin == null)
                throw new ArgumentException("PinViewModel recieved null pin param.");

            Pin = pin;

            PinView = new PinView((Pin is InputPin) ? PinView.PinType.Input : PinView.PinType.Output);

            connectionViewMoedels = new BindingList<ConnectionViewMoedel>();
        }
        public readonly Pin Pin;
        public PinView PinView { get; private set; }
        public BindingList<ConnectionViewMoedel> connectionViewMoedels;

        public void AssignConnection(ConnectionViewMoedel cvm)
        {
            Pin.AssignConnection(cvm.connection);
            connectionViewMoedels.Add(cvm);
        }
    }
}
