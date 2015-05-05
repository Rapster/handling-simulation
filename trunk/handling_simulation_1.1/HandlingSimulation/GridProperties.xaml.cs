using System.Windows.Controls;
using Airport.Utils.Properties;

namespace Nth.Eindhoven.Fontys
{
    /// <summary>
    /// Interaction logic for GridProperties.xaml
    /// </summary>
    public partial class GridProperties : UserControl
    {
        public GridProperties( )
        {
            InitializeComponent( );
        }

        // --------------------------------------------------------------------

        public AirportZoneProperty SelectedZone
        {
            set
            {
                propertyGrid.SelectedObject = value;
            }
        }
    }
}
