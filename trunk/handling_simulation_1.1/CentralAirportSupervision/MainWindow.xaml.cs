using System.Windows;
using AirportServices;
using System.ServiceModel;
using System;
using System.Windows.Controls;

namespace CentralAirportSupervision
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow( )
        {
            InitializeComponent( );
            CreateServer( );
        }

        // --------------------------------------------------------------------

        void ReportIsComplete( object sender, string filename )
        {
            lstOutput.Items.Add( new HandlingEntry( DateTime.Now,
                "Report " + filename + " was created! Containing " + lstOutput.Items.Count + " tests" ) );
        }

        // --------------------------------------------------------------------

        void ReportWasUpdated( object sender, HandlingEntry entry )
        {
            lstOutput.Items.Add( entry );
        }

        // --------------------------------------------------------------------

        private void StartServer( )
        {
            host.Open( );
        }

        // --------------------------------------------------------------------

        private void StopServer( )
        {
            if ( host != null
                 && host.State == CommunicationState.Opened )
            {
                host.Close( );
            }
        }

        // --------------------------------------------------------------------

        private void UpdateServerState( object sender, RoutedEventArgs e )
        {
            if ( host != null
                && host.State == CommunicationState.Opened )
            {
                StopServer( );
            }
            else if ( host != null
                      && ( host.State == CommunicationState.Closed
                      || host.State == CommunicationState.Created )
                      || host == null )
            {
                CreateServer( );
                StartServer( );
            }

            itemState.Content = host.State;
            btnStart.Content = host.State == CommunicationState.Opened
                               ? "Stop"
                               : "Start";
        }

        // --------------------------------------------------------------------

        private void CreateServer( )
        {
            if ( host != null )
            {
                host.Close( );
            }

            remoteServices = new RemoteServices( Convert.ToDouble( txtSpeed.Text ),
                                                 Convert.ToInt32( txtCapacity.Text ),
                                                 Convert.ToDouble( txtInterval.Text ) );
            //Publish the singleton class
            host = new ServiceHost( remoteServices,
                                    new Uri[ ]{
                                    new Uri( "http://localhost:" + Convert.ToUInt32( txtPort.Text ) )} );

            host.AddServiceEndpoint( typeof( IRemoteServices ),
                         new BasicHttpBinding( ),
                         txtDomain.Text );

            remoteServices.ReportUpdated += new EntryHandler( ReportWasUpdated );
            remoteServices.ReportCompleted += new ReportHandler( ReportIsComplete );
        }

        // --------------------------------------------------------------------

        private void Clear( object sender, RoutedEventArgs e )
        {
            lstOutput.Items.Clear( );
        }

        // --------------------------------------------------------------------

        private void ModifyInterval( object sender, System.Windows.Input.TextCompositionEventArgs e )
        {
            ModifyContentTextBox( sender as TextBox, e );
        }

        // --------------------------------------------------------------------

        private void ModifyContentTextBox( TextBox txtBox, System.Windows.Input.TextCompositionEventArgs e )
        {
            int value = ( txtBox.Text != String.Empty )
                          ? Convert.ToInt32( txtBox.Text )
                          : 0;

            txtBox.Text = e.Text == "+"
                          ? ( ++value ).ToString( )
                          : e.Text == "-"
                          && value > 0
                          ? ( --value ).ToString( )
                          : value.ToString( );

            if ( !Char.IsDigit( e.Text, 0 ) )
            {
                e.Handled = true;
            }
        }

        // --------------------------------------------------------------------

        private static IRemoteServices remoteServices;
        private static ServiceHost host;
    }
}
