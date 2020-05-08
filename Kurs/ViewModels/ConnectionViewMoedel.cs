using System;
using System.Collections.Generic;
using System.Text;

using Kurs.Models;
using Kurs.Views;

namespace Kurs.ViewModels
{
    public class ConnectionViewMoedel
    {
        public ConnectionViewMoedel(PinViewModel p1, PinViewModel p2)
        {
            connection = new Connection();

            P1 = p1;
            P2 = p2;
            P1.AssignConnection(this);
            P2.AssignConnection(this);
        }

        public Connection connection;
        public PinViewModel P1;
        public PinViewModel P2;
    }
}
