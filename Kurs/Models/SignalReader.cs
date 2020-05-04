using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs.Models
{
    /// <summary>
    /// Reads signal. Has no output pins.
    /// </summary>
    class SignalReader : IHavePins
    {
        public SignalReader()
        {
            InputPin = new InputPin(this);
        }

        /// <summary>
        /// Do not use.
        /// </summary>
        public InputPin[] InputPins { get; private set; }
        public InputPin InputPin
        {
            get { return InputPins[0]; }
            private set { InputPins[0] = value; }
        }
        public OutputPin OutputPin  //has no output pins.
        {
            get
            {
                throw new Exception("Signal reader doesn't have output pins");
            }
        }
        public bool OutputValue
        {
            get { return InputPin.Value; }
        }
    }
}
