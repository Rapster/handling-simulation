using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airport.Controls
{
    /// <summary>
    /// Represents a set of Way.
    /// </summary>
    public class Ways
    {
        /// <summary>
        /// Initialize a new instance of Ways. A set of Way.
        /// </summary>
        public Ways( )
        {
            Children = new List<Way>( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the way's collection
        /// </summary>
        public List<Way> Children
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets a conveyor collection containing all the conveyor involved in the Ways
        /// </summary>
        public List<Conveyor> Conveyors
        {
            get
            {
                List<Conveyor> tmp = new List<Conveyor>( );
                foreach ( Way way in Children )
                {
                    tmp.AddRange( way.Conveyors );
                }

                return tmp;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Add a way to the end of the way collection.
        /// </summary>
        /// <param name="way">Way to add</param>
        public void Add( Way way )
        {
            Children.Add( way );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Return the shortest way of the specified collection
        /// </summary>
        /// <param name="ways">Way collection where the shorstest distance will 
        /// be calculate</param>
        /// <returns>Returns the shortest way.</returns>
        public static Way ShortestWay( List<Way> ways )
        {
            if ( ways != null
                && ways.Count == 0 )
            {
                return null;
            }

            Way min = ways[ 0 ];

            for ( int i = 1; i < ways.Count; ++i )
            {
                if ( ways[ i ].Distance < min.Distance )
                {
                    min = ways[ i ];
                }
            }

            return min;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Return the longest way of the specified collection
        /// </summary>
        /// <param name="ways">Way collection where the longest distance will 
        /// be calculate</param>
        /// <returns>Returns the longest way.</returns>
        public static Way LongestWay( List<Way> ways )
        {
            if ( ways != null
                 && ways.Count == 0 )
            {
                return null;
            }

            Way max = ways[ 0 ];

            for ( int i = 1; i < ways.Count; ++i )
            {
                if ( ways[ i ].Distance > max.Distance )
                {
                    max = ways[ i ];
                }
            }

            return max;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        public Way GetAvailableWay( AirportZone zone )
        {
            List<Way> tmp = new List<Way>( );

            foreach ( Way way in Children )
            {
                if ( way.Zones.Contains( zone ) )
                {
                    tmp.Add( way );
                }
            }

            return tmp.Count == 0
                   || tmp.Count > 1
                   || zone == null
                   ? null
                   : tmp[ 0 ];
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets a way collection involved by the specified conveyor.
        /// </summary>
        /// <param name="conveyor"></param>
        /// <returns></returns>
        public List<Way> WaysInvolvedBy( Conveyor conveyor )
        {
            List<Way> tmp = new List<Way>( );

            foreach ( Way way in Children )
            {
                if ( way.Conveyors.Contains( conveyor ) )
                {
                    tmp.Add( way );
                }
            }

            return tmp;
        }

        // --------------------------------------------------------------------
    }
}
