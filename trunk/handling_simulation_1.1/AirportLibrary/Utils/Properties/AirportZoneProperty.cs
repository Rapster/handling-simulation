using System.Collections.Generic;
using System.ComponentModel;
using Airport.Controls;
using Airport.Tools;

namespace Airport.Utils.Properties
{
    /// <summary>
    /// AirportZone's property class used by the PropertyGrid
    /// </summary>
    public class AirportZoneProperty
    {
        /// <summary>
        /// Initialize a new AirportZoneProperty
        /// </summary>
        /// <param name="zone">Destination Gate object needed to display the proper characteristics</param>
        /// <param name="packages">Packages going through the specified sorter</param>
        /// <param name="ways">Way going through the specified sorter</param>
        public AirportZoneProperty( AirportZone zone, List<Package> packages, List< Way > ways )
        {
            GlobalPackages.idPackages.Clear( );
            this.packages = packages.FindAll( delegate( Package pkg )
            {
                return pkg.Way.Zones.Contains( zone );
            }
            );

            foreach ( Package pkg in this.packages )
            {
                GlobalPackages.idPackages.Add( pkg.Luggages.Capacity.ToString( ) );
            }

            GlobalInputs.idInputs.Clear( );
            this.ways = ways.FindAll( delegate( Way way )
            {
                return way.Zones.Contains( zone );
            }
            );

            foreach ( Way way in this.ways )
            {
                GlobalInputs.idInputs.Add( way.Start.Name );
            }

            GlobalZones.idZones.Clear( );
            foreach ( AirportZone tmp in zone.Destinations )
            {
                GlobalZones.idZones.Add( tmp.Name );
            }

            State = this.ways.Count > 0
                    ? ( this.ways[ 0 ].End as DestinationGate ).PlaneIsPresent
                    ? "Ready"
                    : "Waiting"
                    : "Undefined";

            Name = zone.Name;
        }

        // --------------------------------------------------------------------

        [Browsable( true )]
        [TypeConverter( typeof( ZoneConverter ) )]
        [DescriptionAttribute( "Destinations reached by this zone" ),
        CategoryAttribute( "Common Properties" )]
        public string Destinations
        {
            get
            {
                return rankZone = GlobalZones.idZones.Count > 0
                                    ? GlobalZones.idZones[ 0 ]
                                    : "Undefined";
            }
            set
            {
                rankZone = value;
            }
        }

        // --------------------------------------------------------------------

        [Browsable( true )]
        [TypeConverter( typeof( PackageConverter ) )]
        [DescriptionAttribute( "Packages going through this zone" ),
        CategoryAttribute( "Common Properties" )]
        public string Packages
        {
            get
            {
                return amount = GlobalPackages.idPackages.Count > 0
                                ? GlobalPackages.idPackages[ 0 ].ToString( )
                                : "Undefined";
            }
            set
            {
                amount = value;
            }
        }

        // --------------------------------------------------------------------

        [DescriptionAttribute( "Name of the component" ),
        CategoryAttribute( "Common Properties" )]
        [ReadOnlyAttribute( true )]
        public string Name
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        [DescriptionAttribute( "Indicate the plane state corresponding to the selected way" ),
        CategoryAttribute( "Common Properties" )]
        [ReadOnlyAttribute( true )]
        public string State
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        protected class GlobalZones
        {
            internal static List<string> idZones = new List<string>( );
        }

        // --------------------------------------------------------------------

        protected class GlobalPackages
        {
            internal static List<string> idPackages = new List<string>( );
        }

        // --------------------------------------------------------------------

        protected class GlobalInputs
        {
            internal static List<string> idInputs = new List<string>( );
        }

        // --------------------------------------------------------------------

        protected class ZoneConverter : TypeConverter
        {
            public override bool GetStandardValuesSupported( ITypeDescriptorContext ctx )
            {
                return true;
            }

            public override bool GetStandardValuesExclusive( ITypeDescriptorContext ctx )
            {
                return true;
            }

            public override TypeConverter.StandardValuesCollection
                   GetStandardValues( ITypeDescriptorContext context )
            {
                return new System.ComponentModel.TypeConverter.StandardValuesCollection( GlobalZones.idZones );
            }
        }

        // --------------------------------------------------------------------

        protected class PackageConverter : TypeConverter
        {
            public override bool GetStandardValuesSupported( ITypeDescriptorContext ctx )
            {
                return true;
            }

            public override bool GetStandardValuesExclusive( ITypeDescriptorContext ctx )
            {
                return true;
            }

            public override TypeConverter.StandardValuesCollection
                   GetStandardValues( ITypeDescriptorContext ctx )
            {
                return new System.ComponentModel.TypeConverter.StandardValuesCollection( GlobalPackages.idPackages );
            }
        }

        // --------------------------------------------------------------------

        protected class WayConverter : TypeConverter
        {
            public override bool GetStandardValuesSupported( ITypeDescriptorContext ctx )
            {
                return true;
            }

            public override bool GetStandardValuesExclusive( ITypeDescriptorContext ctx )
            {
                return true;
            }

            public override TypeConverter.StandardValuesCollection
                   GetStandardValues( ITypeDescriptorContext ctx )
            {
                return new System.ComponentModel.TypeConverter.StandardValuesCollection( GlobalInputs.idInputs );
            }
        }

        // --------------------------------------------------------------------

        protected List<Way> ways;
        protected List<Package> packages;
        private string rankZone;
        private string amount;
    }
}
