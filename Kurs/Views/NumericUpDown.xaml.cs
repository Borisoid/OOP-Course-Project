using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kurs.Views
{
    /// <summary>
    /// Логика взаимодействия для NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public NumericUpDown()
        {
            InitializeComponent();
        }
		public static readonly DependencyProperty ValueProperty;
		public static readonly DependencyProperty MaxValueProperty;
		public static readonly DependencyProperty MinValueProperty;
		public static readonly DependencyProperty IncrementProperty;
		public static readonly DependencyProperty IntOnlyProperty;

		public double Value
		{
			get { return (double)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}
		public double MaxValue
		{
			get { return (double)GetValue(MaxValueProperty); }
			set { SetValue(MaxValueProperty, value); }
		}
		public double MinValue
		{
			get { return (double)GetValue(MinValueProperty); }
			set { SetValue(MinValueProperty, value); }
		}
		public double Increment
		{
			get { return (double)GetValue(IncrementProperty); }
			set { SetValue(IncrementProperty, value); }
		}
		public bool IntOnly
		{
			get { return (bool)GetValue(IntOnlyProperty); }
			set { SetValue(IntOnlyProperty, value); }
		}

		static NumericUpDown()
		{
			FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata();
			metadata.CoerceValueCallback = new CoerceValueCallback(CoerceTxtNum);
			ValueProperty = DependencyProperty.Register(
				"Value",
				typeof(double),
				typeof(NumericUpDown),
				metadata,
				new ValidateValueCallback(ValidateNumTxt)
				);
			MaxValueProperty = DependencyProperty.Register(
				"MaxValue",
				typeof(double),
				typeof(NumericUpDown)
				);
			MinValueProperty = DependencyProperty.Register(
				"MinValue",
				typeof(double),
				typeof(NumericUpDown)
				);
			FrameworkPropertyMetadata IncrementMetadata = new FrameworkPropertyMetadata();
			IncrementMetadata.CoerceValueCallback = new CoerceValueCallback(CoerceIncrement);
			IncrementProperty = DependencyProperty.Register(
				"Increment",
				typeof(double),
				typeof(NumericUpDown),
				IncrementMetadata,
				new ValidateValueCallback(ValidateIncrement)
				);
			IntOnlyProperty = DependencyProperty.Register("IntOnly", typeof(bool), typeof(NumericUpDown));
		}
		private static object CoerceTxtNum(DependencyObject d, object value)
		{
			var uc = d as NumericUpDown;
			double v;
			if (Double.TryParse(value.ToString(), out v))
			{
				if (v > uc.MaxValue)
					v = uc.MaxValue;
				if (v < uc.MinValue)
					v = uc.MinValue;
				if (uc.IntOnly)
					v = Math.Round(v);
				return v;
			}
			else
				return DependencyProperty.UnsetValue;
		}
		private static bool ValidateNumTxt(object o)
		{
			//if (o == null)
			//	return true;
			bool dd = Double.TryParse(o.ToString(), out _);
			if (dd)
				return true;
			else
				return false;
		}

		//returns positive increment value
		private static object CoerceIncrement(DependencyObject d, object value)
		{
			double doub;
			bool dd = Double.TryParse(value.ToString(), out doub);
			if (dd)
				return Math.Abs(doub);
			else
				return DependencyProperty.UnsetValue;
		}
		private static bool ValidateIncrement(object o)
		{
			if (o is double && (double)o >= 0)
				return true;
			else
				return false;
		}

		private void BtnIncrement_Click(object sender, RoutedEventArgs e)
		{
			Value += Increment;
		}
		private void BtnDecrement_Click(object sender, RoutedEventArgs e)
		{
			Value -= Increment;
		}

		private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (e.Command == ApplicationCommands.Cut ||
				e.Command == ApplicationCommands.Paste)
			{
				e.Handled = true;
			}
		}
	}
}
