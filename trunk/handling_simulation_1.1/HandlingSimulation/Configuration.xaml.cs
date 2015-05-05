using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Airport.Controls;
using Airport.Utils;
using Airport.Tools;

namespace Nth.Eindhoven.Fontys
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : Window
    {
        public Configuration( )
        {
            InitializeComponent( );
            Packages = new List<Package>( );
            ways = new List<Way>( );
        }

        // --------------------------------------------------------------------

        public double Interval
        {
            get
            {
                return Convert.ToDouble( txtInterval.Text );
            }
        }

        // --------------------------------------------------------------------

        public string EndPointAdress
        {
            get
            {
                return txtName.Text;
            }
        }

        // --------------------------------------------------------------------

        public uint Port
        {
            get
            {
                return Convert.ToUInt32( txtPort.Text );
            }
        }

        // --------------------------------------------------------------------

        public string Ip
        {
            get
            {
                return txtIp.Text;
            }
        }

        // --------------------------------------------------------------------

        public List<Way> Ways
        {
            get
            {
                return ways;
            }
            set
            {
                List<AirportZone> tmp = new List<AirportZone>( );

                foreach ( Way way in value )
                {
                    if ( !tmp.Contains( way.Start ) )
                    {
                        tmp.Add( way.Start );
                    }
                }

                cbBoxCheckIns.ItemsSource = tmp;
                ways = value;
            }
        }

        // --------------------------------------------------------------------

        public List<Package> Packages
        {
            get
            {
                return packages;
            }
            set
            {
                packages = value;
                gridPackages.ItemsSource = packages;
            }
        }

        // --------------------------------------------------------------------

        public void Clear( )
        {
            Packages.Clear( );
            Ways.Clear( );
        }

        // --------------------------------------------------------------------

        private void ClickOnCancel( object sender, RoutedEventArgs e )
        {
            Close( );
        }

        // --------------------------------------------------------------------

        private void ClickOnOk( object sender, RoutedEventArgs e )
        {
            ServerConfig.Ip = txtIp.Text;
            ServerConfig.Port = Convert.ToUInt32( txtPort.Text );
            ServerConfig.EndPointAdress = txtName.Text;

            Close( );
        }

        // --------------------------------------------------------------------

        private void ModifyAmount( object sender, TextCompositionEventArgs e )
        {
            int value = ( txtBoxAmount.Text != String.Empty )
                          ? Convert.ToInt32( txtBoxAmount.Text )
                          : 0;

            txtBoxAmount.Text = e.Text == "+"
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

        private void GridPackagesKeyUp( object sender, KeyEventArgs e )
        {
            if ( e.Key == Key.Delete )
            {
                DeletePackage( sender, e );
            }
        }

        // --------------------------------------------------------------------

        private void WindowClosing( object sender, System.ComponentModel.CancelEventArgs e )
        {
            Hide( );
            e.Cancel = true;
        }

        // --------------------------------------------------------------------

        private void AddPackage( object sender, RoutedEventArgs e )
        {
            int amount = 0;
            List<Way> result = null;

            if ( cbBoxCheckIns.SelectedItem != null
                 && cbBoxGates.SelectedItem != null
                 && ( amount = Convert.ToInt32( txtBoxAmount.Text ) ) > 0
                 && ( result = ways.FindAll( delegate( Way way )
                 {
                     return way.Start == cbBoxCheckIns.SelectedItem
                            && way.End == cbBoxGates.SelectedItem;
                 } ) ) != null )
            {
                packages.Add( new Package( amount,
                              Airport.Controls.Ways.ShortestWay( result ) ) );

                gridPackages.Items.Refresh( );
            }

            if ( PackagesModified != null )
            {
                PackagesModified( this, e );
            }
        }

        // --------------------------------------------------------------------

        private void DeletePackage( object sender, RoutedEventArgs e )
        {
            for ( int i = gridPackages.SelectedItems.Count - 1; i >= 0; --i )
            {
                Packages.RemoveAt( i );
            }

            gridPackages.Items.Refresh( );

            if ( PackagesModified != null )
            {
                PackagesModified( this, e );
            }
        }

        // --------------------------------------------------------------------

        private void CheckInsSelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            if ( cbBoxCheckIns.SelectedItem != null )
            {
                cbBoxGates.ItemsSource = ( cbBoxCheckIns.SelectedItem as CheckIn ).Destinations;
            }
        }

        // --------------------------------------------------------------------

        public new bool? ShowDialog( )
        {
            cbBoxCheckIns.Items.Refresh( );
            cbBoxGates.Items.Refresh( );
            gridPackages.Items.Refresh( );

            return base.ShowDialog( );
        }

        // --------------------------------------------------------------------

        private void GridPackagesSelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            if ( WaySelected != null
                && gridPackages.SelectedItem != null )
            {
                WaySelected( this, ( gridPackages.SelectedItem as Package ).Way );
            }
        }

        // --------------------------------------------------------------------

        private void ModifyInterval( object sender, TextCompositionEventArgs e )
        {
            if ( Char.IsDigit( e.Text, 0 ) )
            {
                txtInterval.Text = e.Text;
            }
            else
            {
                e.Handled = true;
            }
        }

        // --------------------------------------------------------------------

        private void TestConnexion( object sender, RoutedEventArgs e )
        {
            ServerConfig.Ip = Ip;
            ServerConfig.Port = Port;
            ServerConfig.EndPointAdress = EndPointAdress;

            try
            {
                ServerConfig.GetInstance( ).Services.GetConveyorSpeed( );
            }
            catch ( Exception ex )
            {
                txtOutput.AppendText( ex.Message + "\r\n" );
                return;
            }

            txtOutput.AppendText( "Connexion established on" + ServerConfig.Url + "\r\n" );
        }

        // --------------------------------------------------------------------

        private List<Way> ways;
        private List<Package> packages;

        public event EventHandler PackagesModified;
        public event WayHandler WaySelected;
    }

    public delegate void WayHandler( object sender, Way way );
}
