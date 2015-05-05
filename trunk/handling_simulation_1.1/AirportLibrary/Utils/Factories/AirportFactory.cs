using System;
using Airport.Controls;

namespace Airport.Utils.Factories
{
    /// <summary>
    /// Singleton factory permitting to create an AirportZone according to a type.
    /// </summary>
    public abstract class AirportFactory
    {
        /// <summary>
        /// Gets the corresponding AirportFactory according to the specified parameter.
        /// </summary>
        /// <param name="type">Type of AirportZone to create.</param>
        /// <returns>AirportFactory corresponding to the type specified in parameter.</returns>
        public static AirportFactory GetFactory( Type type )
        {
            if( type == typeof( CheckIn ) )
            {
                return CheckInFactory.GetInstance( );
            }

            if( type == typeof( DestinationGate ) )
            {
                return DestinationGateFactory.GetInstance( );
            }

            if ( type == typeof( Sorter ) )
            {
                return SorterFactory.GetInstance( );
            }

            if( type == typeof( AirportZone ) )
            {
                return AirportZoneFactory.GetInstance( );
            }

            return null;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Create an AirportZone with the specified parameters 
        /// </summary>
        /// <example>
        /// <code lang="c#">
        /// AirportFactory factory = AirportFactory.GetFactory( type );
        /// AirportZone zone = factory.CreateZone( this, tmp.Position, tmp.Background, true );
        ///</code>
        /// </example>
        /// <param name="area">Parent reference to the AirportArea object that contains the zone.</param>
        /// <param name="index">Position's object inside the specified AirportArea.</param>
        /// <param name="image">Background image</param>
        /// <param name="isBuilt">Zone concretly present in its parent</param>
        /// <returns>A new AirportZone object.</returns>
        public abstract AirportZone CreateZone( AirportArea area,
                                                Index index,
                                                string image,
                                                bool isBuilt );
    }
}
