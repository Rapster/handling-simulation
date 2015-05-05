using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Airport.Utils;
using Airport.Tools;

namespace Airport.Controls
{
    /// <summary>
    /// Destination Gate. This is the sink component in the simulation.
    /// Serves as a final destination for packages. Additionally, it contains 
    /// internal storage, to which the packages are routed if their destination 
    /// plane is not present.
    /// </summary>
    public class DestinationGate : AirportZone
    {
        /// <summary>
        /// Initialize a new instance of DestinationGate with specified parameters.
        /// </summary>
        /// <param name="area">Parent reference to the AirportArea object that contains the zone.</param>
        /// <param name="position">X and Y coordinates within the visual grid.</param>
        /// <param name="image">Background image</param>
        /// <param name="storage">Storage where to put the luggages</param>
        /// <param name="isBuilt">Zone concretly present in its parent</param>
        /// <param name="planeIsPresent">Indicates the presence of a plane.</param>
        public DestinationGate( AirportArea area,
                                Index position,
                                string image,
                                Storage storage,
                                bool isBuilt,
                                bool planeIsPresent = false )
            : base( area, position, image, isBuilt )
        {
            storage.Gate = this;
            Storage = storage;
            PlaneIsPresent = planeIsPresent;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Initialize a new DestinationGate.
        /// </summary>
        public DestinationGate( )
            : this( null,
                    new Index( 0,0),
                    String.Empty,
                    new Storage( ),
                    false )
        {
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// A reference to the internal storage.
        /// </summary>
        public Storage Storage
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Get plane state
        /// </summary>
        public bool PlaneIsPresent
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Get component's name
        /// </summary>
        public override string Name
        {
            get
            {
                return "D" + Rank.ToString( );
            }
        }
    }
}
