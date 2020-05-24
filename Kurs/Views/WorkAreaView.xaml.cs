using Kurs.ViewModels;
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

using System.Windows.Threading;
using System.Windows.Interactivity;

namespace Kurs.Views
{
    /// <summary>
    /// Логика взаимодействия для WorkAreaView.xaml
    /// </summary>
    public partial class WorkAreaView : UserControl
    {
        public WorkAreaView()
        {
            InitializeComponent();
        }
        private void canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PointingTo = e.GetPosition(canvas);
        }


        private static DependencyProperty PointingToProperty;
        static WorkAreaView()
        {
            PointingToProperty = DependencyProperty.Register("PointingTo", typeof(Point), typeof(WorkAreaView));

            RoutedDeleteCommand = new RoutedCommand();
            RoutedDeleteCommand.InputGestures.Add(new KeyGesture(Key.Delete));
        }
        public Point PointingTo
        {
            get { return (Point)GetValue(PointingToProperty); }
            set { SetValue(PointingToProperty, value); }
        }

        public static RoutedCommand RoutedDeleteCommand;

        private void view_Loaded(object sender, RoutedEventArgs e)
        {
            var dc = this.DataContext as WorkAreaViewModel;
            if (dc != null)
                dc.GatesLoaded += Refresh;
        }

        private static readonly Action EmptyDelegate = delegate { };
        public void Refresh()
        {
            (this as UIElement).Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }




    /// <summary>
    ///  Allows associated a routed command with a non-ordinary command. 
    /// </summary>
    public class RoutedCommandBinding : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
          "Command",
          typeof(ICommand),
          typeof(RoutedCommandBinding),
          new PropertyMetadata(default(ICommand)));

        /// <summary> The command that should be executed when the RoutedCommand fires. </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary> The command that triggers <see cref="ICommand"/>. </summary>
        public ICommand RoutedCommand { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();

            var binding = new CommandBinding(RoutedCommand, HandleExecuted, HandleCanExecute);
            AssociatedObject.CommandBindings.Add(binding);
        }

        /// <summary> Proxy to the current Command.CanExecute(object). </summary>
        private void HandleCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Command?.CanExecute(e.Parameter) == true;
            e.Handled = true;
        }

        /// <summary> Proxy to the current Command.Execute(object). </summary>
        private void HandleExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Command?.Execute(e.Parameter);
            e.Handled = true;
        }
    }
}
