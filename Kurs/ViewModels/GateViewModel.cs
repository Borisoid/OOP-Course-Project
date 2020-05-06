using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using Kurs.Models;
using Kurs.Views;

namespace Kurs.ViewModels
{
    public class GateViewModel : ViewModelBase
    {
        public readonly Gate gate;
        public readonly GateView gateView;
        public GateViewModel(Gate g)
        {
            if (g == null)
                throw new ArgumentException("GateViewModel recieved null gate param.");

            gate = g;

            #region PinViewModels init & fill

            inputPins = new BindingList<PinViewModel>();
            foreach(InputPin ip in g.InputPins)
                inputPins.Add(new PinViewModel(ip));

            outputPin = new PinViewModel(g.OutputPin);

            #endregion

            gateView = new GateView(this);
            gateView.DataContext = this;

            
        }

        public BindingList<PinViewModel> inputPins { get; set; }
        public PinViewModel outputPin { get; set; }

        public string Name
        { 
            get { return gate.Name; }
        }
        public int InputsNumber
        {
            get { return gate.InputsNumber; }
        }


        

    }
}
