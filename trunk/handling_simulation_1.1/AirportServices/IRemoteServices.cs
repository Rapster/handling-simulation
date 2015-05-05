using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace AirportServices
{
    [ServiceContract]
    public interface IRemoteServices
    {
        [OperationContract]
        double GetConveyorSpeed( );

        // --------------------------------------------------------------------

        [OperationContract]
        double GetInterval( );

        // --------------------------------------------------------------------

        [OperationContract]
        int GetStorageCapacity( );

        // --------------------------------------------------------------------

        [OperationContract]
        void UpdateHandlingReport( HandlingEntry report );

        // --------------------------------------------------------------------

        [OperationContract]
        void SaveHandlingReport( );

        // --------------------------------------------------------------------

        event EntryHandler ReportUpdated;
        event ReportHandler ReportCompleted;
    }

    public delegate void EntryHandler( object sender, HandlingEntry entry );
    public delegate void ReportHandler( object sender, string filename );
}
