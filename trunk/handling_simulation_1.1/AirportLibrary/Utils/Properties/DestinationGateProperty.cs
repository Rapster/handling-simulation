using System.Collections.Generic;
using Airport.Controls;
using System.ComponentModel;
using Airport.Tools;

namespace Airport.Utils.Properties
{
    /// <summary>
    /// DestinationGate's property class used by the PropertyGrid 
    /// </summary>
    public class DestinationGateProperty : AirportZoneProperty
    {
        /// <summary>
        /// Initialize a new SorterProperty
        /// </summary>
        /// <param name="zone">Destination Gate object needed to display the proper characteristics</param>
        /// <param name="packages">Packages going through the specified sorter</param>
        /// <param name="ways">Way going through the specified sorter</param>
        public DestinationGateProperty( DestinationGate zone, List<Package> packages, List<Way> ways )
            : base( zone, packages, ways )
        {
            GlobalInputs.idInputs.Clear( );
            ways = ways.FindAll( delegate( Way way )
            {
                return way.Zones.Contains( zone );
            }
            );

            foreach ( Way way in ways )
            {
                GlobalInputs.idInputs.Add( way.Start.Name );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Indicates the different sources of the specified DestinationGate
        /// </summary>
        [Browsable( true )]
        [TypeConverter( typeof( WayConverter ) )]
        [DescriptionAttribute( "Gate's sources" ),
        CategoryAttribute( "Destination Gate Properties" )]
        public string Sources
        {
            get
            {
                return source = GlobalInputs.idInputs.Count > 0
                               ? GlobalInputs.idInputs[ 0 ].ToString( )
                               : "Undefined";
            }
            set
            {
                source = value;
            }
        }

        // --------------------------------------------------------------------

        private string source;
    }
}
