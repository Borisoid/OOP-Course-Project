using System;
using System.Collections.Generic;
using System.Text;

using Kurs.Models;
using Kurs.Views;

namespace Kurs.ViewModels
{
    public class ConnectionViewModel : ViewModelBase
    {
        public ConnectionViewModel(Connection con)
        {
            connection = con;

            PinViewModel1 = con.InputPin.pinViewModel;
            PinViewModel2 = con.OutputPin.pinViewModel;
            PinViewModel1.AssignConnection(this);
            PinViewModel2.AssignConnection(this);
        }

        public Connection connection;
        public readonly PinViewModel PinViewModel1;
        public readonly PinViewModel PinViewModel2;

        public PinView PinView1 { get { return PinViewModel1.PinView; } }
        public PinView PinView2 { get { return PinViewModel2.PinView; } }

        public void Disconnect()
        {
            PinViewModel1.BreakConnection(this);
            PinViewModel2.BreakConnection(this);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Connection))
                return false;
            var c = obj as Connection;
            return connection.Equals(c);
        }

        //public bool IsSelected
        //{
        //    get { return _isSelected; }
        //    set
        //    {
        //        _isSelected = value;
        //        OnPropertyChanged("IsSelected");
        //    }
        //}
        //public bool _isSelected;
    }
}
