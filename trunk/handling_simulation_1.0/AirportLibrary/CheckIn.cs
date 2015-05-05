using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Airport.Forms
{
    [Serializable]
    public class CheckIn : AirportZone
    {
        public CheckIn( )
            : base( new Point( 0, 0) )
        {

        }

        public override string ToString( )
        {
            return base.ToString( );
        }
    }
}
