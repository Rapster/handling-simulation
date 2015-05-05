using Airport.Controls;

namespace Airport.Utils.Factories
{
    /// <summary>
    /// CheckIn factory permitting to create CheckIn object.
    /// </summary>
    class CheckInFactory : AirportFactory
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
            return new CheckIn( area, index, image, isBuilt );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets unique instance of the factory
        /// </summary>
        /// <returns>Unique instance of CheckInFactory.</returns>
        public static CheckInFactory GetInstance( )
        {
            if ( instance == null )
            {
                instance = new CheckInFactory( );
            }

            return instance;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Initialize a new instance of CheckInFactory.
        /// </summary>
        private CheckInFactory( )
        {
        }

        // --------------------------------------------------------------------

        private static CheckInFactory instance = null;
    }
}
