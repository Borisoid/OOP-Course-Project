using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

using Kurs.Models;
using Kurs.Commands;
using System.Windows;

namespace Kurs.ViewModels
{
    class MainVewModel
    {
        public MainVewModel()
        {
            itemsPicker = new ItemsPickerViewModel();
            workArea = new WorkAreaViewModel();

            workArea.itemsPicker = itemsPicker;

            #region Testing

            //bool[] ar = { true, false, false, false, true, true, true, true };
            //var g = new Gate("3AND", 3, ar);
            //var gv = new GateViewModel(g);

            //bool[] ar2 = { true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, false };
            //var g2 = new Gate("5AND", 5, ar2);
            //var gv2 = new GateViewModel(g2);

            //workArea.GateList.Add(new WorkAreaViewModel.GateViewModelWithCoordinates(gv2, 20, 60));
            //workArea.GateList.Add(new WorkAreaViewModel.GateViewModelWithCoordinates(gv, 150, 300));

            #endregion
        }

        public ItemsPickerViewModel itemsPicker { get; set; }
        public WorkAreaViewModel workArea { get; set; }

        #region Commands

        private DelegateCommand<GateViewModel> RunCommand;

        public ICommand runCommand
        {
            get
            {
                if (RunCommand == null)
                {
                    RunCommand = new DelegateCommand<GateViewModel>(Run);
                }
                return RunCommand;
            }
        }

        private void Run(GateViewModel gvm)
        {
            foreach(WorkAreaViewModel.GateViewModelWithCoordinates g in workArea.GateList)
            {
                if(g.gateViewModel.Name == "SOURCE")
                g.gateViewModel.gate.OutputValue = true;
            }
            foreach (WorkAreaViewModel.GateViewModelWithCoordinates g in workArea.GateList)
            {
                if (g.gateViewModel.Name == "READER")
                {
                    if (!LookForCycle(g.gateViewModel.gate, new List<IHavePins>()))
                        MessageBox.Show(g.gateViewModel.gate.OutputValue.ToString());
                    else
                        MessageBox.Show("there's a cycle bitch");
                }
            }
        }

        #endregion

        /// <summary>
        /// Returns true if graph has cycle.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool LookForCycle(IHavePins g, List<IHavePins> list)
        {
            List<IHavePins> Marked = new List<IHavePins>(list);

            foreach (IHavePins ihp in Marked)
                if (Object.ReferenceEquals(g, ihp))
                    return true;

            Marked.Add(g);
                
            foreach(InputPin ip in g.InputPins)
            {
                foreach(Connection c in ip.Connections)
                {
                    if (LookForCycle(c.OutputPin.Owner, Marked))
                        return true;
                }
            }

            return false;
        }
    }
}
