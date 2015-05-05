using System.Collections.Generic;
using Airport.Controls;
using Airport.Utils.Properties;
using Airport.Tools;

namespace Airport.Utils.Factories
{
    /// <summary>
    /// DestinationGate factory permitting to create a DestinationGate property.
    /// </summary>
    class DestinationGatePropertyFactory : AirportZonePropertyFactory
    {
        public override AirportZoneProperty CreateProperties( AirportZone zone,
                                                              List<Package> packages,
                                                              List<Way> ways )
        {
            return new DestinationGateProperty( zone as DestinationGate, packages, ways );
        }

        // --------------------------------------------------------------------

        public static DestinationGatePropertyFactory GetInstance( )
        {
            if ( instance == null )
            {
                instance = new DestinationGatePropertyFactory( );
            }

            return instance;
        }

        // --------------------------------------------------------------------

        private DestinationGatePropertyFactory( )
        {
        }

        // --------------------------------------------------------------------

        private static DestinationGatePropertyFactory instance = null;
    }
}
