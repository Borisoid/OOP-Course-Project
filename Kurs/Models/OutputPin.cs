using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs.Models
{
    public class OutputPin : Pin
    {
        public OutputPin(IHavePins owner)
        {
            Owner = owner;
        }

        public override void AssignConnection(Connection con)
        {
            this.Connection = con;
            con.OutputPin = this;
        }
        public override bool Value
        {
            get { return Owner.OutputValue; }
        }
    }
}
