using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Airport.Forms
{
    [Serializable]
    public class DestinationGate : AirportZone
    {
        public DestinationGate( )
            : base( new Point( 0, 0 ) )
        {

        }

        private Storage storage;
        private Flight flight;
    }
}
