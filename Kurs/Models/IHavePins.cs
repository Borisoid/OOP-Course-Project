using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs.Models
{
    public interface IHavePins
    {
        public InputPin[] InputPins { get;  }
        public OutputPin OutputPin { get; }

        /// <summary>
        /// Value that an element actually outputs
        /// </summary>
        public bool OutputValue { get; }

    }
}
