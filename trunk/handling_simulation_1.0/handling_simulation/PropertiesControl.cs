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
    public partial class PropertiesControl : UserControl
    {
        public PropertiesControl( )
        {
            InitializeComponent( );
            zones = new List< Airport.Forms.AirportZone >( );
        }

        // --------------------------------------------------------------------

        public Control SelectedObject
        {
            set
            {
                cmboComponents.SelectedItem = value;
                propertyGrid.SelectedObject = value;
                cmboComponents.Refresh( );
            }
        }

        // --------------------------------------------------------------------

        public void AddControl( Airport.Forms.AirportZone control )
        {
            zones.Add( control );
            cmboComponents.Items.Add( control.ToString( ) );
            SelectedObject = control;
        }

        // --------------------------------------------------------------------

        private void cmboComponents_SelectedIndexChanged( object sender, EventArgs e )
        {
            AirportZone result = zones.Find( delegate( AirportZone zone )
            {
                return zone.Position.ToString( ) == ( sender as String );
            }
            );

            propertyGrid.SelectedObject = result;
        }

        // --------------------------------------------------------------------

        private List< Airport.Forms.AirportZone > zones;

    }
}
