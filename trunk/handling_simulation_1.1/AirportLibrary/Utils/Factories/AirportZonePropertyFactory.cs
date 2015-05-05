using System.Collections.Generic;
using Airport.Controls;
using Airport.Utils.Properties;
using Airport.Tools;

namespace Airport.Utils.Factories
{
    /// <summary>
    /// AirportZone property factory permitting to create base property object AirportZone.
    /// Displaying proper attributes of a component and packages going through the component.
    /// </summary>
    public abstract class AirportZonePropertyFactory
    {
        /// <summary>
        /// Gets the corresponding AirportFactory according to the specified parameter.
        /// </summary>
        /// <param name="zone">AirportZone permitting to create the corresponding property.</param>
        /// <returns>AirportZonePropertyFactory corresponding to the AirportZone specified in parameter.</returns>
        public static AirportZonePropertyFactory GetFactory( AirportZone zone )
        {
            if ( zone is CheckIn )
            {
                return CheckInPropertyFactory.GetInstance( );
            }

            if ( zone is DestinationGate )
            {
                return DestinationGatePropertyFactory.GetInstance( );
            }

            if ( zone is Sorter )
            {
                return SorterPropertyFactory.GetInstance( );
            }

            return null;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Create an AirportZoneProperty depending the specified AirportZone.
        /// </summary>
        /// <example>
        /// <code lang="c#">
        /// AirportZonePropertyFactory factory = AirportZonePropertyFactory.GetFactory( airportZone );
        /// propertyGrid.SelectedObject = factory.CreateProperties( airportZone,
        ///                                                         packages,
        ///                                                         ways );
        /// </code>
        /// </example>
        /// <param name="zone">AirportZone containing informations to create the AirportZoneProperty.</param>
        /// <param name="packages">Set of Package can pass through the specified AirportZone.</param>
        /// <param name="ways">Set of Ways can pass through the specified AirportZone.</param>
        /// <returns>The AirportZoneProperty containing all the AirportZone information.</returns>
        public abstract AirportZoneProperty CreateProperties( AirportZone zone,
                                                              List<Package> packages,
                                                              List<Way> ways );

        // --------------------------------------------------------------------
    }
}
