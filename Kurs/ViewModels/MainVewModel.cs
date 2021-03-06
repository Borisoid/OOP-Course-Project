﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Kurs.Models;
using Kurs.Commands;
using System.Windows;
using Microsoft.Win32;
using Kurs.Views;

namespace Kurs.ViewModels
{
    class MainVewModel
    {
        public MainVewModel()
        {
            itemsPicker = new ItemsPickerViewModel();
            workArea = new WorkAreaViewModel();

            workArea.itemsPicker = itemsPicker;
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

            workArea.Renumber();

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
                    MessageBox.Show("There is a cycle. You cannot continue while it is here.");
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
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "kurs files (*.kurs)|*.kurs|dat files (*.dat)|*.dat";
            dialog.DefaultExt = "kurs";
            dialog.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\KursCircuit";
            if (dialog.ShowDialog() == true)
            {
                var path = dialog.FileName;

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(fs, new WorkAreaSerialization(workArea));
                }
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
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "kurs files (*.kurs)|*.kurs|dat files (*.dat)|*.dat";
            dialog.DefaultExt = "kurs";
            dialog.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (dialog.ShowDialog() == true)
            {
                var path = dialog.FileName;

                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    WorkAreaSerialization ser = formatter.Deserialize(fs) as WorkAreaSerialization;

                    workArea.Load(ser);

                }
            }
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
