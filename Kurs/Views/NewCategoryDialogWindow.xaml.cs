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
using System.Windows.Shapes;

namespace Kurs.Views
{
    /// <summary>
    /// Логика взаимодействия для NewCategoryDialogWindow.xaml
    /// </summary>
    public partial class NewCategoryDialogWindow : Window
    {
        public NewCategoryDialogWindow()
        {
            InitializeComponent();
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            InputTextBox.SelectAll();
            InputTextBox.Focus();
        }

        public string CategoryName
        {
            get { return InputTextBox.Text; }
        }
    }
}
