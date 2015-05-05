using Airport.Controls;

namespace Airport.Utils.Factories
{
    /// <summary>
    /// Sorter factory permitting to create Sorter object.
    /// </summary>
    class SorterFactory : AirportFactory
    {
        /// <summary>
        /// Create an AirportZone with the specified parameters 
        /// </summary>
        /// <param name="area">Parent reference to the AirportArea object that contains the zone.</param>
        /// <param name="position">Position's object inside the specified AirportArea.</param>
        /// <param name="image">Background image</param>
        /// <param name="isBuilt">Zone concretly present in its parent</param>
        /// <returns>A new AirportZone object.</returns>
        public override AirportZone CreateZone( AirportArea area,
                                                Index position,
                                                string image,
                                                bool isBuilt )
        {
            return new Sorter( area, position, image, isBuilt );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets unique instance of the factory
        /// </summary>
        /// <returns>Unique instance of SorterFactory.</returns>
        public static SorterFactory GetInstance( )
        {
            if ( instance == null )
            {
                instance = new SorterFactory( );
            }

            return instance;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Initialize a new instance of SorterFactory.
        /// </summary>
        private SorterFactory( )
        {
        }

        // --------------------------------------------------------------------

        private static SorterFactory instance = null;
    }
}
