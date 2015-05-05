using Airport.Controls;
using System.Collections.Generic;
using System;

namespace Airport.Tools
{
    /// <summary>
    /// This component permit to assign some luggages on a way.
    /// </summary>
    /// <example>
    /// <code lang="c#">
    /// int interval = 1;
    /// Package.interval = 2;
    ///
    /// foreach ( Package pkg in packages )
    ///{
    ///    if ( pkg.Way.Zones.OfType&lt;Sorter&gt;( ).Count( ) == 0 )
    ///    {
    ///        pkg.AddLuggages( );
    ///        continue;
    ///    }
    ///    
    ///    int end = pkg.Luggages.Capacity;
    ///    
    ///    for ( int i = 0; i &lt; end; ++i )
    ///    {
    ///        if ( pkg.IsOverloaded( i, interval ) )
    ///        {
    ///            continue;
    ///        }
    ///        
    ///        pkg.AddLuggage( );
    ///    }
    ///}
    /// </code>
    /// </example>
    public class Package
    {
        /// <summary>
        /// Initialize a new instance Package. This object permit to assign 
        /// some luggages on a way,
        /// with a number of luggage and the assigned way.
        /// </summary>
        /// <param name="amount">Number of luggage to set</param>
        /// <param name="way">Way to set to these luggage</param>
        public Package( int amount, Way way )
        {
            Way = way;
            Start = way.Start as CheckIn;
            End = way.End;

            this.amount = amount;
            Luggages = new List<Luggage>( amount );
            entries = new Dictionary<Luggage, double>( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Initialize a new instance Package. This object permit to assign 
        /// some luggages on a way.
        /// </summary>
        public Package( )
            : this( 0, new Way( ) )
        {
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Get the first element of specified way
        /// </summary>
        public CheckIn Start
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Get the last element of specified way
        /// </summary>
        public DestinationGate End
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the way assigned to the Package
        /// </summary>
        public Way Way
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the set of luggage
        /// </summary>
        public List<Luggage> Luggages
        {
            get;
            private set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Add a new luggage in the Package object
        /// </summary>
        /// <returns>0 if Start and End are different than null, otherwise -1</returns>
        public int AddLuggage( )
        {
            if ( Start != null
                && End != null )
            {
                amount = Luggages.Capacity = Luggages.Count == Luggages.Capacity
                                                ? ++Luggages.Capacity
                                                : Luggages.Capacity;

                Luggages.Add( new Luggage( Start, End ) );
                entries.Add( Luggages[ Luggages.Count - 1 ],
                             currentTime * Package.interval );

                return 0;
            }

            return -1;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Add a set of luggage corresponding to the rest of luggages to add.
        /// </summary>
        /// <returns>-1 if luggages count is greater than luggages capacity, otherwise returns 0.</returns>
        public int AddLuggages( )
        {
            for ( int i = Luggages.Count; i < Luggages.Capacity; ++i )
            {
                if ( Luggages.Capacity > Luggages.Count )
                {
                    Luggages.Add( new Luggage( Start, End ) );
                }
                else
                {
                    return -1;
                }
            }

            return 0;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Get the last luggage added into the Package object.
        /// </summary>
        /// <returns>Returns the last luggage, or if there is no, returns null</returns>
        public Luggage Last( )
        {
            return Luggages.Count > 0
                   ? Luggages[ Luggages.Count - 1 ]
                   : null;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Indicate if the package object is overloaded.
        /// </summary>
        /// <param name="index">Position of a luggage</param>
        /// <param name="interval">interval between two luggage</param>
        /// <returns>True if the package is overloaded, otherwise false</returns>
        public bool IsOverloaded( int index, double interval )
        {
            Luggage luggage = Last( );
            currentTime = index;

            if ( luggage != null )
            {
                double t1 = entries[ luggage ];
                double t2 = index * Package.interval;

                return Math.Abs( t1 - t2 ) < interval;
            }

            return false;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Clear all the luggages added into the Package object and into the Storage.
        /// Luggages amount is reinitialize to the amount passed in parameter of the constructor.
        /// </summary>
        public void ReInitialiaze( )
        {
            Luggages.Clear( );
            End.Storage.Luggages.Clear( );
            entries.Clear( );

            Luggages.Capacity = amount;
            currentTime = 0;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the name of the package
        /// </summary>
        public string Name
        {
            get
            {
                return Luggages.Capacity + "" + End.Name;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Interval separating two luggage
        /// </summary>
        public static double interval;

        private int currentTime;
        private int amount;
        private Dictionary<Luggage, double> entries;
    }
}
