using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Airport.Utils;

namespace Airport.Controls
{
    /// <summary>
    /// Check-in gate. Source of packages
    /// </summary>
    public class CheckIn : AirportZone
    {
        /// <summary>
        /// Initialize a new instance of a CheckIn
        /// </summary>
        public CheckIn( )
            : base( null,
                    new Index( 0, 0 ),
                    String.Empty )
        {
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Instanciate a new instance of a CheckIn
        /// </summary>
        /// <param name="area">Parent reference to the AirportArea object that contains the zone.</param>
        /// <param name="position">X and Y coordinates within the visual grid.</param>
        /// <param name="image">Background image</param>
        /// <param name="isBuilt">Zone concretly present in its parent</param>
        public CheckIn( AirportArea area,
                        Index position,
                        string image,
                        bool isBuilt )
            : base( area,
                    position,
                    image,
                    isBuilt )
        {
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the output conveyor of the CheckIn
        /// </summary>
        public Conveyor Output
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
                return "C" + Rank.ToString( );
            }
        }
    }
}
