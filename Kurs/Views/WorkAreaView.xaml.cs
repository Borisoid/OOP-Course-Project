using Kurs.ViewModels;
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

namespace Kurs.Views
{
    /// <summary>
    /// Логика взаимодействия для WorkAreaView.xaml
    /// </summary>
    public partial class WorkAreaView : UserControl
    {
        public WorkAreaView()
        {
            InitializeComponent();
        }
        private void canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PointingTo = e.GetPosition(canvas);
        }


        private static DependencyProperty PointingToProperty;
        static WorkAreaView()
        {
            PointingToProperty = DependencyProperty.Register("PointingTo", typeof(Point), typeof(WorkAreaView));
        }
        public Point PointingTo
        {
            get { return (Point)GetValue(PointingToProperty); }
            set { SetValue(PointingToProperty, value); }
        }
    }
}
