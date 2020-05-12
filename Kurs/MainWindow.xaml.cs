using System;
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

using Kurs.Models;
using Kurs.ViewModels;
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
        public MainWindow()
        {
            InitializeComponent();
            wavm = new WorkAreaViewModel(workArea);


            bool[] ar = { true, false, false, false, true, true, true, true };
            var g = new Gate("3AND", 3, ar);
            var gv = new GateViewModel(g);

            bool[] ar2 = { true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, false };
            var g2 = new Gate("5AND", 5, ar2);
            var gv2 = new GateViewModel(g2);

            wavm.GateList.Add(new WorkAreaViewModel.GateViewModelWithCoordinates(gv2, 20, 60));
            wavm.GateList.Add(new WorkAreaViewModel.GateViewModelWithCoordinates(gv, 150, 300));
        }

        public WorkAreaViewModel wavm;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var con = new ConnectionViewModel(wavm.GateList[1].gateViewModel.inputPins[1], wavm.GateList[0].gateViewModel.outputPin);
            wavm.ConnectionList.Add(new WorkAreaViewModel.ConnectionViewModelWithCoordinates(con, wavm.View));
        }
    }
}
