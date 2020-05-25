using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data.SQLite;
using System.Windows;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Input;
using System.Text.RegularExpressions;

using Kurs.DataBase;
using Kurs.Models;
using Kurs.ViewModels;
using Kurs.Views;
using Kurs.Commands;

namespace Kurs.ViewModels
{
    public class ItemsPickerViewModel : ViewModelBase
    {
        #region Constructors

        public ItemsPickerViewModel()
        {
            Categories = new BindingList<CheckedCategory>();
            FilteredGates = new BindingList<GateViewModel>();

            //when 'checked' property of 'Categories' item changes call 'FilterGates(object, ListChangedEventArgs)'
            Categories.ListChanged += FilterGates;

            QueryCategories();
        }

        #endregion

        #region Connection

        SQLiteConnection SQLiteCon
        {
            get
            {
                if (_con != null)
                {
                    if (_con.State == System.Data.ConnectionState.Closed)
                        _con.Open();
                    DBRecoveryMaster.CheckConnection(_con);
                    return _con;
                }
                else
                {
                    SQLiteConnectionStringBuilder csb = new SQLiteConnectionStringBuilder();
                    csb.DataSource = Properties.AppSettings.Default.DB_path;
                    csb.ForeignKeys = true;
                    _con = new SQLiteConnection(csb.ConnectionString);
                    _con.Open();
                    DBRecoveryMaster.CheckConnection(_con);
                    return _con;
                }
            }
        }
        SQLiteConnection _con;

        #endregion

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
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                OnPropertyChanged("SearchString");
            }
        }

        #endregion

        #region Data

        public string _searchString;

        #endregion

        #region Methods

        public void FilterGates(object sender, ListChangedEventArgs e)
        {
            FilteredGates.Clear();


            CategoryQueryBilder qb = new CategoryQueryBilder();
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

                Gate g = new Gate(name, inpNumber, function);
                GateViewModel gvm = new GateViewModel(g);

                FilteredGates.Add(gvm);
            }
        }

        public void QueryCategories()
        {
            Categories.Clear();

            SQLiteCommand com = new SQLiteCommand(SQLiteCon);
            com.CommandText = "SELECT * FROM view_Categories";
            SQLiteDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                Categories.Add(new CheckedCategory(dr.GetString(0)));
            }
        }

        #endregion

        #region Commands


        #region Search

        private DelegateCommand searchCommand;

        public ICommand SearchCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand = new DelegateCommand(Search);
                }
                return searchCommand;
            }
        }

        private void Search()
        {
            foreach(CheckedCategory cc in Categories)
            {
                cc.Checked = false;
            }

            if (String.IsNullOrEmpty(SearchString))
                return;

            FilteredGates.Clear();

            Regex regex = new Regex(SearchString, RegexOptions.IgnoreCase);

            SQLiteCommand com = new SQLiteCommand(SQLiteCon);
            com.CommandText = "SELECT * FROM view_Gates";
            SQLiteDataReader r = com.ExecuteReader();
            while(r.Read())
            {
                string Name = r.GetString(0);
                if(regex.IsMatch(Name))
                {
                    int InputsNumber = r.GetInt32(1);
                    string function = r.GetString(2);

                    FilteredGates.Add(new GateViewModel(new Gate(Name, InputsNumber, function)));
                }
            }
        }

        #endregion

        #region SelectCommand

        private DelegateCommand<GateViewModel> selectCommand;

        public ICommand SelectCommand
        {
            get
            {
                if (selectCommand == null)
                {
                    selectCommand = new DelegateCommand<GateViewModel>(Select);
                }
                return selectCommand;
            }
        }

        private void Select(GateViewModel gvm)
        {
            SelectedGateViewModel = gvm;
        }

        public GateViewModel SelectedGateViewModel
        {
            get { return _selectedGateViewModel; }
            set
            {
                if(_selectedGateViewModel != null)
                    _selectedGateViewModel.Selected = false;    //old
                _selectedGateViewModel = value;                 //new
                if (_selectedGateViewModel != null)
                    _selectedGateViewModel.Selected = true;

                //MessageBox.Show(_selectedGateViewModel.Name);
            }
        }
        public GateViewModel _selectedGateViewModel;

        #endregion

        #region NewCategory

        private DelegateCommand AddCategoryCommand;
        public ICommand addCategoryCommand
        {
            get
            {
                if (AddCategoryCommand == null)
                {
                    AddCategoryCommand = new DelegateCommand(AddCategory);
                }
                return AddCategoryCommand;
            }
        }
        public void AddCategory()
        {
            NewCategoryDialogWindow dialog = new NewCategoryDialogWindow();
            if (dialog.ShowDialog() == true)
            {
                var con = SQLiteCon;
                var tran = con.BeginTransaction();
                var cmd = con.CreateCommand();
                cmd.Transaction = tran;
                cmd.CommandText = $"INSERT INTO tbl_Categories(Name) VALUES (@Name)";
                cmd.Parameters.Add("@Name", System.Data.DbType.String).Value = dialog.CategoryName;

                try
                {
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                QueryCategories();
            }
        }

        #endregion

        #region NewGate

        private DelegateCommand AddGateCommand;
        public ICommand addGateCommand
        {
            get
            {
                if (AddGateCommand == null)
                {
                    AddGateCommand = new DelegateCommand(AddGate);
                }
                return AddGateCommand;
            }
        }
        public void AddGate()
        {
            List<string> categories = new List<string>();
            foreach (CheckedCategory cc in Categories)
                if (cc.Category != "NATIVE" && cc.Category != "PERIPHERAL")
                    categories.Add(cc.Category);
            NewGateDialogWindow dialog = new NewGateDialogWindow(categories);

            if(dialog.ShowDialog() == true)
            {
                var qb = new AddGateQueryBuilder();
                qb.GateName = dialog.GateName;
                qb.InputsNumber = dialog.Inputs;
                qb.Function = dialog.Function;
                qb.Categories = new List<string>(dialog.SelectedCategories);

                var con = SQLiteCon;
                var tran = con.BeginTransaction();
                var cmd = con.CreateCommand();
                cmd.Transaction = tran;
                cmd.CommandText = qb.Query;
                try
                {
                    cmd.ExecuteNonQuery();
                    tran.Commit();

                    FilterGates(null, null);
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        #endregion

        #endregion
    }
}
