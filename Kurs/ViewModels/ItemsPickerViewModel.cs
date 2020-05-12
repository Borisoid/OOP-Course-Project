using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data.SQLite;

using Kurs.DataBase;
using Kurs.Models;
using Kurs.ViewModels;
using Kurs.Views;
using System.Windows;
using System.Data.SqlClient;
using System.Linq;

namespace Kurs.ViewModels
{
    class ItemsPickerViewModel : ViewModelBase
    {
        #region Constructors

        public ItemsPickerViewModel()
        {
            Categories = new BindingList<CheckedCategory>();
            FilteredGates = new BindingList<GateViewModel>();

            //when 'checked' property of 'Categories' item changes call 'FilterGates(object, LestChangedEventArgs)'
            Categories.ListChanged += FilterGates;

            SQLiteCommand com = new SQLiteCommand(SQLiteCon);
            com.CommandText = "SELECT * FROM view_Categories";
            SQLiteDataReader dr = com.ExecuteReader();
            while(dr.Read())
            {
                Categories.Add(new CheckedCategory(dr.GetString(0) ) );
            }


            



            #region CommentedTests

            //Categories.Add(new CheckedCategory("rnd", false));

            //bool[] ar = { true, false, false, false, true, true, true, true };
            //var g = new Gate("3AND", 3, ar);
            //var gv = new GateViewModel(g);

            //FilteredGates.Add(gv);

            #endregion
        }

        #endregion

        SQLiteConnection SQLiteCon
        {
            get
            {
                if (_con != null)
                    return _con;
                else
                {
                    SQLiteConnectionStringBuilder csb = new SQLiteConnectionStringBuilder();
                    csb.DataSource = Properties.AppSettings.Default.DB_path;
                    csb.ForeignKeys = true;
                    _con = new SQLiteConnection(csb.ConnectionString);
                    _con.Open();
                    return _con;
                }
            }
        }
        SQLiteConnection _con;

        #region Nested Calsses

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

        #endregion

        #region Properties

        public BindingList<CheckedCategory> Categories { get; set; }
        public BindingList<GateViewModel> FilteredGates { get; set; }

        #endregion



        #region Methods

        public void FilterGates(object sender, ListChangedEventArgs e)
        {
            FilteredGates.Clear();

            QueryBilder qb = new QueryBilder();
            foreach (CheckedCategory cc in Categories)
                if (cc.Checked)
                    qb.Categories.Add(cc.Category);

            SQLiteCommand com = new SQLiteCommand(SQLiteCon);
            com.CommandText = qb.Query;

            SQLiteDataReader r = com.ExecuteReader();
            while(r.Read())
            {
                string name = r.GetString(0);
                int inpNumber = r.GetInt32(1);
                string function = r.GetString(2);

                #region Converting string of 0's and 1's into bool array

                function.Reverse();
                bool[] func = new bool[function.Length];
                for(int i = 0; i < function.Length; i++)
                {
                    func[i] = function[i] == 1;
                }

                #endregion

                Gate g = new Gate(name, inpNumber, func);
                GateViewModel gvm = new GateViewModel(g);

                FilteredGates.Add(gvm);
            }
        }

        #endregion
    }
}
