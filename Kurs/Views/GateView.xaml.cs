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
using Kurs.Models;

namespace Kurs.Views
{
    /// <summary>
    /// Логика взаимодействия для GateView.xaml
    /// </summary>
    public partial class GateView : UserControl, INotifyPropertyChanged
    {
        #region Constructors

        //public GateView(GateViewModel gvm)
        //{
        //    gateViewModel = gvm;

        //    InitializeComponent();

        //    HeightPerPin = Properties.AppSettings.Default.BaseGateViewHeightPerPin;
        //    RectWidth = Properties.AppSettings.Default.BaseGateViewWidth;
        //    PinWidth = Properties.AppSettings.Default.BasePinViewRectWidth;
        //    ScaleFactor = 1;
        //}
        public GateView()
        {
            InitializeComponent();

            HeightPerPin = Properties.AppSettings.Default.BaseGateViewHeightPerPin;
            RectWidth = Properties.AppSettings.Default.BaseGateViewWidth;
            PinWidth = Properties.AppSettings.Default.BasePinViewRectWidth;
            ScaleFactor = 1;
        }

        static GateView()
        {
            ShowBottomLabelsProperty = DependencyProperty.Register("ShowBottomLabels", typeof(bool), typeof(GateView));
        }

        #endregion

        //private static DependencyProperty SelectedProperty;
        //static GateView()
        //{
        //    SelectedProperty = DependencyProperty.Register("Selected", typeof(bool), typeof(GateView));
        //}
        //public bool Selected
        //{
        //    get { return (bool)GetValue(SelectedProperty); }
        //    set { SetValue(SelectedProperty, value); OnPropertyChanged("OutlineColor"); }
        //}
        //public SolidColorBrush OutlineColor
        //{
        //    get
        //    {
        //        if(Selected)
        //        {
        //            var b = new SolidColorBrush();
        //            b.Color = Colors.Red;
        //            return b;
        //        }
        //        else
        //        {
        //            var b = new SolidColorBrush();
        //            b.Color = Colors.Black;
        //            return b;
        //        }
        //    }
        //}

        #region Properties

        public int InputsNumber
        {
            get
            {
                if (this.DataContext != null)
                    return (this.DataContext as GateViewModel).InputsNumber;
                else
                    return 0;
            }
        }
        public int HeightPerPin
        {
            get { return _baseHeightPerPin * _scaleFactor; }
            set
            {
                _baseHeightPerPin = value;
                //OnPropertyChanged("HeightPerPin");
                OnPropertyChanged("RectHeight");
                OnPropertyChanged("halfPinHeightHeight");
            }
        }
        public double RectHeight
        {
            get { return _baseHeightPerPin * (InputsNumber == 0 || InputsNumber == 1 ? 1.3 : InputsNumber) * _scaleFactor; }
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
                //OnPropertyChanged("ScaleFactor");
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
        public int OneThirdPinHeight
        {
            get { return _baseHeightPerPin * _scaleFactor / 3; }
        }
        public int BottomLabelHeight
        {
            get
            {
                if (ShowBottomLabels)
                    return 15;
                else
                    return 0;
            }
        }

        #endregion


        private static DependencyProperty ShowBottomLabelsProperty;
        public bool ShowBottomLabels
        {
            get { return (bool)GetValue(ShowBottomLabelsProperty); }
            set
            {
                SetValue(ShowBottomLabelsProperty, value);
                OnPropertyChanged("BottomLabelHeight");
            }
        }


        #region Events

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            //OnPropertyChanged("InputsNumber");
            OnPropertyChanged("RectHeight");
            OnPropertyChanged("BottomLabelHeight");

            if(DataContext != null)
                if ((DataContext as GateViewModel).Name == "READER")
                    RightSide.Visibility = Visibility.Hidden;
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

        public GateViewModel gateViewModel;
        public int _baseHeightPerPin;
        public int _baseRectWidth;
        public int _scaleFactor;
        public int _pinWidth;
        public int _inputsNumber;

        #endregion
    }

    #region ValueConverter

    /// <summary>
    /// Converts value into margin-top (0, value, 0, 0).
    /// </summary>
    public class MarginConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new Thickness(0, System.Convert.ToInt32(value), 0, 0);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    #endregion
}
