using System;
using System.Collections.Generic;
using System.ComponentModel;
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

using Kurs.Models;
using Kurs.ViewModels;

namespace Kurs.Views
{
    /// <summary>
    /// Логика взаимодействия для ConnectionView.xaml
    /// </summary>
    public partial class ConnectionView : UserControl, INotifyPropertyChanged
    {
        public ConnectionView()
        {
            InitializeComponent();
        }

        private static DependencyProperty X1Property;
        private static DependencyProperty Y1Property;
        private static DependencyProperty X2Property;
        private static DependencyProperty Y2Property;

        static ConnectionView()
        {
            X1Property = DependencyProperty.Register("X1", typeof(double), typeof(ConnectionView));
            Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(ConnectionView));
            X2Property = DependencyProperty.Register("X2", typeof(double), typeof(ConnectionView));
            Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(ConnectionView));
        }

        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set
            {
                SetValue(X1Property, value);
                OnPropertyChanged("Points");
            }
        }
        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set
            {
                SetValue(Y1Property, value);
                OnPropertyChanged("Points");
            }
        }
        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set
            {
                SetValue(X2Property, value);
                OnPropertyChanged("Points");
            }
        }
        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set
            {
                SetValue(Y2Property, value);
                OnPropertyChanged("Points");
            }
        }

        public Point IntermediatePoint1
        {
            get
            {
                Point Point1 = new Point(X1, Y1);
                Point Point2 = new Point(X2, Y2);

                double x = Point1.X + ((Point2.X - Point1.X) / 2);
                double y = Point1.Y;

                return new Point(x, y);
            }
        }
        public Point IntermediatePoint2
        {
            get
            {
                Point Point1 = new Point(X1, Y1);
                Point Point2 = new Point(X2, Y2);

                double x = Point1.X + ((Point2.X - Point1.X) / 2);
                double y = Point2.Y;

                return new Point(x, y);
            }
        }

        public PointCollection Points
        {
            get
            {
                Point Point1 = new Point(X1, Y1);
                Point Point2 = new Point(X2, Y2);

                var pc = new PointCollection();
                pc.Add(Point1);
                pc.Add(IntermediatePoint1);
                pc.Add(IntermediatePoint2);
                pc.Add(Point2);

                return pc;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged("Points");
        }
    }
}


//public Point Point1
//{
//    get
//    {
//        if (Pin1 == null)
//            return new Point();
//        Point ConnectionPointInPin = new Point();
//        ConnectionPointInPin.X = Pin1.ConnectionPointX;
//        ConnectionPointInPin.Y = Pin1.ConnectionPointY;

//        //WorkArea "Canvas"
//        DependencyObject container = Pin1;
//        while (container.GetType() != typeof(Canvas))
//        {
//            container = VisualTreeHelper.GetParent(container);
//        }
//        Point relativeLocation = Pin1.TranslatePoint(ConnectionPointInPin, container as UIElement);
//        MessageBox.Show(relativeLocation.ToString());

//        return relativeLocation;
//    }
//}
//public Point Point2
//{
//    get
//    {
//        if (Pin2 == null)
//            return new Point();
//        Point ConnectionPointInPin = new Point();
//        ConnectionPointInPin.X = Pin2.ConnectionPointX;
//        ConnectionPointInPin.Y = Pin2.ConnectionPointY;

//        //WorkArea "Canvas"
//        DependencyObject container = Pin2;
//        while (container.GetType() != typeof(Canvas))
//        {
//            container = VisualTreeHelper.GetParent(container);
//        }
//        Point relativeLocation = Pin2.TranslatePoint(ConnectionPointInPin, container as UIElement);
//        MessageBox.Show(relativeLocation.ToString());

//        return relativeLocation;
//    }
//}