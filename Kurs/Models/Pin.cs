using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs.Models
{
    public abstract class Pin
    {
        public IHavePins Owner { get; protected set; }
        public abstract bool Value { get; }
        public Connection Connection { get; protected set; }
        public abstract void AssignConnection(Connection con);
    }
}
