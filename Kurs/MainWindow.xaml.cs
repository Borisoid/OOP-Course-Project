﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Kurs.Views;

namespace Kurs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(PinView pw)
        {
            InitializeComponent();
            wnd.Children.Add(pw);
        }
        public MainWindow(GateView gv)
        {
            InitializeComponent();
            wnd.Children.Add(gv);
        }
        public MainWindow(WorkAreaView wa)
        {
            InitializeComponent();
            wnd.Children.Add(wa);
        }
    }
}
