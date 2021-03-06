﻿using Kurs.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;

using Kurs.Commands;
using Kurs.Models;

namespace Kurs.ViewModels
{
    public class WorkAreaViewModel : ViewModelBase
    {
        #region Constructors

        public WorkAreaViewModel()
        {

            GateList = new BindingList<GateViewModelWithCoordinates>();
            ConnectionList = new BindingList<ConnectionViewModelWithCoordinates>();

            InputPins = new BindingList<PinViewModel>();
            OutputPins = new BindingList<PinViewModel>();

            InputPins.ListChanged += InputPinsChanged;
            OutputPins.ListChanged += OutputPinsChanged;

        }

        #endregion

        #region Pin selection & connection

        /// <summary>
        /// Adds gate with coords in work area; 'registers' all its pins to allow selection and connection.
        /// </summary>
        /// <param name="gvmwc"></param>
        public void AddGate(GateViewModelWithCoordinates gvmwc)
        {
            if (gvmwc.gateViewModel.Name == "SOURCE" || gvmwc.gateViewModel.Name == "READER")
            {
                gvmwc.gateViewModel.ShowBottomLabels = true;
                gvmwc.gateViewModel.NumberLabelVisible = true;
                
            }

            GateList.Add(gvmwc);

            if (gvmwc.gateViewModel.Name == "SOURCE" || gvmwc.gateViewModel.Name == "READER")
                Renumber();

            foreach (PinViewModel p in gvmwc.gateViewModel.inputPins)
                InputPins.Add(p);

            OutputPins.Add(gvmwc.gateViewModel.outputPin);
        }
        public void Renumber()
        {
            int i = 0, j = 0;
            foreach (GateViewModelWithCoordinates gvmwc in GateList)
            {
                if (gvmwc.gateViewModel.Name == "SOURCE")
                {
                    gvmwc.gateViewModel.NumberLabel = (i).ToString();
                    i++;
                }
                if (gvmwc.gateViewModel.Name == "READER")
                {
                    gvmwc.gateViewModel.NumberLabel = (j).ToString();
                    j++;
                }
            }
        }

        public void InputPinsChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                if (InputPins[e.NewIndex].IsSelected)
                    SelectedInputPin = InputPins[e.NewIndex];
            }
        }
        public void OutputPinsChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                if (OutputPins[e.NewIndex].IsSelected)
                    SelectedOutputPin = OutputPins[e.NewIndex];
            }
        }

        public BindingList<PinViewModel> InputPins { get; set; }
        public BindingList<PinViewModel> OutputPins { get; set; }

        public PinViewModel SelectedInputPin
        {
            get { return _selectedInputPin; }
            set
            {
                if (_selectedInputPin != null)
                    _selectedInputPin.IsSelected = false;
                _selectedInputPin = value;
                TryConnect();
            }
        }
        public PinViewModel _selectedInputPin;

        public PinViewModel SelectedOutputPin
        {
            get { return _selectedOutputPin; }
            set
            {
                if (_selectedOutputPin != null)
                    _selectedOutputPin.IsSelected = false;
                _selectedOutputPin = value;
                TryConnect();
            }
        }
        public PinViewModel _selectedOutputPin;

        public void TryConnect()
        {
            if (SelectedInputPin == null || SelectedOutputPin == null)
                return;
            if (SelectedInputPin.Pin.Owner == SelectedOutputPin.Pin.Owner)
            {
                SelectedInputPin = null;
                SelectedOutputPin = null;
                return;
            }
            Connection con = new Connection();
            SelectedInputPin.Pin.AssignConnection(con);
            SelectedOutputPin.Pin.AssignConnection(con);
            ConnectionViewModel cvm = new ConnectionViewModel(con);
            ConnectionList.Add(new ConnectionViewModelWithCoordinates(cvm));
            SelectedInputPin = null;
            SelectedOutputPin = null;

            return;
        }

        public event Action GatesLoaded;

        #endregion

        #region NestedClasses

        public class GateViewModelWithCoordinates : INotifyPropertyChanged
        {
            #region Constructors

            public GateViewModelWithCoordinates(GateViewModel gvm, int x, int y)
            {
                gateViewModel = gvm;
                X = x;
                Y = y;
            }

            #endregion

            #region INotifyPropertyChanged

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler handler = PropertyChanged;

                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            #endregion

            #region Properties

            public int X
            {
                get { return _x; }
                set
                {
                    _x = value;
                    OnPropertyChanged("X");
                }
            }
            public int Y
            {
                get { return _y; }
                set
                {
                    _y = value;
                    OnPropertyChanged("Y");
                }
            }
            public GateViewModel gateViewModel { get; set; }
            public bool IsSelected
            {
                get { return gateViewModel.Selected; }
                set { gateViewModel.Selected = value; }
            }

            #endregion

            #region Data

            public int _x;
            public int _y;

            #endregion
        }

        public class ConnectionViewModelWithCoordinates : INotifyPropertyChanged
        {
            #region Constructors

            public ConnectionViewModelWithCoordinates(ConnectionViewModel con)
            {
                connectionViewModel = con;

                RecalcCoordinates();
            }

            #endregion

            #region Methods

            public void RecalcCoordinates()
            {
                {
                    PinView Pin1 = connectionViewModel.PinView1;
                    System.Windows.Point ConnectionPointInPin = new System.Windows.Point();
                    ConnectionPointInPin.X = Pin1.ConnectionPointX;
                    ConnectionPointInPin.Y = Pin1.ConnectionPointY;

                    //WorkArea "Canvas"
                    DependencyObject container = Pin1;
                    while (container.GetType() != typeof(Canvas))
                    {
                        container = VisualTreeHelper.GetParent(container);
                    }
                    System.Windows.Point relativeLocation = Pin1.TranslatePoint(ConnectionPointInPin, container as UIElement);

                    X1 = relativeLocation.X;
                    Y1 = relativeLocation.Y;
                }
                {
                    PinView Pin2 = connectionViewModel.PinView2;
                    System.Windows.Point ConnectionPointInPin = new System.Windows.Point();
                    ConnectionPointInPin.X = Pin2.ConnectionPointX;
                    ConnectionPointInPin.Y = Pin2.ConnectionPointY;

                    //WorkArea "Canvas"
                    DependencyObject container = Pin2;
                    while (container.GetType() != typeof(Canvas))
                    {
                        container = VisualTreeHelper.GetParent(container);
                    }
                    System.Windows.Point relativeLocation = Pin2.TranslatePoint(ConnectionPointInPin, container as UIElement);

                    X2 = relativeLocation.X;
                    Y2 = relativeLocation.Y;
                }
            }

            #endregion

            #region Properties

            public double X1
            {
                get { return _x1; }
                set
                {
                    _x1 = value;
                    OnPropertyChanged("X1");
                }
            }
            public double Y1
            {
                get { return _y1; }
                set
                {
                    _y1 = value;
                    OnPropertyChanged("Y1");
                }
            }
            public double X2
            {
                get { return _x2; }
                set
                {
                    _x2 = value;
                    OnPropertyChanged("X2");
                }
            }
            public double Y2
            {
                get { return _y2; }
                set
                {
                    _y2 = value;
                    OnPropertyChanged("Y2");
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

            #region INotifyPropertyChanged

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler handler = PropertyChanged;

                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            #endregion

            #region Data

            public ConnectionViewModel connectionViewModel;
            public bool _selected;
            public double _x1;
            public double _y1;
            public double _x2;
            public double _y2;

            #endregion
        }

        #endregion

        #region Data

        public BindingList<GateViewModelWithCoordinates> GateList { get; set; }
        public BindingList<ConnectionViewModelWithCoordinates> ConnectionList { get; set; }
        public ItemsPickerViewModel itemsPicker;

        #endregion

        #region Commands

        #region PlaceCommand

        /// <summary>
        /// Places gate selected in ItemsPicker at the coords of mouse.
        /// </summary>
        private DelegateCommand<System.Windows.Point> PlaceCommand;

        public ICommand placeCommand
        {
            get
            {
                if (PlaceCommand == null)
                {
                    PlaceCommand = new DelegateCommand<System.Windows.Point>(Place);
                }
                return PlaceCommand;
            }
        }

        private void Place(System.Windows.Point p)
        {
            if (itemsPicker.SelectedGateViewModel != null)
                AddGate(new GateViewModelWithCoordinates(
                            (GateViewModel)itemsPicker.SelectedGateViewModel.Clone(),
                            (int)p.X,
                            (int)p.Y)
                        );
        }

        #endregion

        #region SelectGateCommand

        private DelegateCommand<GateViewModel> selectGateCommand;

        public ICommand SelectGateCommand
        {
            get
            {
                if (selectGateCommand == null)
                {
                    selectGateCommand = new DelegateCommand<GateViewModel>(SelectGate);
                }
                return selectGateCommand;
            }
        }

        private void SelectGate(GateViewModel gvm)
        {
            Deselect();
            foreach(GateViewModelWithCoordinates gvmwc in GateList)
            {
                if (gvmwc.gateViewModel == gvm)
                {
                    SelectedGateViewModelWithCoordinates = gvmwc;
                    break;
                }
            }
        }


        public GateViewModelWithCoordinates SelectedGateViewModelWithCoordinates
        {
            get { return _selectedGateViewModelWithCoordinates; }
            set
            {
                if (_selectedGateViewModelWithCoordinates != null)
                    _selectedGateViewModelWithCoordinates.IsSelected = false;
                _selectedGateViewModelWithCoordinates = value;
                if (_selectedGateViewModelWithCoordinates != null)
                    _selectedGateViewModelWithCoordinates.IsSelected = true;
            }
        }
        public GateViewModelWithCoordinates _selectedGateViewModelWithCoordinates;

        #endregion

        #region SelectConnectionCommand

        private DelegateCommand<ConnectionViewModelWithCoordinates> selectConnectionCommand;
        public ICommand SelectConnectionCommand
        {
            get
            {
                if (selectConnectionCommand == null)
                {
                    selectConnectionCommand = new DelegateCommand<ConnectionViewModelWithCoordinates>(SelectConnection);
                }
                return selectConnectionCommand;
            }
        }
        private void SelectConnection(ConnectionViewModelWithCoordinates cvmwc)
        {
            Deselect();
            SelectedConnectionViewModelWithCoordinates = cvmwc;
        }


        public ConnectionViewModelWithCoordinates SelectedConnectionViewModelWithCoordinates
        {
            get { return _selectedConnectionViewModelWithCoordinates; }
            set
            {
                if (_selectedConnectionViewModelWithCoordinates != null)
                    _selectedConnectionViewModelWithCoordinates.Selected = false;
                _selectedConnectionViewModelWithCoordinates = value;
                if(_selectedConnectionViewModelWithCoordinates != null)
                    _selectedConnectionViewModelWithCoordinates.Selected = true;
            }
        }
        public ConnectionViewModelWithCoordinates _selectedConnectionViewModelWithCoordinates;

        #endregion

        #region DeleteCommand

        /// <summary>
        /// Deletes selected connection or gate with all its connections
        /// </summary>
        private DelegateCommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new DelegateCommand(Delete);
                }
                return deleteCommand;
            }
        }
        private void Delete()
        {
            if(SelectedConnectionViewModelWithCoordinates != null)
            {
                SelectedConnectionViewModelWithCoordinates.connectionViewModel.Disconnect();
                ConnectionList.Remove(SelectedConnectionViewModelWithCoordinates);
                SelectedConnectionViewModelWithCoordinates = null;
            }
            if(SelectedGateViewModelWithCoordinates != null)
            {
                foreach (PinViewModel p in SelectedGateViewModelWithCoordinates.gateViewModel.AllPins)
                {
                    var cvms = p.connectionViewMoedels;
                    foreach (ConnectionViewModel c in p.connectionViewMoedels)
                    {
                        foreach (ConnectionViewModelWithCoordinates cvmwc in ConnectionList)
                            if (Object.ReferenceEquals(c, cvmwc.connectionViewModel))
                            {
                                ConnectionList.Remove(cvmwc);
                                break;
                            }
                    }
                    while (cvms.Count != 0)
                    {
                        cvms[0].Disconnect();
                        cvms.RemoveAt(0);
                    }
                }
                GateList.Remove(SelectedGateViewModelWithCoordinates);
                SelectedGateViewModelWithCoordinates = null;
            }
        }

        #endregion

        /// <summary>
        /// Deselects gates and connections.
        /// </summary>
        public void Deselect()
        {
            SelectedConnectionViewModelWithCoordinates = null;
            SelectedGateViewModelWithCoordinates = null;
        }

        #endregion

        #region Methods

        public void Load(WorkAreaSerialization ser)
        {
            #region Clear

            ConnectionList.Clear();
            GateList.Clear();

            InputPins.Clear();
            OutputPins.Clear();

            SelectedInputPin = null;
            SelectedOutputPin = null;

            #endregion

            foreach (WorkAreaSerialization.GateModelWithCoordinates g in ser.GateList)
            {
                AddGate(new GateViewModelWithCoordinates(new GateViewModel(g.gate), g.X, g.Y));
            }

            GatesLoaded();

            RestoreConnections();
        }

        public void RestoreConnections()
        {
            #region Restore connection view models

            var hs = new HashSet<Connection>();
            foreach (GateViewModelWithCoordinates g in GateList)
            {
                foreach (PinViewModel p in g.gateViewModel.AllPins)
                {
                    foreach (Connection cvm in p.Pin.Connections)
                        hs.Add(cvm);
                }
            }

            foreach (Connection con in hs)
            {
                ConnectionViewModel cvm = new ConnectionViewModel(con);
                ConnectionList.Add(new ConnectionViewModelWithCoordinates(cvm));
            }

            #endregion
        }

        #endregion
    }

    [Serializable]
    public class WorkAreaSerialization
    {
        public WorkAreaSerialization(WorkAreaViewModel source)
        {
            GateList = new List<GateModelWithCoordinates>();

            foreach (WorkAreaViewModel.GateViewModelWithCoordinates g in source.GateList)
            {
                GateList.Add(new GateModelWithCoordinates(g.X, g.Y, g.gateViewModel.gate));
            }
        }

        public List<GateModelWithCoordinates> GateList { get; set; }

        [Serializable]
        public class GateModelWithCoordinates
        {
            public GateModelWithCoordinates(int x, int y, Gate g)
            {
                X = x;
                Y = y;
                gate = g;
            }

            public int X;
            public int Y;
            public Gate gate;
        }
    }
}