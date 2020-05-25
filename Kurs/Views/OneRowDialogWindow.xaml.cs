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
    public partial class OneRowDialogWindow : Window
    {
        public OneRowDialogWindow(string windowName, string question, int maxLen)
        {
            InitializeComponent();

            Title = windowName;
            Question.Content = question;
            InputTextBox.MaxLength = maxLen;
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

        public string Answer
        {
            get { return InputTextBox.Text; }
        }
    }
}
