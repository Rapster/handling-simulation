using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Airport.Utils;

namespace Airport.Forms
{
    [Serializable]
    class Storage : AirportZone
    {
        public Storage( )
            : base( new Point( 0, 0 ) )
        {
            luggages = new List<Luggage>( );
        }

        private List<Luggage> luggages;
    }
}
