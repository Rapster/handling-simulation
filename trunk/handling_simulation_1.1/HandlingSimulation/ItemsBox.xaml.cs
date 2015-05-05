using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Airport.Controls;

namespace Nth.Eindhoven.Fontys
{
    /// <summary>
    /// Interaction logic for ItemsBox.xaml
    /// </summary>
    public partial class ItemsBox : UserControl
    {
        public ItemsBox( )
        {
            InitializeComponent( );
        }

        private void OnMouseEnter( object sender, MouseEventArgs e )
        {
            Cursor = Cursors.Hand;
        }

        private void OnMouseMove( object sender, MouseEventArgs e )
        {
            if ( e.LeftButton == MouseButtonState.Pressed )
            {
                DataObject data = new DataObject( sender.GetType( ), sender );
                DragDrop.DoDragDrop( this, data, DragDropEffects.Move );
            }
        }

        private void PlotConveyorDown( object sender, RoutedEventArgs e )
        {
            if ( PlotLineDown != null )
            {
                PlotLineDown( sender, e );
            }
        }

        public bool PlottingButtonState
        {
            get { return (bool)btnPlottingLine.IsChecked; }
            set { btnPlottingLine.IsChecked = value; }
        }

        public event EventHandler PlotLineDown;
    }
}
