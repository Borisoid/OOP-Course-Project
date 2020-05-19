using Kurs.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Kurs.ViewModels
{
    class LogicalTableViewModel
    {
        IOValues IO;
        DataTable table;

        public LogicalTableViewModel(List<IOValues> IOList)
        {
            table = new DataTable();

            int inputNum = IOList[0].IValues.Length;
            int outputNum = IOList[0].OValues.Length;

            for (int i = inputNum - 1; i >= 0; i--)
            {
                table.Columns.Add("S" + i, typeof(string));
            }
            for (int i = outputNum - 1; i >= 0; i--)
            {
                table.Columns.Add("R" + i, typeof(string));
            }

            foreach(IOValues io in IOList)
            {
                DataRow dr = table.NewRow();
                var chars = (io.IValues.Reverse().Concat(io.OValues.Reverse()));
                int count = chars.Count();
                var objArr = new object[count];
                for(int i = 0; i < count; i++)
                {
                    objArr[i] = chars.ElementAt(i).ToString();
                }

                dr.ItemArray = objArr;
                table.Rows.Add(dr);
            }
        }

        public void ShowView()
        {
            var view = new LogicalTableView();
            view.dataGrid.DataContext = table.DefaultView;

            view.Show();
        }
    }
}
