using Airport.Controls;
using Airport.Tools;

namespace Airport.Utils.Factories
{
    /// <summary>
    /// DestinationGate factory permitting to create DestinationGate object.
    /// </summary>
    class DestinationGateFactory : AirportFactory
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
            return new DestinationGate( area, index, image, new Storage( ), isBuilt );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets unique instance of the factory
        /// </summary>
        /// <returns>Unique instance of DestinationGateFactory.</returns>
        public static DestinationGateFactory GetInstance( )
        {
            if ( instance == null )
            {
                instance = new DestinationGateFactory( );
            }

            return instance;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Initialize a new instance of DestinationGateFactory.
        /// </summary>
        private DestinationGateFactory( )
        {
        }

        // --------------------------------------------------------------------

        private static DestinationGateFactory instance = null;
    }
}
