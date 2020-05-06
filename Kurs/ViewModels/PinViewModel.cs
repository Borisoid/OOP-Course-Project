using System;
using System.Collections.Generic;
using System.Text;

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

        }
        public readonly Pin Pin;
        public PinView PinView { get; private set; }
    }
}
