
namespace Airport.Utils
{
    public class Monitor
    {
        private Monitor( )
        {
            AllowRefresh = true;
        }

        // --------------------------------------------------------------------

        public static Monitor Instance( )
        {
            if ( instance == null )
            {
                instance = new Monitor( );
            }

            return instance;
        }

        // --------------------------------------------------------------------

        public bool AllowRefresh
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        private static Monitor instance = null;
    }
}
