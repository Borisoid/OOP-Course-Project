using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs.Models
{
    public class InputPin : Pin
    {
        public InputPin(IHavePins owner) : base()
        {
            Owner = owner;
        }

        public override void AssignConnection(Connection con)
        {
            this.Connections.Add(con);
            con.InputPin = this;
        }
        public override bool Value
        {
            get
            {
                foreach (Connection con in Connections)
                {
                    if (con.OutputPin.Value)
                        return true;
                }
                return false;
            }
        }
    }
}
