using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs.Models
{
    [Serializable]
    public class Connection
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Connection))
                return false;
            else
            {
                var c = obj as Connection;
                return Object.ReferenceEquals(this.InputPin, c.InputPin) && 
                    Object.ReferenceEquals(this.OutputPin, c.OutputPin);
            }
        }
        public InputPin InputPin { get; set; }
        public OutputPin OutputPin { get; set; }

    }
}
