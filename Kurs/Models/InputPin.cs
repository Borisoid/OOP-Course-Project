using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs.Models
{
    public class InputPin : Pin
    {
        public InputPin(IHavePins owner)
        {
            Owner = owner;
        }

        public override void AssignConnection(Connection con)
        {
            this.Connection = con;
            con.InputPin = this;
        }
        public override bool Value
        {
            get
            {
                return Connection.OutputPin.Value;
            }
        }
    }
}
