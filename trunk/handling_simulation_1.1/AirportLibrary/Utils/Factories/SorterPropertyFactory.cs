using System.Collections.Generic;
using Airport.Controls;
using Airport.Utils.Properties;
using Airport.Tools;

namespace Airport.Utils.Factories
{
    /// <summary>
    /// Sorter factory permitting to create a Sorter property.
    /// </summary>
    class SorterPropertyFactory : AirportZonePropertyFactory
    {
        public override AirportZoneProperty CreateProperties( AirportZone zone, 
                                                              List<Package> packages,
                                                              List<Controls.Way> ways )
        {
            return new SorterProperty( zone as Sorter, packages, ways );
        }

        // --------------------------------------------------------------------

        public static SorterPropertyFactory GetInstance( )
        {
            if ( instance == null )
            {
                instance = new SorterPropertyFactory( );
            }

            return instance;
        }

        // --------------------------------------------------------------------

        private SorterPropertyFactory( )
        {
        }

        // --------------------------------------------------------------------

        private static SorterPropertyFactory instance = null;
    }
}
