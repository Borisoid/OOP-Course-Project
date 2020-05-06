using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ComponentModel;

using Kurs.ViewModels;

namespace Kurs.Views
{
    /// <summary>
    /// Логика взаимодействия для GateView.xaml
    /// </summary>
    public partial class GateView : UserControl, INotifyPropertyChanged
    {
        public GateView(GateViewModel gvm)
        {
            InitializeComponent();

            gateViewModel = gvm;

            HeightPerPin = Properties.AppSettings.Default.BaseGateViewHeightPerPin;
            RectWidth = Properties.AppSettings.Default.BaseGateViewWidth;
            PinWidth = Properties.AppSettings.Default.BasePinViewRectWidth;
            ScaleFactor = 1;
        }

        public GateViewModel gateViewModel;


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public int InputsNumber
        {
            get { return gateViewModel.InputsNumber; }
        }
        public int HeightPerPin
        {
            get { return _baseHeightPerPin * _scaleFactor; }
            set
            {
                _baseHeightPerPin = value;
                OnPropertyChanged("HeightPerPin");
                OnPropertyChanged("RectHeight");
            }
        }
        public int RectHeight
        {
            get { return _baseHeightPerPin * InputsNumber * _scaleFactor; }
        }
        public int RectWidth
        {
            get { return _baseRectWidth * _scaleFactor; }
            set
            {
                _baseRectWidth = value;
                OnPropertyChanged("RectWidth");
            }
        }
        public int ScaleFactor
        {
            get { return _scaleFactor; }
            set
            {
                _scaleFactor = value;
                OnPropertyChanged("ScaleFactor");
                OnPropertyChanged("RectHeight");
                OnPropertyChanged("RectWidth");
            }
        }
        public int PinWidth
        {
            get { return _pinWidth; }
            set
            {
                _pinWidth = value;
                OnPropertyChanged("PinWidth");
            }
        }

        public int _baseHeightPerPin;
        public int _baseRectWidth;
        public int _scaleFactor;
        public int _pinWidth;
    }
}
