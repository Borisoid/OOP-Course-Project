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
    /// Логика взаимодействия для LogicalTableView.xaml
    /// </summary>
    public partial class LogicalTableView : Window
    {
        public LogicalTableView()
        {
            InitializeComponent();
        }

        public int wndWidth
        {
            get { return colWidth * (InputsNumber + OutputsNumber + 1) ; }
        }

        public int colWidth = 29;

        public int InputsNumber;
        public int OutputsNumber;

        private void view_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = wndWidth;
            foreach(DataGridColumn dgc in dataGrid.Columns)
            {
                dgc.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            if (InputsNumber == 0)
                return;
            dataGrid.Columns[InputsNumber - 1].Width = new DataGridLength(2, DataGridLengthUnitType.Star);/*new DataGridLength(2, DataGridLengthUnitType.Star);*/
        }
    }
}
