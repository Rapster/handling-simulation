using Airport.Controls;
using System;

namespace Airport.Tools
{
    /// <summary>
    /// Represents a luggage.
    /// </summary>
    public class Luggage
    {
        /// <summary>
        /// Initialize a new instance of Luggage.
        /// </summary>
        /// <param name="checkIn">Start AirportZone</param>
        /// <param name="gate">End AirportZone.</param>
        public Luggage( CheckIn checkIn,
                        DestinationGate gate )
        {
            Id = Guid.NewGuid( );
            Start = checkIn;
            End = gate;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets a reference to the destination gate of the luggage
        /// </summary>
        public DestinationGate End
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets a reference to the check-in gate of the luggage
        /// </summary>
        public CheckIn Start
        {
            get;
            set;
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Gets a unique Id
        /// </summary>
        public Guid Id
        {
            get;
            private set;
        }
    }
}
