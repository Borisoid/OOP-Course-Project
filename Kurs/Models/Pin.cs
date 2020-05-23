using Kurs.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs.Models
{
    [Serializable]
    public abstract class Pin
    {
        public Pin()
        {
            Connections = new List<Connection>();
        }
        public IHavePins Owner { get; protected set; }
        public abstract bool Value { get; }
        public List<Connection> Connections { get; protected set; }
        public abstract void AssignConnection(Connection con);

        [NonSerialized]
        public PinViewModel pinViewModel;
    }
}
