using Airport.Controls;

namespace Airport.Utils.Factories
{
    /// <summary>
    /// AirportZone factory permitting to create base object AirportZone.
    /// </summary>
    class AirportZoneFactory : AirportFactory
    {
        /// <summary>
        /// Create an AirportZone with the specified parameters 
        /// </summary>
        /// <param name="area">Parent reference to the AirportArea object that contains the zone.</param>
        /// <param name="index">Position's object inside the specified AirportArea.</param>
        /// <param name="image">Background image</param>
        /// <param name="isBuilt">Zone concretly present in its parent</param>
        /// <returns>A new AirportZone object.</returns>
        public override AirportZone CreateZone( AirportArea area,
                                                Index index,
                                                string image,
                                                bool isBuilt )
        {
            return new AirportZone( area, index, image, isBuilt );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets unique instance of the factory
        /// </summary>
        /// <returns>Unique instance of AirportZoneFactory.</returns>
        public static AirportZoneFactory GetInstance( )
        {
            if ( instance == null )
            {
                instance = new AirportZoneFactory( );
            }

            return instance;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Initialize a new instance of AirportZoneFactory.
        /// </summary>
        private AirportZoneFactory( )
        {
        }

        // --------------------------------------------------------------------

        private static AirportZoneFactory instance = null;
    }
}
