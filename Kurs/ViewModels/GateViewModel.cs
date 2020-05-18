using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using Kurs.Models;
using Kurs.Views;

namespace Kurs.ViewModels
{
    public class GateViewModel : ViewModelBase, ICloneable
    {
        #region Constructors

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

        }

        #endregion

        #region Properties

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

        public List<PinViewModel> AllPins
        {
            get
            {
                var res = new List<PinViewModel>(inputPins);
                res.Add(outputPin);
                return res;
            }
        }

        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged("Selected");
            }
        }

        #endregion

        #region IClonnable

        public object Clone()
        {
            Gate copyGate = new Gate(gate.Name, gate.InputsNumber, gate.Function);
            GateViewModel copy = new GateViewModel(copyGate);

            return copy;
        }

        #endregion



        #region Data

        public readonly Gate gate;
        public bool _selected;

        #endregion
    }
}
