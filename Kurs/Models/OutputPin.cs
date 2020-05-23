using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs.Models
{
    [Serializable]
    public class OutputPin : Pin
    {
        public OutputPin(IHavePins owner) : base()
        {
            Owner = owner;
        }

        public override void AssignConnection(Connection con)
        {
            this.Connections.Add(con);
            con.OutputPin = this;
        }
        public override bool Value
        {
            get { return Owner.OutputValue; }
        }
    }
}
