using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Airport.Utils
{
    [Serializable]
    public class Luggage
    {
        public Luggage( )
        {

        }

        public Guid DestinationId
        {
            get;
            set;
        }

        public int MoveOn( Conveyor conveyor )
        {
            return 0;
        }
    }
}
