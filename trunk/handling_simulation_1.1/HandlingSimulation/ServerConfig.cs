using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AirportServices;
using System.ServiceModel;

namespace Nth.Eindhoven.Fontys
{
    public class ServerConfig
    {
        public static ServerConfig GetInstance( )
        {
            if ( instance == null )
            {
                instance = new ServerConfig( );
            }

            return instance;
        }

        // --------------------------------------------------------------------

        public static string Ip
        {
            get;
            internal set;
        }

        // --------------------------------------------------------------------

        public static uint Port
        {
            get;
            internal set;
        }

        // --------------------------------------------------------------------

        public static string EndPointAdress
        {
            get;
            internal set;
        }

        // --------------------------------------------------------------------

        public static string Url
        {
            get
            {
                return "http://" + Ip + ":" + Port + "/" + EndPointAdress;
            }
        }

        // --------------------------------------------------------------------

        public IRemoteServices Services
        {
            get
            {
                httpFactory = new ChannelFactory<IRemoteServices>(
                              new BasicHttpBinding( ),
                              new EndpointAddress( Url ) );

                httpFactory.Open( );
                httpProxy = httpFactory.CreateChannel( );
                return httpProxy;
            }
        }

        // --------------------------------------------------------------------

        public void Close( )
        {
            if ( httpFactory != null )
            {
                httpFactory.Close( );
            }
        }

        // --------------------------------------------------------------------

        private ServerConfig( )
        {
        }

        // --------------------------------------------------------------------

        private IRemoteServices httpProxy;
        private ChannelFactory<IRemoteServices> httpFactory;
        private static ServerConfig instance;
    }
}
