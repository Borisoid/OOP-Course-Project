using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Kurs.Models;
using Kurs.ViewModels;
using Kurs.Views;

namespace Kurs
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //var Pin = new OutputPin(null);
            //var vm = new PinViewModel(Pin);
            //var view = vm.PinView;
            //var w = new MainWindow(view);

            bool[] ar = { true, false, false, false, true, true, true, true };
            var g = new Gate("3AND", 3, ar);
            var gv = new GateViewModel(g);

            bool[] ar2 = { true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, false };
            var g2 = new Gate("5AND", 5, ar2);
            var gv2 = new GateViewModel(g2);

            var wa = new WorkAreaViewModel();
            

            

            var vm = new WorkAreaView();
            vm.DataContext = wa;
            //var w = new MainWindow(gv2.gateView);
            var w = new MainWindow(vm);

            w.Show();

            wa.GateList.Add(new WorkAreaViewModel.GateViewModelWithCoordinates(gv2, 20, 30));
            wa.GateList.Add(new WorkAreaViewModel.GateViewModelWithCoordinates(gv, 150, 300));

            var con = new ConnectionViewModel(gv.inputPins[1], gv2.outputPin);
            wa.ConnectionList.Add(con);
        }
    }
}
