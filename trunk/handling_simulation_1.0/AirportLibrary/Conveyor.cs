using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Airport.Forms;

namespace Airport.Utils
{
    public class Conveyor
    {
        public Conveyor( AirportZone start, AirportZone end )
        {
            Start = start;
            End = end;
        }

        // --------------------------------------------------------------------

        public bool IsValid
        {
            get
            {
                int tolerance = 10;
                return ( Math.Abs( Angle( ) - 180 ) <= tolerance
                        || Math.Abs( angle - 90 ) <= tolerance
                        || Math.Abs( angle - 45 ) <= tolerance );
            }
        }

        // --------------------------------------------------------------------

        public double Angle( )
        {
            if ( Start != null
                && End != null )
            {
                return angle = ( Math.Atan2( End.Location.Y - Start.Location.Y,
                                             End.Location.X - Start.Location.X )
                                             + Math.PI / 2 )
                                             * ( 180 / Math.PI );
            }

            return 0.0;
        }

        // --------------------------------------------------------------------

        public AirportZone Start
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        public AirportZone End
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        private double angle;

    }
}
