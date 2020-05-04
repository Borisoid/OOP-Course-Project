using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs.Models
{
    /// <summary>
    /// Source of signal, outputs Value. Has no input pins.
    /// </summary>
    class SignalSource : IHavePins
    {
        public SignalSource(bool value = false)
        {
            OutputPin = new OutputPin(this);
            Value = value;
        }

        public InputPin[] InputPins { get { throw new Exception("Signal source doesn't have input pins"); } }   //SignalSource doesn't have inputs
        public OutputPin OutputPin { get; private set; }

        /// <summary>
        /// Stores value SignalSource outputs.
        /// </summary>
        public bool _value;

        /// <summary>
        /// Gets/sets value SignalSource outputs.
        /// </summary>
        public bool Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public bool OutputValue
        {
            get { return _value; }
        }
    }
}
