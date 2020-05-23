using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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

        private DelegateCommand RunCommand;

        public ICommand runCommand
        {
            get
            {
                if (RunCommand == null)
                {
                    RunCommand = new DelegateCommand(Run);
                }
                return RunCommand;
            }
        }
        
        /// <summary>
        /// Goes through all the possible input variations and gets all the output values.
        /// </summary>
        private void Run()
        {
            List<IOValues> IOList = new List<IOValues>();
            List<Gate> Inputs = new List<Gate>();
            List<Gate> Outputs = new List<Gate>();

            foreach (WorkAreaViewModel.GateViewModelWithCoordinates g in workArea.GateList)
            {
                if (g.gateViewModel.Name == "SOURCE")
                    Inputs.Add(g.gateViewModel.gate);
                if (g.gateViewModel.Name == "READER")
                    Outputs.Add(g.gateViewModel.gate);
            }

            foreach(Gate g in Outputs)
            {
                if(LookForCycle(g, new List<IHavePins>() ) )
                {
                    MessageBox.Show("There is a cycle. You cannot continue untill removing it.");
                    return;
                }
            }

            for (int i = 0; i < Math.Pow(2, Inputs.Count); i++)
            {
                //LIST INDEX accurete. Reverse to get bin.
                string ArgumentString = "";

                //setting inputs values so that they form number of current iteration in binary
                foreach (Gate g in Inputs)
                {
                    if ((1 << Inputs.IndexOf(g) & i) != 0)
                    {
                        g.OutputValue = true;
                        ArgumentString = ArgumentString + '1';
                    }
                    else
                    {
                        g.OutputValue = false;
                        ArgumentString = ArgumentString + '0';
                    }
                }

                //LIST INDEX accurate. Reverse to get bin.
                string FunctionString = "";
                foreach(Gate g in Outputs)
                {
                    FunctionString = FunctionString + (g.OutputValue ? '1' : '0');
                }

                //MessageBox.Show(ArgumentString + "_" + FunctionString);

                IOList.Add(new IOValues(ArgumentString, FunctionString));
            }

            var lvm = new LogicalTableViewModel(IOList);
            lvm.ShowView();

            #region CommentedTests

            //foreach (WorkAreaViewModel.GateViewModelWithCoordinates g in workArea.GateList)
            //{
            //    if (g.gateViewModel.Name == "SOURCE")
            //        g.gateViewModel.gate.OutputValue = true;
            //}
            //foreach (WorkAreaViewModel.GateViewModelWithCoordinates g in workArea.GateList)
            //{
            //    if (g.gateViewModel.Name == "READER")
            //    {
            //        if (!LookForCycle(g.gateViewModel.gate, new List<IHavePins>()))
            //            MessageBox.Show(g.gateViewModel.gate.OutputValue.ToString());
            //        else
            //            MessageBox.Show("there's a cycle bitch");
            //    }
            //}

            #endregion
        }


        private DelegateCommand FileSaveCommand;

        public ICommand fileSaveCommand
        {
            get
            {
                if (FileSaveCommand == null)
                {
                    FileSaveCommand = new DelegateCommand(FileSave);
                }
                return FileSaveCommand;
            }
        }
        public void FileSave()
        {
            using (FileStream fs = new FileStream(@"C:\Users\Boris\Desktop\Kurs.dat", FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(fs, new WorkAreaSerialization(workArea));
            }
        }


        private DelegateCommand FileLoadCommand;

        public ICommand fileLoadCommand
        {
            get
            {
                if (FileLoadCommand == null)
                {
                    FileLoadCommand = new DelegateCommand(FileLoad);
                }
                return FileLoadCommand;
            }
        }
        public void FileLoad()
        {
            using (FileStream fs = new FileStream(@"C:\Users\Boris\Desktop\Kurs.dat", FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                WorkAreaSerialization ser = formatter.Deserialize(fs) as WorkAreaSerialization;

                //workArea.Load(ser);
            }
        }

        private DelegateCommand FileTestCommand;

        public ICommand fileTestCommand
        {
            get
            {
                if (FileTestCommand == null)
                {
                    FileTestCommand = new DelegateCommand(FileTest);
                }
                return FileTestCommand;
            }
        }
        public void FileTest()
        {
            //workArea.RestoreConnections();
        }

        #endregion

        /// <summary>
        /// Returns true if circuit (graph) has a cycle.
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

    public class IOValues
    {
        public IOValues(string i, string o)
        {
            IValues = i;
            OValues = o;
        }
        public string IValues { get; set; }
        public string OValues { get; set; }
    }
}
