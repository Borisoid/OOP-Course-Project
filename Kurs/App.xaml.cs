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
            var g = new Gate("2AND", 3, ar);
            var gv = new GateViewModel(g);
            var w = new MainWindow(gv.gateView);
            w.Show();
        }
    }
}
