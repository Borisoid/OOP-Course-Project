using Kurs.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace Kurs.ViewModels
{
    public class WorkAreaViewModel : ViewModelBase
    {
        public WorkAreaViewModel()
        {
            GateList = new BindingList<GateViewModelWithCoordinates>();
            ConnectionList = new BindingList<ConnectionViewModel>();
        }

        public class GateViewModelWithCoordinates
        {
            public GateViewModelWithCoordinates(GateViewModel gvm, int x, int y)
            {
                gateViewModel = gvm;
                X = x;
                Y = y;
            }

            public GateViewModel gateViewModel { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }

        public BindingList<GateViewModelWithCoordinates> GateList { get; set; }
        public BindingList<ConnectionViewModel> ConnectionList { get; set; }
    }
}
