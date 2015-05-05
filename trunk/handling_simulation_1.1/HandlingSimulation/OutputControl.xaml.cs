using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Nth.Eindhoven.Fontys
{
    /// <summary>
    /// Interaction logic for OutputControl.xaml
    /// </summary>
    public partial class OutputControl : UserControl
    {
        public OutputControl( )
        {
            InitializeComponent( );
        }

        // --------------------------------------------------------------------

        public void AddMessage( string message )
        {
            outputText.AppendText( message + "\r\n" );
        }

        // --------------------------------------------------------------------

        public void Clear( )
        {
            errorGrid.Items.Clear( );
            outputText.Clear( );
        }

        // --------------------------------------------------------------------

        public void AddException( Exception exception )
        {
            errorGrid.Items.Add( exception );
        }
    }
}
