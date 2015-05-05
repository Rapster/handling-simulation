using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Xml.Serialization;
using Airport.Controls;
using Airport.Tools;
using Airport.Utils;
using Microsoft.Win32;
using Airport.Utils.Factories;
using AirportServices;

namespace Nth.Eindhoven.Fontys
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HandlingSimulation : Window
    {
        public HandlingSimulation( )
        {
            InitializeComponent( );

            fileSystemWatcher = new FileSystemWatcher( );
            cfgWindow = new Configuration( );

            storyBoard = new Storyboard( );
            storyBoard.Completed += new EventHandler( SimulationIsDone );
            storyBoard.CurrentTimeInvalidated +=
                new EventHandler( StoryBoardCurrentTimeInvalidated );

            cfgWindow.PackagesModified +=
                new EventHandler( PackagesModified );
            cfgWindow.Closing +=
                new System.ComponentModel.CancelEventHandler( ConfigurationIsClosing );
            cfgWindow.WaySelected +=
                new WayHandler( WaySelectedFromConfiguration );

            drawingArea.AirportArea.DropNewAirportZone +=
                new AirportHandler( NewAirportZoneWasDropped );
            drawingArea.AirportArea.ClickOnAirportZone +=
                new AirportClickHandler( ClickOnAirportZone );

            paraTimeline = new ParallelTimeline( TimeSpan.FromSeconds( 0 ) );
            storyBoard.Children.Add( paraTimeline );

            ratio = Convert.ToDouble( txtRatio.Text );
        }

        // --------------------------------------------------------------------

        private void WaySelectedFromConfiguration( object sender, Way way )
        {
            drawingArea.ResetSelection( );
            drawingArea.DisplayRankLabels( );
            drawingArea.SetSelected( way );
        }

        // --------------------------------------------------------------------

        private void StoryBoardCurrentTimeInvalidated( object sender, EventArgs e )
        {
            if ( ( sender as ClockGroup ).CurrentTime != null )
            {
                ClockGroup clock = ( sender as ClockGroup );
                progressBar.Maximum = clock.NaturalDuration.TimeSpan.TotalSeconds;
                progressBar.Value = clock.CurrentTime.Value.TotalSeconds;
                txtProgress.Text = ( clock.CurrentTime.Value.TotalSeconds / progressBar.Maximum ).ToString( "P0",
                                        CultureInfo.InvariantCulture );

                txtSimuTime.Text = String.Format( "{0:00}:{1:00}:{2:00}",
                                                  clock.CurrentTime.Value.Hours,
                                                  clock.CurrentTime.Value.Minutes,
                                                  clock.CurrentTime.Value.Seconds );

                TimeSpan time = new TimeSpan( 0, 0, (int)( progressBar.Value * ratio ) );

                txtRealTime.Text = String.Format( "{0:00}:{1:00}:{2:00}",
                                                    time.Hours,
                                                    time.Minutes,
                                                    time.Seconds );
            }
        }

        // --------------------------------------------------------------------

        private void Update( string name )
        {
            Application app = System.Windows.Application.Current;
            if ( app != null )
            {
                app.Dispatcher.BeginInvoke( DispatcherPriority.Background,
                                           (Action)delegate
                                           {
                                               CallbackNewName( name );
                                           } );
            }
        }

        // --------------------------------------------------------------------

        private void PlotLineSelected( object sender, EventArgs e )
        {
            drawingArea.PlottingMode =
                (bool)( sender as ToggleButton ).IsChecked;
        }

        // --------------------------------------------------------------------

        private void NewDocument( object sender, RoutedEventArgs e )
        {
            if ( !fileModified
             && !fileSaved )
            {
                return;
            }

            MessageBoxResult ret = MessageBoxResult.Yes;

            if ( fileModified
                 && ( ret = MessageBox.Show( "Do you want to save the modificatons?",
                                             Name,
                                             MessageBoxButton.YesNoCancel,
                                             MessageBoxImage.Exclamation ) )
                                             == MessageBoxResult.Yes )
            {
                SaveDocument( sender, e );
            }
            else if ( ret == MessageBoxResult.Cancel )
            {
                return;
            }

            ResetAll( );
        }

        // --------------------------------------------------------------------

        private void OpenDocument( object sender, RoutedEventArgs e )
        {
            MessageBoxResult ret = MessageBoxResult.Yes;

            if ( fileModified
                && ( ret = MessageBox.Show( "The file has been updated, do you want to save the modifications?",
                                            Name,
                                            MessageBoxButton.YesNoCancel,
                                            MessageBoxImage.Exclamation ) )
                                            == MessageBoxResult.Yes )
            {
                SaveDocument( sender, e );
            }
            else if ( ret == MessageBoxResult.Cancel )
            {
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog( );
            openFileDialog.InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.Personal );
            openFileDialog.Filter = "Simulation Files (*.hsm)|*.hsm";

            if ( (bool)openFileDialog.ShowDialog( this ) )
            {
                AirportFile file = new AirportFile( );
                Deserialize( openFileDialog.FileName, ref file );
                ResetAll( );
                drawingArea.AirportArea.FillTable( file.Zones );
                drawingArea.FillDrawingArea( file.Ways );
                cfgWindow.Packages = file.Packages;
                InitializeFileSystemWatcher( openFileDialog.FileName );
                Update( openFileDialog.FileName );
            }
        }

        // --------------------------------------------------------------------

        private void SaveDocument( object sender, RoutedEventArgs e )
        {
            if ( fileSaved )
            {
                try
                {
                    Serialize( currentFile );
                    fileModified = false;
                    fileSaved = true;
                }
                catch ( Exception ex )
                {
                    MessageBox.Show( ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error );
                }
            }
            else
            {
                SaveAsDocument( sender, e );
            }
        }

        // --------------------------------------------------------------------

        private void SaveAsDocument( object sender, RoutedEventArgs e )
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog( );
            saveFileDialog.InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.Personal );
            saveFileDialog.Filter = "Simulation Files (*.hsm)|*.hsm";

            if ( (bool)saveFileDialog.ShowDialog( this ) )
            {
                try
                {
                    InitializeFileSystemWatcher( saveFileDialog.FileName );
                    Serialize( saveFileDialog.FileName );
                }
                catch ( Exception ex )
                {
                    MessageBox.Show( ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error );
                    return;
                }
            }
        }

        // --------------------------------------------------------------------

        private void Exit( object sender, RoutedEventArgs e )
        {
            Close( );
        }

        // --------------------------------------------------------------------

        private void InitializeFileSystemWatcher( string filename )
        {
            fileSystemWatcher.Filter = "*.hsm";
            fileSystemWatcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
            fileSystemWatcher.Changed += new System.IO.FileSystemEventHandler( this.FileChanged );
            fileSystemWatcher.Created += new System.IO.FileSystemEventHandler( this.FileCreated );
            fileSystemWatcher.Renamed += new System.IO.RenamedEventHandler( this.FileRenamed );
            fileSystemWatcher.Path = System.IO.Path.GetDirectoryName( filename );
            fileSystemWatcher.EnableRaisingEvents = true;
            currentFile = filename;
            fileSaved = true;
        }

        // --------------------------------------------------------------------

        private void FileChanged( object sender, FileSystemEventArgs e )
        {
            if ( e.FullPath == currentFile )
            {
                Update( e.FullPath );
                fileModified = false;
                fileSaved = true;
            }
        }

        // --------------------------------------------------------------------

        private void FileCreated( object sender, FileSystemEventArgs e )
        {
            if ( e.FullPath == currentFile )
            {
                Update( e.FullPath );
                fileModified = false;
                fileSaved = true;
            }
        }

        // --------------------------------------------------------------------

        private void FileRenamed( object sender, RenamedEventArgs e )
        {
            if ( e.OldFullPath == currentFile )
            {
                currentFile = e.FullPath;
                Update( e.FullPath );
            }
        }

        // --------------------------------------------------------------------

        private int Serialize( string filename )
        {
            List<AirportZone> zones = new List<AirportZone>( );
            FileStream fs = null;
            int error = 0;

            zones = drawingArea.Zones.FindAll( delegate( AirportZone zone )
            {
                return zone.IsBuilt;
            }
            );

            AirportFile file = new AirportFile( zones, drawingArea.Ways, cfgWindow.Packages );

            try
            {
                using ( fs = new FileStream( filename, FileMode.Create ) )
                {
                    XmlSerializer serializer = new XmlSerializer( typeof( AirportFile ) );
                    serializer.Serialize( fs, file );
                }
            }
            catch ( Exception )
            {
                error = -1;
            }
            finally
            {
                if ( fs != null )
                {
                    fs.Close( );
                }
            }

            return error;
        }

        // --------------------------------------------------------------------

        private int Deserialize( string filename, ref AirportFile airportFile )
        {
            if ( !System.IO.File.Exists( filename ) )
            {
                return -1;
            }

            FileStream fs = null;

            try
            {
                using ( fs = new FileStream( filename, FileMode.Open ) )
                {
                    XmlSerializer serializer = new XmlSerializer( typeof( AirportFile ) );
                    airportFile = serializer.Deserialize( fs ) as AirportFile;
                }
            }
            catch ( Exception )
            {
                fs.Close( );
                return -1;
            }

            foreach ( AirportZone zone in airportFile.Zones )
            {
                zone.Area = drawingArea.AirportArea;
            }

            fs.Close( );
            return 0;
        }

        // --------------------------------------------------------------------

        private void CallbackNewName( string name )
        {
            if ( Title.Contains( '*' ) )
            {
                Title.Remove( Title.LastIndexOf( '*' ) );
            }

            Title = Title.Split( '-' )[ 0 ] + "- " + name;
        }

        // --------------------------------------------------------------------

        private void InitializeAttributes( )
        {
            currentFile = String.Empty;
            fileSaved = false;
            fileModified = false;
            Title = "Handling Simulation 1.0 - New Document";
        }

        // --------------------------------------------------------------------

        private void NewAirportZoneWasDropped( AirportZone oldZone, AirportZone newZone )
        {
            Application app = System.Windows.Application.Current;

            if ( app != null )
            {
                app.Dispatcher.BeginInvoke( DispatcherPriority.Background,
                                           (Action)delegate
                                           {
                                               if ( !Title.Contains( '*' ) )
                                               {
                                                   Title += "*";
                                               }
                                           } );
            }

            AutoSave( );
            fileModified = true;
        }

        // --------------------------------------------------------------------

        private void DrawingAreaReleaseMouse( object sender, EventArgs e )
        {
            AutoSave( );
            itemsBox.PlottingButtonState = false;
            drawingArea.PlottingMode = false;
        }

        // --------------------------------------------------------------------

        private void MainWindowKeyDown( object sender, KeyEventArgs e )
        {
            drawingArea.SelectionMode = ( e.Key == Key.LeftCtrl
                                          || e.Key == Key.RightCtrl )
                                          ? AirportDrawing.SelectedMode.SelectSeveral
                                          : AirportDrawing.SelectedMode.SelectOne;
        }

        // --------------------------------------------------------------------

        private void MainWindowKeyUp( object sender, KeyEventArgs e )
        {
            drawingArea.SelectionMode = ( e.Key == Key.LeftCtrl
                                          || e.Key == Key.RightCtrl )
                                          ? AirportDrawing.SelectedMode.SelectOne
                                          : AirportDrawing.SelectedMode.SelectOne;
        }

        // --------------------------------------------------------------------

        private void DeleteSelectionClick( object sender, RoutedEventArgs e )
        {
            drawingArea.ClearSelection( );
        }

        // --------------------------------------------------------------------

        private void DeleteAllClick( object sender, RoutedEventArgs e )
        {
            drawingArea.Clear( );
        }

        // --------------------------------------------------------------------

        private void ConveyorIsDrawing( Conveyor sender, MouseEventArgs e )
        {
            txtDegree.Text = sender.Angle.ToString( "#.##",
                                                    CultureInfo.InvariantCulture.NumberFormat );
        }

        // --------------------------------------------------------------------

        private void SelectAll( object sender, RoutedEventArgs e )
        {
            drawingArea.SelectAll( );
        }

        // --------------------------------------------------------------------

        private void MainWindowClosing( object sender, System.ComponentModel.CancelEventArgs e )
        {
            MessageBoxResult ret = MessageBoxResult.Yes;

            if ( fileModified
                && ( ret = MessageBox.Show( "The file was updated, do you want to save the modifications?",
                                            Name,
                                            MessageBoxButton.YesNoCancel,
                                            MessageBoxImage.Exclamation ) )
                                            == MessageBoxResult.Yes )
            {
                SaveDocument( sender, null );
            }
            else if ( ret == MessageBoxResult.Cancel )
            {
                e.Cancel = true;
            }

            if ( e.Cancel == false )
            {
                ServerConfig.GetInstance( ).Close( );
                System.Environment.Exit( 0 );
            }
        }

        // --------------------------------------------------------------------

        private void AutoSave( )
        {
            string filename = ( currentFile != null )
            ? "~" + System.IO.Path.GetFileName( currentFile ) + ""
            : "~NewDocument-" + DateTime.Now.ToShortDateString( ).Replace( '/', '-' ) + ".hsm";

            Application app = System.Windows.Application.Current;
            if ( app != null )
            {
                app.Dispatcher.BeginInvoke( DispatcherPriority.Background,
                                           (Action)delegate
                                           {
                                               Serialize( filename );
                                           } );
            }
        }

        // --------------------------------------------------------------------

        private void UnSelectAll( object sender, RoutedEventArgs e )
        {
            drawingArea.UnSelectAll( );
        }

        // --------------------------------------------------------------------

        private void ResetAll( )
        {
            drawingArea.Clear( );
            InitializeAttributes( );
            cfgWindow.Clear( );
            outputControl.Clear( );
            SetModeEdition( true );
        }

        // --------------------------------------------------------------------

        private void BuildClick( object sender, EventArgs e )
        {
            CheckSimulation( );
        }

        // --------------------------------------------------------------------

        private int CheckSimulation( )
        {
            outputControl.Clear( );
            List<AirportSimuException> errors = new List<AirportSimuException>( );

            if ( cfgWindow.Packages.Count == 0 )
            {
                errors.Add( new AirportSimuException( "No package is detected",
                    "Create at least one to run the simulation.Configure in the panel's configuration" ) );

            }

            if ( drawingArea.Ways.Count == 0 )
            {
                errors.Add( new AirportSimuException( "No way is detected",
                    "Create at least one to run the simulation. Use the items panel " +
                    "to drag and drop components inside the main area and draw the way between these components" ) );
            }

            ServerConfig.Ip = cfgWindow.Ip;
            ServerConfig.Port = cfgWindow.Port;
            ServerConfig.EndPointAdress = cfgWindow.EndPointAdress;

            try
            {
                speed = ServerConfig.GetInstance( ).Services.GetConveyorSpeed( );
                interval = ServerConfig.GetInstance( ).Services.GetInterval( );
                Storage.capacity = ServerConfig.GetInstance( ).Services.GetStorageCapacity( );
                Package.interval = cfgWindow.Interval;
            }
            catch ( Exception ex )
            {
                errors.Add( new AirportSimuException( ex.Message,
                   "Check if the configuration are correctly filled or the server is not running" ) );
            }

            foreach ( AirportSimuException error in errors )
            {
                outputControl.AddException( error );
            }

            return errors.Count > 0
                   ? -1
                   : 0;
        }

        // --------------------------------------------------------------------

        private void OpenConfigurationWindow( object sender, RoutedEventArgs e )
        {
            cfgWindow.Ways = drawingArea.Ways;
            drawingArea.DisplayRankLabels( );
            cfgWindow.ShowDialog( );
        }

        // --------------------------------------------------------------------

        private void ConfigurationIsClosing( object sender, System.ComponentModel.CancelEventArgs e )
        {
            drawingArea.ResetSelection( );
        }

        // --------------------------------------------------------------------

        private void RunClick( object sender, EventArgs e )
        {
            if ( CheckSimulation( ) < 0 )
            {
                navigationBar.Status = Navigation.StateMode.Stop;
                return;
            }

            ResizeMode = System.Windows.ResizeMode.NoResize;
            RouteCalculation( );
            SetModeEdition( false );
        }

        // --------------------------------------------------------------------

        private void RouteCalculation( )
        {
            NameScope.SetNameScope( this, new NameScope( ) );

            int i = 0;
            double pixels = drawingArea.Ways[ 0 ].Zones[ 0 ].RenderSize.Height;
            List<Package> pkgs = UpdatePackages( cfgWindow.Packages );

            foreach ( Package pkg in pkgs )
            {
                for ( int j = 0; j < pkg.Luggages.Count; ++j )
                {
                    PointAnimationUsingKeyFrames keyFrames
                       = new PointAnimationUsingKeyFrames( );

                    EllipseGeometry ellipse
                        = new EllipseGeometry( pkg.Start.Center, 15, 15 );

                    RegisterName( "obj" + i, ellipse );

                    System.Windows.Shapes.Path aPath = new System.Windows.Shapes.Path( );
                    aPath.Data = ellipse;

                    VisualBrush brush = new VisualBrush( );
                    StackPanel stckPanel = new StackPanel( );
                    Label lblAmount = new Label( );

                    stckPanel.Background = cfgWindow.Packages.Contains( pkg )
                                              ? Brushes.Blue
                                              : Brushes.Green;

                    lblAmount.Content = ( j + 1 ) + "/" + pkg.Name;
                    lblAmount.Foreground = Brushes.White;
                    lblAmount.Margin = new Thickness( 2 );
                    stckPanel.Children.Add( lblAmount );
                    brush.Visual = stckPanel;

                    aPath.Fill = brush;
                    storyCanvas.Children.Add( aPath );

                    int start = pkgs.Where( tmp =>
                                            tmp.Start == pkg.Start ).Count( ) == 1
                                            ? j
                                            : i;

                    double length = 0;
                    double seconds = start * ( interval / ratio );

                    keyFrames.KeyFrames.Add(
                        new LinearPointKeyFrame(
                            pkg.Start.Center,
                            KeyTime.FromTimeSpan( TimeSpan.FromSeconds( seconds ) ) )
                    );

                    foreach ( Conveyor c in pkg.Way.Conveyors )
                    {
                        length += ( 2 * c.Distance ) / pixels;
                        seconds = length * ( 36 / ( speed * ratio * 10 ) )
                                  + ( start * ( interval / ratio ) );

                        keyFrames.KeyFrames.Add(
                            new LinearPointKeyFrame(
                                c.End.Center,
                                KeyTime.FromTimeSpan( TimeSpan.FromSeconds( seconds ) ) )
                        );
                    }

                    if ( pkg.Way.End.Storage.IsFull
                         && !pkg.Way.End.PlaneIsPresent )
                    {
                        ++i;
                        break;
                    }

                    if ( !pkg.Way.End.PlaneIsPresent )
                    {
                        pkg.Way.End.Storage.AddLuggage( new Luggage( pkg.Way.Start as CheckIn,
                                                                     pkg.Way.End ) );
                    }

                    Storyboard.SetTargetName( keyFrames, "obj" + i );
                    Storyboard.SetTargetProperty( keyFrames,
                                                  new PropertyPath( EllipseGeometry.CenterProperty ) );

                    keyFrames.CurrentStateInvalidated += new EventHandler( WayIsDone );

                    paraTimeline.Children.Add( keyFrames );

                    ++i;
                }
            }
        }

        // --------------------------------------------------------------------

        private List<Package> UpdatePackages( List<Package> packages )
        {
            List<Package> old_pkgs = new List<Package>( );
            List<Package> new_pkgs = new List<Package>( );
            List<Package> dir_pkgs = new List<Package>( );

            foreach ( Package tmp in packages )
            {
                if ( tmp.Way.Zones.OfType<Sorter>( ).Count( ) == 0 )
                {
                    tmp.AddLuggages( );
                    dir_pkgs.Add( tmp );
                    continue;
                }

                int end = tmp.Luggages.Capacity;

                for ( int i = 0; i < end; ++i )
                {
                    Package pkg = tmp;
                    List<Package> pkgs = new List<Package>( old_pkgs );
                    pkgs.AddRange( new_pkgs );

                    if ( pkg.IsOverloaded( i, interval )
                         && ( pkg = FindAlternative( pkgs, pkg, i ) ) == null )
                    {
                        continue;
                    }

                    pkg.AddLuggage( );

                    if ( packages.Contains( pkg )
                         && !old_pkgs.Contains( pkg ) )
                    {
                        old_pkgs.Add( pkg );
                    }
                    else if ( !packages.Contains( pkg ) )
                    {
                        new_pkgs.Add( pkg );
                    }
                }
            }

            new_pkgs.AddRange( dir_pkgs );
            old_pkgs.AddRange( new_pkgs );

            return old_pkgs;
        }

        // --------------------------------------------------------------------

        private Package FindAlternative( List<Package> packages, Package package, int index )
        {
            Package pkg = packages.Find( delegate( Package tmp )
            {
                return tmp.Way == package.Way
                       && !tmp.IsOverloaded( index, interval );
            }
            );

            return pkg == null
                   ? new Package( 1, package.Way )
                   : pkg;
        }

        // --------------------------------------------------------------------

        private void WayIsDone( object sender, EventArgs e )
        {
            string message = null;
            Clock clk = ( sender as AnimationClock );
            PointAnimationUsingKeyFrames frames = ( clk.Timeline
                                                   as PointAnimationUsingKeyFrames );

            if ( clk.CurrentTime == clk.NaturalDuration )
            {
                PointKeyFrameCollection col = frames.KeyFrames;

                Package package = cfgWindow.Packages.Find( delegate( Package pkg )
                {
                    return pkg.Start.Center == col[ 0 ].Value
                           && pkg.End.Center == col[ col.Count - 1 ].Value;
                }
                );

                if ( package == null )
                {
                    message = "Package not found corresponding to the incoming luggage";
                    outputControl.AddException( new AirportSimuException( message,
                        "Contact the administrator" ) );

                    ServerConfig.GetInstance( ).Services.UpdateHandlingReport(
                        new HandlingEntry( DateTime.Now, message ) );

                    return;
                }

                if ( !package.End.PlaneIsPresent
                     && !package.End.Storage.IsFull )
                {
                    package.End.Storage.AddLuggage( new Luggage( package.Start,
                                                                 package.End ) );
                    message = String.Format( "One luggage from {0} to {1} has " +
                                             "been dropped in the storage {2} waiting the plane",
                                             package.Start.Name,
                                             package.End.Name,
                                             package.End.Storage.Name );

                    outputControl.AddMessage( message );
                }
                else if ( package.End.PlaneIsPresent )
                {
                    message = String.Format( "One luggage from {0} to {1} " +
                                              "reached his destination",
                                              package.Start.Name,
                                              package.End.Name );

                    outputControl.AddMessage( message );
                }

                ServerConfig.GetInstance( ).Services.UpdateHandlingReport(
                        new HandlingEntry( DateTime.Now, message ) );

                if ( package.End.Storage.IsFull )
                {
                    message = String.Format( "Storage {0} is full",
                        package.End.Storage.Name );

                    outputControl.AddException( new AirportSimuException( message, "Empty it!" ) );

                    ServerConfig.GetInstance( ).Services.UpdateHandlingReport(
                        new HandlingEntry( DateTime.Now, message ) );
                }
            }
        }

        // --------------------------------------------------------------------

        private void ClearStorages( )
        {
            foreach ( Package pkg in cfgWindow.Packages )
            {
                pkg.Way.End.Storage.Luggages.Clear( );
            }
        }

        // --------------------------------------------------------------------

        private void ClearPackages( )
        {
            foreach ( Package pkg in cfgWindow.Packages )
            {
                pkg.ReInitialiaze( );
            }
        }

        // --------------------------------------------------------------------

        private void StopClick( object sender, EventArgs e )
        {
            SetModeEdition( true );
        }

        // --------------------------------------------------------------------

        private void ResumeClick( object sender, EventArgs e )
        {
            drawingArea.DisplayRankLabels( );
            storyBoard.Resume( storyCanvas );
        }

        // --------------------------------------------------------------------

        private void PauseClick( object sender, EventArgs e )
        {
            drawingArea.ResetSelection( );
            storyBoard.Pause( storyCanvas );
        }

        // --------------------------------------------------------------------

        private void SimulationIsDone( object sender, EventArgs e )
        {
            SetModeEdition( true );
            ServerConfig.GetInstance( ).Services.SaveHandlingReport( );
        }

        // --------------------------------------------------------------------

        private void PackagesModified( object sender, EventArgs e )
        {
            fileModified = true;
        }

        // --------------------------------------------------------------------

        private void ClickOnAirportZone( AirportZone subject, MouseButtonEventArgs e )
        {
            if ( !subject.IsBuilt )
            {
                return;
            }

            AirportZonePropertyFactory factory = AirportZonePropertyFactory.GetFactory( subject );
            gridProperties.SelectedZone = factory.CreateProperties( subject,
                                                                    cfgWindow.Packages,
                                                                    drawingArea.Ways );
        }

        // --------------------------------------------------------------------

        private void SetModeEdition( bool editionMode )
        {
            if ( editionMode )
            {
                storyBoard.Stop( storyCanvas );
                drawingArea.ResetSelection( );
                paraTimeline.Children.Clear( );
                storyCanvas.Children.Clear( );
                txtStatusMode.Text = "Edition Mode";
                ResizeMode = System.Windows.ResizeMode.CanResize;
                navigationBar.Status = Navigation.StateMode.Stop;
            }
            else
            {
                ratio = Convert.ToDouble( txtRatio.Text );
                drawingArea.DisplayRankLabels( );
                outputControl.Clear( );
                txtStatusMode.Text = "Simulation Mode";
                storyBoard.Begin( storyCanvas, true );
            }

            ClearPackages( );
            drawingArea.IsEnabled = editionMode;
            itemsBox.IsEnabled = editionMode;
        }

        // --------------------------------------------------------------------

        private void DecreaseRatio( object sender, RoutedEventArgs e )
        {
            if ( ( ratio = Convert.ToDouble( txtRatio.Text ) ) >= 0.5 )
            {
                txtRatio.Text = ( ratio - 0.5 ).ToString( );
            }
        }

        // --------------------------------------------------------------------

        private void IncreaseRatio( object sender, RoutedEventArgs e )
        {
            ratio = Convert.ToDouble( txtRatio.Text ) + 0.5;
            txtRatio.Text = ratio.ToString( );
        }

        // --------------------------------------------------------------------

        private void ClbkWayIsCreated( object sender, Way way )
        {
            List<Package> pkgs = cfgWindow.Packages.FindAll( delegate( Package pkg )
            {
                return pkg.Start == way.Start
                       && pkg.End == way.End;
            }
            );

            if ( pkgs.Count == 0 )
            {
                return;
            }

            List<Way> tmp = new List<Way>( );

            foreach ( Package pkg in pkgs )
            {
                tmp.Add( pkg.Way );
            }

            tmp.Add( way );

            Way w = Ways.ShortestWay( tmp );

            if ( w != null )
            {
                foreach ( Package pkg in pkgs )
                {
                    pkg.Way = w;
                }
            }
        }

        // --------------------------------------------------------------------

        private void ClbkWayIsDeleted( object sender, Way way )
        {
            List<Package> pkgs = cfgWindow.Packages.FindAll( delegate( Package tmp )
            {
                return tmp.Way == way;
            }
            );

            foreach ( Package pkg in pkgs )
            {
                cfgWindow.Packages.Remove( pkg );
            }
        }

        // --------------------------------------------------------------------

        private void PrintSimulation( object sender, RoutedEventArgs e )
        {
            PrintDialog dialog = new PrintDialog( );

            if ( dialog.ShowDialog( ) == true )
            {
                dialog.PrintVisual( drawingArea, Path.GetFileName( currentFile ) );
            }
        }

        // --------------------------------------------------------------------

        private void OpenHelp( object sender, RoutedEventArgs e )
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process( );
            proc.StartInfo.FileName = "..\\..\\Resources\\user_manual.pdf";
            proc.StartInfo.Verb = "Open";
            proc.StartInfo.CreateNoWindow = true;
            proc.Start( );
        }

        // --------------------------------------------------------------------

        private bool fileSaved;
        private bool fileModified;
        private string currentFile;
        private double ratio;
        private double speed;
        private double interval;

        private FileSystemWatcher fileSystemWatcher;
        private Configuration cfgWindow;
        private Storyboard storyBoard;
        private ParallelTimeline paraTimeline;
    }
}
