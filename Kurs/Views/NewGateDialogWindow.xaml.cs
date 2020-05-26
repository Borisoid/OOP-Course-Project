using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Kurs.ViewModels;

namespace Kurs.Views
{
    /// <summary>
    /// Логика взаимодействия для NewGeteDialogWindow.xaml
    /// </summary>
    public partial class NewGateDialogWindow : Window
    {
        public NewGateDialogWindow(List<string> categories)
        {
            InitializeComponent();

            Categories = new BindingList<CheckedCategory>();
            foreach (string str in categories)
                Categories.Add(new CheckedCategory(str));

            ic.ItemsSource = Categories;
        }

        public class CheckedCategory : ViewModelBase
        {
            public CheckedCategory(string cat, bool check = false)
            {
                Category = cat;
                Checked = check;
            }
            public string Category
            {
                get { return _category; }
                set
                {
                    _category = value;
                    OnPropertyChanged("Category");
                }
            }
            public bool Checked
            {
                get { return _checked; }
                set
                {
                    _checked = value;
                    OnPropertyChanged("Checked");
                }
            }

            public string _category;
            public bool _checked;
        }

        public BindingList<CheckedCategory> Categories { get; set; }

        public List<string> SelectedCategories
        {
            get
            {
                var lst = new List<string>();
                foreach (CheckedCategory cc in Categories)
                    if (cc.Checked)
                        lst.Add(cc.Category);
                return
                    lst;
            }
        }

        public int Inputs
        {
            get
            {
                return (int)Math.Round(InputsNumber.Value);
            }
        }

        public string Function
        {
            get
            {
                var fnc = FunctionTextBox.Text;
                int fncLen = (int)Math.Pow(2, Inputs);
                if (fnc.Length > fncLen)
                {
                    int diff = fnc.Length - fncLen;
                    fnc = fnc.Substring(diff);
                }
                if(fnc.Length < fncLen)
                {
                    int diff = fncLen - fnc.Length;
                    string toAdd = "";
                    while (toAdd.Length != diff)
                        toAdd += '0';
                    fnc = toAdd + fnc;
                }
                return fnc;
            }
        }

        public string GateName
        {
            get { return GateNameTextBox.Text; }
        }

        private void FunctionTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D1 || e.Key == Key.D0 || e.Key == Key.NumPad0 || e.Key == Key.NumPad1)
            {
                if (FunctionTextBox.Text.Length < Math.Pow(2, InputsNumber.Value))
                    return;
                else
                    e.Handled = true;
            }
            if(e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Left || e.Key == Key.Right)
            { return; }
            else
                e.Handled = true;
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            if(GateNameTextBox.Text.Length <= 0)
            {
                MessageBox.Show("Gate name cannot be empty");
                return;
            }

            this.DialogResult = true;
        }


    }
}
