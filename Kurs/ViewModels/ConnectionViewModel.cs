using System;
using System.Collections.Generic;
using System.Text;

using Kurs.Models;
using Kurs.Views;

namespace Kurs.ViewModels
{
    public class ConnectionViewModel : ViewModelBase
    {
        public ConnectionViewModel(PinViewModel p1, PinViewModel p2)
        {
            connection = new Connection();

            PinViewModel1 = p1;
            PinViewModel2 = p2;
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
