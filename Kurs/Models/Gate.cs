using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kurs.Models
{
    public class Gate : IHavePins
    {
        public Gate(string name, int inputsNumber, string function)
        {
            if (inputsNumber < 0)
                throw new ArgumentException("Gate cannot have less than 0 inputs.");
            if (Math.Pow(2, inputsNumber) != function.Length)
                throw new ArgumentException("passed function doesn't correspond to passed inputsNumber " +
                    "(must be 2^inputsNumber == function.Length)");

            Name = name;
            InputsNumber = inputsNumber;
            Function = function;

            OutputPin = new OutputPin(this);
            for (int i = 0; i < inputsNumber; i++)
            {
                InputPins[i] = new InputPin(this);
            }
        }

        public string Name { get; set; }
        public OutputPin OutputPin { get; private set; }
        public InputPin[] InputPins { get; private set; }
        public int InputsNumber
        {
            get { return InputPins.Length; }
            private set
            {
                InputPins = new InputPin[value];
            }
        }
        public int InputVariationsNumber
        {
            get
            {
                return (int)Math.Pow(2, InputsNumber);
            }
        }
        public bool OutputValue
        {
            get
            {
                if (InputPins.Length == 0)
                    return _setOutputValue;

                string InputsString = "";
                foreach(InputPin pin in InputPins)
                {
                    InputsString = (pin.Value ? '1' : '0') + InputsString;
                }
                int DecimalValue = Convert.ToInt32(InputsString, 2);

                int lastIndex = Function.Length - 1;
                return Function[lastIndex - DecimalValue] == '1';
            }
            set { _setOutputValue = value; }
        }

        public bool _setOutputValue;

        /// <summary>
        /// Represents bool function coresponding to the Gate.
        /// Function[m] - value of the output having inputs recieve m in binary.
        /// </summary>
        public string Function
        {
            get { return _function; }
            set
            {
                if (value.Length != InputVariationsNumber)
                    throw new ArgumentException("passed output-defining array doesn't assign an output value " +
                        "to all input variations (value.Length != OutputVariationsNumber)");
                else
                    _function = value;
            }
        }
        public string _function;
    }
}
