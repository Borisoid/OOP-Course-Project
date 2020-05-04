using System;
using System.Collections.Generic;
using System.Text;

using Kurs.Models;
using Kurs.Views;

namespace Kurs.ViewModels
{
    class PinViewModel : ViewModelBase
    {
        public PinViewModel(Pin pin)
        {
            Pin = pin;

            RectHeight = Properties.AppSettings.Default.PinViewRectHeight;
            RectWidth = Properties.AppSettings.Default.PinViewRectWidth;

            PinView = new PinView();
            PinView.DataContext = this;


        }
        public Pin Pin { get; private set; }
        public PinView PinView { get; private set; }

        private int _rectHeight;
        public int RectHeight
        {
            get { return _rectHeight; }
            set
            {
                _rectHeight = value;
                OnPropertyChanged("RectHeight");
            }
        }

        private int _rectWidth;
        public int RectWidth
        {
            get { return _rectWidth; }
            set
            {
                _rectWidth = value;
                OnPropertyChanged("RectWidth");
            }
        }
        public int CircleRadius
        {
            get { return RectHeight / 2; }
        }
        public int LineLength
        {
            get { return RectWidth - CircleRadius; }
        }
        public int LineY
        {
            get { return RectHeight / 2; }
        }
        public int LineX1
        {
            get { return (Type == PinType.Input) ? CircleRadius : 0; }
        }
        public int LineX2
        {
            get { return LineX1 + LineLength; }
        }
        public int CircleTopPositionInCanvas
        {
            get { return 0; }
        }
        public int CircleLeftPositionInCanvas
        {
            get
            {
                if (Type == PinType.Output)
                    return RectWidth - (CircleRadius * 2);
                else
                    return 0;
            }
        }

        public enum PinType
        {
            Input,
            Output
        }
        public PinType Type
        {
            get
            {
                if (Pin is InputPin)
                    return PinType.Input;
                if (Pin is OutputPin)
                    return PinType.Output;
                else
                    throw new ArgumentException();
            }
        }
    }
}
