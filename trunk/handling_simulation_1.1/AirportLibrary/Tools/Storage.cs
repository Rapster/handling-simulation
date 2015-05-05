using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Airport.Tools;
using Airport.Utils;
using Airport.Controls;

namespace Airport.Tools
{
    /// <summary>
    /// Represents a container of luggage.
    /// </summary>
    public class Storage : AirportZone
    {
        /// <summary>
        /// Initialize a new instance of storage. This object contain a set of luggage.
        /// </summary>
        public Storage( )
            : this( null, 0 )
        {
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Initialize a new instance of storage. This object contain a set of luggage.
        /// </summary>
        /// <param name="gate">Destination gate that contains the storage component</param>
        /// <param name="capacity">Maximal number of luggages contained in a storage</param>
        public Storage( DestinationGate gate, int capacity )
        {
            Storage.capacity = capacity;
            Luggages = new List<Luggage>( capacity );
            Gate = gate;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Collection of all the pieces of luggage within the storage component.
        /// </summary>
        public List<Luggage> Luggages
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Destination gate that contains the storage component.
        /// </summary>
        public DestinationGate Gate
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Add a luggage in the storage.
        /// </summary>
        /// <param name="luggage">Luggage to add</param>
        public void AddLuggage( Luggage luggage )
        {
            if ( Luggages.Count <= Luggages.Capacity )
            {
                Luggages.Add( luggage );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Add a set of luggage into the storage.
        /// </summary>
        /// <param name="luggages"></param>
        public void AddLuggages( List<Luggage> luggages )
        {
            if ( Luggages.Count + luggages.Count <= Luggages.Capacity )
            {
                Luggages.AddRange( luggages );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the unique ID of the linked destination gate.
        /// </summary>
        public override int Rank
        {
            get
            {
                return Gate != null
                        ? Gate.Rank
                        : -1;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets component's name
        /// </summary>
        public override string Name
        {
            get
            {
                return "S" + Rank.ToString( );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Indicates if the number of contained luggages is equals to the maximum 
        /// capacity of the Storage.
        /// </summary>
        public bool IsFull
        {
            get
            {
                return Luggages.Count >= Storage.capacity;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Storage capacity.
        /// </summary>
        public static int capacity;
    }
}
