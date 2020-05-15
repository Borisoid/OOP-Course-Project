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

        public void AddGate(GateViewModelWithCoordinates gvmwc)
        {
            GateList.Add(gvmwc);

            foreach (PinViewModel p in gvmwc.gateViewModel.inputPins)
                InputPins.Add(p);

            OutputPins.Add(gvmwc.gateViewModel.outputPin);
        }

        public void InputPinsChanged(object sender, ListChangedEventArgs e)
        {
            if(e.ListChangedType == ListChangedType.ItemChanged)
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
            if(SelectedInputPin.Pin.Owner == SelectedOutputPin.Pin.Owner)
            {
                SelectedInputPin = null;
                SelectedOutputPin = null;
                return;
            }
            ConnectionViewModel con = new ConnectionViewModel(SelectedInputPin, SelectedOutputPin);
            ConnectionList.Add(new ConnectionViewModelWithCoordinates(con));
            SelectedInputPin = null;
            SelectedOutputPin = null;
            return;
        }

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
                    while(container.GetType() != typeof(Canvas))
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
            public WorkAreaView View;
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

        public BindingList<PinViewModel> InputPins { get; set; }
        public BindingList<PinViewModel> OutputPins { get; set; }
        #endregion

        #region Commands

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
                AddGate(new GateViewModelWithCoordinates((GateViewModel)itemsPicker.SelectedGateViewModel.Clone(), (int)p.X, (int)p.Y));
        }

        #endregion



        public ItemsPickerViewModel itemsPicker;
    }
}