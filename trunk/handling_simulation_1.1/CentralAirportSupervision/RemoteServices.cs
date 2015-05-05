using System;
using System.ServiceModel;
using AirportServices;
using System.IO;
using System.Xml.Serialization;

namespace CentralAirportSupervision
{
    [ServiceBehavior( InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Single )]
    class RemoteServices : IRemoteServices
    {
        public RemoteServices( double speed, int capacity, double interval )
        {
            this.speed = speed;
            this.capacity = capacity;
            this.interval = interval;
            report = new HandlingReport( );
        }

        // --------------------------------------------------------------------

        public double GetConveyorSpeed( )
        {
            return speed;
        }

        // --------------------------------------------------------------------

        public int GetStorageCapacity( )
        {
            return capacity;
        }

        // --------------------------------------------------------------------

        public void UpdateHandlingReport( HandlingEntry entry )
        {
            report.AddEntry( entry );

            if ( ReportUpdated != null )
            {
                ReportUpdated( this, entry );
            }
        }

        // --------------------------------------------------------------------

        public double GetInterval( )
        {
            return interval;
        }

        // --------------------------------------------------------------------

        public void SaveHandlingReport( )
        {
            string filename = "hsm_report_" + DateTime.Now.ToString( )
                                .Replace( '/', '-' )
                                .Replace( " ", "--" )
                                .Replace( ':', '-' ) + ".xml";

            using ( FileStream fs = new FileStream( filename, FileMode.Create ) )
            {
                XmlSerializer serializer = new XmlSerializer( typeof( HandlingReport ) );
                serializer.Serialize( fs, report );
            }

            if ( ReportCompleted != null )
            {
                ReportCompleted( this, filename );
            }

            report.Entries.Clear( );
        }

        // --------------------------------------------------------------------

        public event EntryHandler ReportUpdated;
        public event ReportHandler ReportCompleted;

        private double speed;
        private int capacity;
        private double interval;
        private HandlingReport report;
    }
}
