using System.Collections.Generic;
using Airport.Controls;
using Airport.Utils.Properties;
using Airport.Tools;

namespace Airport.Utils.Factories
{
    /// <summary>
    /// CheckInProperty factory permitting to create a CheckIn property.
    /// </summary>
    class CheckInPropertyFactory : AirportZonePropertyFactory
    {
        public override AirportZoneProperty CreateProperties( AirportZone zone, 
                                                              List<Package> packages,
                                                              List<Controls.Way> ways )
        {
            return new AirportZoneProperty( zone, packages, ways );
        }

        
        // --------------------------------------------------------------------

        public static CheckInPropertyFactory GetInstance( )
        {
            if ( instance == null )
            {
                instance = new CheckInPropertyFactory( );
            }

            return instance;
        }

        // --------------------------------------------------------------------

        private CheckInPropertyFactory( )
        {
        }

        // --------------------------------------------------------------------

        private static CheckInPropertyFactory instance = null;
    }
}
