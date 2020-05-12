using Kurs.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace Kurs.ViewModels
{
    public class WorkAreaViewModel : ViewModelBase
    {
        #region Constructors

        public WorkAreaViewModel(WorkAreaView view)
        {
            View = view;
            View.DataContext = this;

            GateList = new BindingList<GateViewModelWithCoordinates>();
            ConnectionList = new BindingList<ConnectionViewModelWithCoordinates>();
        }

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

            #endregion

            #region Data

            public int _x;
            public int _y;

            #endregion
        }

        public class ConnectionViewModelWithCoordinates : INotifyPropertyChanged
        {
            #region Constructors

            public ConnectionViewModelWithCoordinates(ConnectionViewModel con, WorkAreaView view)
            {
                connectionViewModel = con;
                View = view;

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
                    DependencyObject container = View.canvas;
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
                    DependencyObject container = View.canvas;
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

        public WorkAreaView View;
        public BindingList<GateViewModelWithCoordinates> GateList { get; set; }
        public BindingList<ConnectionViewModelWithCoordinates> ConnectionList { get; set; }

        #endregion
    }
}


/*PinView Pin1 = connectionViewModel.PinView1;
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
                    MessageBox.Show(relativeLocation.ToString());

                    return relativeLocation;*/
