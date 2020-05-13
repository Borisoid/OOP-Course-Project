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
using Kurs.Models;
using Kurs.ViewModels;

namespace Kurs.Views
{
    /// <summary>
    /// Логика взаимодействия для PinView.xaml
    /// </summary>
    public partial class PinView : UserControl, INotifyPropertyChanged
    {
        public PinView(PinType type)
        {
            InitializeComponent();

            //This = this;

            _type = type;
            RectHeight = Properties.AppSettings.Default.BasePinViewRectHeight;
            RectWidth = Properties.AppSettings.Default.BasePinViewRectWidth;

            //canvas.DataContext = this;

            #region <>

                //< Line    X1 = "{Binding LineX1}" X2 = "{Binding LineX2}"
                //          Canvas.Top = "{Binding LineY}"
                //          Canvas.Left = "0"
                //          Stroke = "Black"
                //          StrokeThickness = "2"
                //          IsHitTestVisible = "False" />
                //  < Ellipse   Height = "{Binding CircleDiameter}" Width = "{Binding CircleDiameter}"
                //              Canvas.Top = "{Binding CircleTopPositionInCanvas}"
                //              Canvas.Left = "{Binding CircleLeftPositionInCanvas}"
                //              Stroke = "Black"
                //              StrokeThickness = "2" />

            #endregion
        }
        public PinView()
        {
            InitializeComponent();

            //This = this;

            RectHeight = Properties.AppSettings.Default.BasePinViewRectHeight;
            RectWidth = Properties.AppSettings.Default.BasePinViewRectWidth;
        }

        #region Properties

        public int RectHeight
        {
            get { return _rectHeight; }
            set
            {
                _rectHeight = value;
                Notify();
            }
        }
        public int RectWidth
        {
            get { return _rectWidth; }
            set
            {
                _rectWidth = value;
                Notify();
            }
        }
        public PinType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                Notify();
            }
        }

        public int CircleRadius
        {
            get { return RectHeight / 2; }
        }
        public int CircleDiameter
        {
            get { return CircleRadius * 2; }
        }
        public int LineLength
        {
            get { return RectWidth - CircleRadius; }
        }
        public int LineY
        {
            get { return RectHeight / 2; }
        }
        public int LineX1
        {
            get { return (_type == PinType.Input) ? CircleRadius : 0; }
        }
        public int LineX2
        {
            get { return LineX1 + LineLength; }
        }
        public int ConnectionPointX
        {
            get { return (_type == PinType.Input) ? LineX1 : LineX2; }
        }
        public int ConnectionPointY
        {
            get { return LineY; }
        }
        public int CircleTopPositionInCanvas
        {
            get { return 0; }
        }
        public int CircleLeftPositionInCanvas
        {
            get
            {
                if (_type == PinType.Output)
                    return RectWidth - (CircleRadius * 2);
                else
                    return 0;
            }
        }

        #endregion

        #region DependencyProperties

        //private static DependencyProperty ThisProperty;
        //static PinView()
        //{
        //    ThisProperty = DependencyProperty.Register("This", typeof(PinView), typeof(PinView));
        //}
        //public PinView This
        //{
        //    get { return (PinView)GetValue(ThisProperty); }
        //    set { SetValue(ThisProperty, value); }
        //}

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

        public void Notify()
        {
            OnPropertyChanged("CircleRadius");
            OnPropertyChanged("CircleDiameter");
            OnPropertyChanged("LineLength");
            OnPropertyChanged("LineY");
            OnPropertyChanged("LineX1");
            OnPropertyChanged("LineX2");
            OnPropertyChanged("ConnectionPointX");
            OnPropertyChanged("ConnectionPointY");
            OnPropertyChanged("CircleTopPositionInCanvas");
            OnPropertyChanged("CircleLeftPositionInCanvas");
        }

        #endregion

        #region NestedEnum

        public enum PinType
        {
            Input,
            Output
        }

        #endregion

        #region Data

        private int _rectHeight;
        private int _rectWidth;
        public PinType _type { get; set; }

        #endregion

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            PinViewModel th = this.DataContext as PinViewModel;
            if (th != null)
                th.PinView = this;
        }
    }
}
