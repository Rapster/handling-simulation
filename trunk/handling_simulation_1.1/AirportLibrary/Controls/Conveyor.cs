using System;
using System.Windows;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Windows.Media;

namespace Airport.Controls
{
    /// <summary>
    /// Represents a segment of a conveyor line.
    /// </summary>
    public class Conveyor
    {
        /// <summary>
        /// Initialize a new instance of a conveyor.
        /// </summary>
        public Conveyor( )
            : this( new AirportZone( ),
                    new AirportZone( ) )
        {
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Initialize a new instance of a conveyor with its two extremities.
        /// </summary>
        public Conveyor( AirportZone start, AirportZone end )
        {
            Start = start;
            End = end;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// The angle of the conveyor segment on the drawing canvas.
        /// </summary>
        public double Angle
        {
            get
            {
                if ( Start != null
                    && End != null )
                {
                    return ( Math.Atan2( End.Center.Y - Start.Center.Y,
                                         End.Center.X - Start.Center.X ) )
                                         * ( -180 / Math.PI );
                }

                return 0.0;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Get the first zone involved by the conveyor
        /// </summary>
        public AirportZone Start
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Get the last zone involved by the conveyor
        /// </summary>
        public AirportZone End
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Copy this object in a new piece of memory
        /// </summary>
        /// <returns></returns>
        public Conveyor Clone( )
        {
            return new Conveyor( Start, End );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// A conveyor segment is valid (allowed to be placed in the simulation layout)
        /// if its angle is a multiple of 45 degrees, combined with a 5 degree tolerance.
        /// </summary>
        public bool IsValid
        {
            get
            {
                double angle = Angle;
                return ( angle == 0.0
                        || ( 40 <= Math.Abs( angle )
                        && Math.Abs( angle ) <= 50 )
                        || ( 85 <= Math.Abs( angle )
                        && Math.Abs( angle ) <= 95 )
                        || ( 130 <= Math.Abs( angle )
                        && Math.Abs( angle ) <= 140 )
                        || ( 175 <= Math.Abs( angle )
                        && Math.Abs( angle ) <= 185 ) )
                        && Start != null
                        && End != null
                        && Start != End
                        && ( End is Sorter )
                             ? ( End as Sorter ).Input == null
                             ? true
                             : false
                             : true;
            }
        }

        // --------------------------------------------------------------------
        /// <summary>
        /// The pixel length of the conveyor segment.
        /// </summary>
        public double Distance
        {
            get
            {
                return Math.Sqrt( Math.Pow( End.Center.Y - Start.Center.Y, 2 ) +
                                  Math.Pow( End.Center.X - Start.Center.X, 2 ) );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Returns whether the conveyor segment is connected to a particular AirportZone
        /// </summary>
        /// <param name="zone">A reference to the particular AirportZone</param>
        /// <returns>True if the zone is either the starting or the ending point of the conveyor segment</returns>
        public bool BelongsTo( AirportZone zone )
        {
            return Start.Position == zone.Position
                   || End.Position == zone.Position;
        }

        // --------------------------------------------------------------------
    }
}
