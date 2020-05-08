using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Kurs.ViewModels
{
    public class WorkAreaViewModel
    {
        public WorkAreaViewModel()
        {
            GateList = new BindingList<GateViewModelWithCoordinates>();
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
    }
}
