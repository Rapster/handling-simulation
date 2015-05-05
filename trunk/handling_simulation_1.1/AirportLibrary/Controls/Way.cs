using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airport.Controls
{
    /// <summary>
    /// Represents a follow of conveyor, starting with a 
    /// CheckIn and ending with a DestinationGate
    /// </summary>
    public class Way : IEquatable< Way >
    {
        /// <summary>
        /// Initialize a new Way.
        /// </summary>
        public Way( )
        {
            conveyors = new List<Conveyor>( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Initialize a new Way. Represents a follow of conveyor, starting with a 
        /// CheckIn and ending with a DestinationGate
        /// </summary>
        /// <param name="conveyors">Set of conveyors to add to the conveyors follow</param>
        public Way( List<Conveyor> conveyors )
        {
            Conveyors = conveyors;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the conveyors contained by the Way
        /// </summary>
        public List<Conveyor> Conveyors
        {
            get { return conveyors; }
            set { conveyors = value; }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the first element of the Way
        /// </summary>
        public AirportZone Start
        {
            get
            {
                return conveyors.Count != 0
                        ? conveyors[ 0 ].Start
                        : null;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the last element of the Way
        /// </summary>
        public DestinationGate End
        {
            get
            {
                return conveyors.Count != 0
                        ? conveyors[ conveyors.Count - 1 ].End as DestinationGate
                        : null;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Add a conveyor to the end of the conveyor collection.
        /// </summary>
        /// <param name="conveyor">Conveyor to add</param>
        public void Add( Conveyor conveyor )
        {
            conveyors.Add( conveyor );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Add the conveyors to the end of the conveyor collection.
        /// </summary>
        /// <param name="conveyors"></param>
        public void AddRange( List<Conveyor> conveyors )
        {
            Conveyors.AddRange( conveyors );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Remove the first conveyor of the specific conveyor from the 
        /// conveyor's collection.
        /// </summary>
        /// <param name="conveyor">Conveyor to remove</param>
        public void Remove( Conveyor conveyor )
        {
            conveyors.Remove( conveyor );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Remove all conveyors contained in the conveyor collection.
        /// </summary>
        public void Clear( )
        {
            conveyors.Clear( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the number of conveyor contained in the Way
        /// </summary>
        public int Count
        {
            get { return conveyors.Count; }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Return true if all the conveyor are valid and start by a check-in 
        /// and end by a destination-gate
        /// </summary>
        public bool IsValid
        {
            get
            {
                bool error = conveyors.Count > 0
                             && Start is CheckIn
                             && End is DestinationGate;

                for ( int i = 0; i < conveyors.Count - 1; ++i )
                {
                    if ( !conveyors[ i ].IsValid
                         && !conveyors[ i ].End.Equals( conveyors[ i + 1 ].Start ) )
                    {
                        return false;
                    }
                }

                return error;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Get the length in pixel of the Way.
        /// </summary>
        public double Distance
        {
            get
            {
                double distance = 0;
                foreach ( Conveyor c in Conveyors )
                {
                    distance += c.Distance;
                }

                return distance;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets all the node of the Way.
        /// </summary>
        public List<AirportZone> Zones
        {
            get
            {
                List<AirportZone> tmp = new List<AirportZone>( );

                if ( Conveyors.Count != 0 )
                {
                    tmp.Add( Conveyors[ 0 ].Start );
                }

                foreach ( Conveyor conveyor in Conveyors )
                {
                    tmp.Add( conveyor.End );
                }

                return tmp;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Build a new way with two piece of way
        /// </summary>
        /// <param name="lWay">First element</param>
        /// <param name="rWay">Second element</param>
        /// <returns>An aggregated way of the specified objects</returns>
        public static Way operator +( Way lWay, Way rWay )
        {
            lWay = lWay.Start is CheckIn
                   ? lWay
                   : rWay;

            rWay = rWay.Start is Sorter
                   ? rWay
                   : lWay;

            int i = 0;
            List<Conveyor> conveyors = new List<Conveyor>( );

            while ( lWay.Conveyors[ i ].Start != rWay.Start )
            {
                conveyors.Add( lWay.Conveyors[ i ] );
                ++i;
            }

            conveyors.AddRange( rWay.Conveyors );
            return new Way( conveyors );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Build a new way with two piece of way
        /// </summary>
        /// <param name="way">Element to add to the current Way</param>
        /// <returns>An aggregated way of the specified object</returns>
        public Way Append( Way way )
        {
            return this + way;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Check if the specified object have the same Start zone and End zone than the Way.
        /// </summary>
        /// <param name="other">Object to compare</param>
        /// <returns>True if Start and End are equals to the Way, otherwise false.</returns>
        public bool Equals( Way other )
        {
            return other != null
                   && Start != null
                   && End != null
                   && Start.Equals( other.Start )
                   && End.Equals( other.End );
        }

        // --------------------------------------------------------------------

        private List<Conveyor> conveyors;
    }
}
