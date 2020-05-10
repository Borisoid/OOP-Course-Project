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
    public partial class ConnectionView : UserControl
    {
        public ConnectionView()
        {
            InitializeComponent();
        }
        public PinView Pin1;
        
        public PinView Pin2;

        public Point Point1
        {
            get
            {
                if (Pin1 == null)
                    return new Point();
                Point ConnectionPointInPin = new Point();
                ConnectionPointInPin.X = Pin1.ConnectionPointX;
                ConnectionPointInPin.Y = Pin1.ConnectionPointY;

                //WorkArea "Canvas"
                DependencyObject container = Pin1;
                while (container.GetType() != typeof(Canvas))
                {
                    container = VisualTreeHelper.GetParent(container);
                }
                Point relativeLocation = Pin1.TranslatePoint(ConnectionPointInPin, container as UIElement);
                MessageBox.Show(relativeLocation.ToString());

                return relativeLocation;
            }
        }
        public Point Point2
        {
            get
            {
                if (Pin2 == null)
                    return new Point();
                Point ConnectionPointInPin = new Point();
                ConnectionPointInPin.X = Pin2.ConnectionPointX;
                ConnectionPointInPin.Y = Pin2.ConnectionPointY;

                //WorkArea "Canvas"
                DependencyObject container = Pin2;
                while (container.GetType() != typeof(Canvas))
                {
                    container = VisualTreeHelper.GetParent(container);
                }
                Point relativeLocation = Pin2.TranslatePoint(ConnectionPointInPin, container as UIElement);
                MessageBox.Show(relativeLocation.ToString());

                return relativeLocation;
            }
        }
        public Point IntermediatePoint1
        {
            get
            {
                double x = Point1.X + ((Point2.X - Point1.X) / 2);
                double y = Point1.Y;

                return new Point(x, y);
            }
        }
        public Point IntermediatePoint2
        {
            get
            {
                double x = Point1.X + ((Point2.X - Point1.X) / 2);
                double y = Point2.Y;

                return new Point(x, y);
            }
        }

        public PointCollection Points
        {
            get
            {
                var pc = new PointCollection();
                pc.Add(Point1);
                pc.Add(IntermediatePoint1);
                pc.Add(IntermediatePoint2);
                pc.Add(Point2);

                return pc;
            }
        }
    }
}
