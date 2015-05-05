using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace AirportServices
{
    [DataContract]
    [Serializable]
    public class HandlingEntry
    {
        public HandlingEntry( )
            : this( DateTime.Now, null )
        {
        }

        // --------------------------------------------------------------------

        public HandlingEntry( DateTime date, string description )
        {
            Date = date;
            Description = description;
        }
        
        // --------------------------------------------------------------------

        [DataMember]
        public DateTime Date
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        [DataMember]
        public string Description
        {
            get;
            set;
        }
    }
}
