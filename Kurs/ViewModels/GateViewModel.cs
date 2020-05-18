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

        public GateViewModel(Gate g, bool NumLabelVisible = false, bool ValLabelVisible = false)
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

            NumberLabelVisible = NumLabelVisible;
            ValueLabelVisible = ValLabelVisible;
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

        public string NumberLabel
        {
            get { return _numberLabel; }
            set
            {
                _numberLabel = value;
                OnPropertyChanged("NumberLabel");
            }
        }
        public string _numberLabel;

        public bool NumberLabelVisible
        {
            get { return _numberLabelVisible; }
            set
            {
                _numberLabelVisible = value;
                OnPropertyChanged("NumberLabelVisible");
            }
        }
        public bool _numberLabelVisible;

        public string ValueLabel
        {
            get { return _valueLabel; }
            set
            {
                _valueLabel = value;
                OnPropertyChanged("ValueLabel");
            }
        }
        public string _valueLabel;

        public bool ValueLabelVisible
        {
            get { return _valueLabelVisible; }
            set
            {
                _valueLabelVisible = value;
                OnPropertyChanged("ValueLabelVisible");
            }
        }
        public bool _valueLabelVisible;

        public List<PinViewModel> AllPins
        {
            get
            {
                var res = new List<PinViewModel>(inputPins);
                res.Add(outputPin);
                return res;
            }
        }

        public bool ShowBottomLabels
        {
            get { return _showBottomLabels; }
            set
            {
                _showBottomLabels = value;
                OnPropertyChanged("ShowBottomLabels");
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
        public bool _showBottomLabels;

        #endregion
    }
}
