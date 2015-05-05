using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Airport.Forms;

namespace Nth.Eindhoven.Fontys
{
    public partial class ComponentsBar : UserControl
    {
        public ComponentsBar( )
        {
            InitializeComponent( );
        }

        private void componentMouseMove( object sender, MouseEventArgs e )
        {
            ( sender as AirportZone ).DoDragDrop( ( sender as AirportZone ).BackgroundImage,
                                                    DragDropEffects.Move );
        }

        private void componentMouseEnter( object sender, EventArgs e )
        {
            Cursor.Current = Cursors.Hand;
        }


        private void checkBoxConveyor_CheckedChanged( object sender, EventArgs e )
        {
            if ( ClickedOnConvoyer != null )
            {
                ClickedOnConvoyer( sender, e );
            }
        }

        public event EventHandler ClickedOnConvoyer;
    }
}
