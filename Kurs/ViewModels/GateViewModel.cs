using System;
using System.Collections.Generic;
using System.Text;

using Kurs.Models;

namespace Kurs.ViewModels
{
    public class GateViewModel : ViewModelBase
    {
        public readonly Gate gate;
        public GateViewModel(Gate g)
        {
            gate = g;
        }

        public string Name
        { 
            get { return gate.Name; }
        }
        public int InputsNumber
        {
            get { return gate.InputsNumber; }
        }
        public int VisibleRectHeight
        {
            get { return InputsNumber * Properties.AppSettings.Default.VisibleGateViewHeightPerPin; }
        }
        public int VisibleRectWidth
        {
            get { return Properties.AppSettings.Default.VisibleGateViewWidth; }
        }

    }
}
