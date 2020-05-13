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
        public MainWindow()
        {
            InitializeComponent();
        }

        //public WorkAreaViewModel wavm;

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    var con = new ConnectionViewModel(wavm.GateList[1].gateViewModel.inputPins[1], wavm.GateList[0].gateViewModel.outputPin);
        //    wavm.ConnectionList.Add(new WorkAreaViewModel.ConnectionViewModelWithCoordinates(con));
        //}
    }
}
