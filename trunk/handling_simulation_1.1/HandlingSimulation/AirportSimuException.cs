using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nth.Eindhoven.Fontys
{
    class AirportSimuException : Exception
    {
        public AirportSimuException( string message, string advice )
            : base( message )
        {
            Advice = advice;
        }

        // --------------------------------------------------------------------

        public string Advice
        {
            get;
            set;
        }
    }
}
