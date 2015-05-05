using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AirportServices
{
    [DataContract]
    [Serializable]
    public class HandlingReport
    {
        public HandlingReport( )
            : this( new List<HandlingEntry>( ) )
        {
        }

        // --------------------------------------------------------------------

        public HandlingReport( List<HandlingEntry> entries )
        {
            Entries = entries;
        }

        // --------------------------------------------------------------------

        public void AddEntry( HandlingEntry entry )
        {
            Entries.Add( entry );
        }

        // --------------------------------------------------------------------

        [DataMember]
        public List<HandlingEntry> Entries
        {
            get;
            set;
        }

        // --------------------------------------------------------------------
    }
}
