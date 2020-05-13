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
            MainVewModel mainVewModel = new MainVewModel();
            var w = new MainWindow();
            w.DataContext = mainVewModel;


            w.Show();
        }
    }
}
