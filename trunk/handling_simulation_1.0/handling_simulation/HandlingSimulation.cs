using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Airport.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Airport.Utils;

namespace Nth.Eindhoven.Fontys
{
    public partial class HandlingSimulation : Form
    {
        public HandlingSimulation( )
        {
            InitializeComponent( );

            airportArea.AirportZoneIsSelected +=
                new Airport.Forms.AirportArea.AirportHandler( AirportZoneIsSelected );

            airportArea.DropNewAirportZone +=
                new Airport.Forms.AirportArea.AirportHandler( DropNewAirportZone );

            airportArea.AirportZoneIsNotSelected +=
                new AirportArea.AirportHandler( AirportZoneIsNotSelected );

            airportArea.ConveyorAngleChanged +=
                new AirportArea.ConveyorHandler( ConveyorAngleChanged );

            componentsBar.ClickedOnConvoyer +=
                new EventHandler( ClickedOnPlotLine );
        }

        // --------------------------------------------------------------------

        private void ConveyorAngleChanged( Conveyor subject, EventArgs e )
        {
            lblAngle.Text = subject.Angle( ).ToString( "#.##" );
        }

        // --------------------------------------------------------------------

        private void ClickedOnPlotLine( object sender, EventArgs e )
        {
            airportArea.Plotting = ( sender as CheckBox ).Checked;
            Cursor = ( airportArea.Plotting )
                        ? Cursors.Cross
                        : Cursors.Default;
        }

        // --------------------------------------------------------------------

        private void DropNewAirportZone( AirportZone subject, EventArgs e )
        {
            if ( !fileModified )
            {
                Text += "*";
            }

            fileModified = true;
        }

        // --------------------------------------------------------------------

        private void AirportZoneIsSelected( AirportZone sender, EventArgs e )
        {
            btnDelete.Enabled = true;
            selectedZone = sender;
        }

        // --------------------------------------------------------------------

        private void AirportZoneIsNotSelected( AirportZone subject, EventArgs e )
        {
            btnDelete.Enabled = false;
            selectedZone = null;
        }

        // --------------------------------------------------------------------

        private void OpenFile( object sender, EventArgs e )
        {
            OpenFileDialog openFileDialog = new OpenFileDialog( );
            openFileDialog.InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.Personal );
            openFileDialog.Filter = "Simulation Files (*.hsm)|*.hsm";
            if ( openFileDialog.ShowDialog( this ) == DialogResult.OK )
            {
                List<AirportZone> zones = new List<AirportZone>( );
                Deserialize( openFileDialog.FileName, ref zones );
                airportArea.PopulateTable( zones );
                InitializeFileSystemWatcher( openFileDialog.FileName );
                Text = Text.Split( '-' )[ 0 ] + "- " + openFileDialog.FileName;
            }
        }

        // --------------------------------------------------------------------

        private void SaveAsDocument( object sender, EventArgs e )
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog( );
            saveFileDialog.InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.Personal );
            saveFileDialog.Filter = "Simulation Files (*.hsm)|*.hsm";
            if ( saveFileDialog.ShowDialog( this ) == DialogResult.OK )
            {
                try
                {
                    InitializeFileSystemWatcher( saveFileDialog.FileName );
                    Serialize( saveFileDialog.FileName );
                }
                catch ( Exception ex )
                {
                    MessageBox.Show( ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }
            }
        }

        // --------------------------------------------------------------------

        private void Exit( object sender, EventArgs e )
        {
            Close( );
        }

        // --------------------------------------------------------------------

        private void ToolBarToolClick( object sender, EventArgs e )
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        // --------------------------------------------------------------------

        private void StatusBarToolClick( object sender, EventArgs e )
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        // --------------------------------------------------------------------

        private void FormResize( object sender, EventArgs e )
        {
            Airport.Utils.Monitor.Instance( ).AllowRefresh = false;
        }

        // --------------------------------------------------------------------

        private void FormResizeEnd( object sender, EventArgs e )
        {
            Airport.Utils.Monitor.Instance( ).AllowRefresh = true;
            airportArea.ResizeTable( );
        }

        // --------------------------------------------------------------------

        private void SimulationClosing( object sender, FormClosingEventArgs e )
        {
            System.Windows.Forms.DialogResult ret = System.Windows.Forms.DialogResult.OK;

            if ( fileModified
                && ( ret = MessageBox.Show( "The file was updated, do you want to save the modifications?",
                                    Name,
                                    MessageBoxButtons.YesNoCancel,
                                    MessageBoxIcon.Exclamation ) )
                                    == System.Windows.Forms.DialogResult.OK )
            {
                SaveDocument( sender, e );
            }
            else if ( ret == System.Windows.Forms.DialogResult.Cancel )
            {
                e.Cancel = true;
            }
        }

        // --------------------------------------------------------------------

        protected override void WndProc( ref Message m )
        {
            base.WndProc( ref m );

            if ( m.Msg == 161 // WM_NCLBUTTONDOWN
                 && (int)m.WParam == 9 ) // HTMAXBUTTON 
            {
                Airport.Utils.Monitor.Instance( ).AllowRefresh = true;
                airportArea.ResizeTable( );
            }
        }

        // --------------------------------------------------------------------

        private void NewDocument( object sender, EventArgs e )
        {
            if ( !fileModified
                && !fileSaved )
            {
                return;
            }

            System.Windows.Forms.DialogResult ret =
                System.Windows.Forms.DialogResult.OK;

            if ( fileModified
                 && ( ret = MessageBox.Show( "The file was updated, do you want to save the modificatons?",
                                    Name,
                                    MessageBoxButtons.YesNoCancel,
                                    MessageBoxIcon.Exclamation ) )
                                    == System.Windows.Forms.DialogResult.OK )
            {
                SaveDocument( sender, e );
            }
            else if ( ret == System.Windows.Forms.DialogResult.Cancel )
            {
                return;
            }

            if ( fileModified
                && !fileSaved
                && ( ret = MessageBox.Show( "Do you want to save changes you made",
                                    Name,
                                    MessageBoxButtons.YesNoCancel,
                                    MessageBoxIcon.Exclamation ) )
                                    == System.Windows.Forms.DialogResult.OK )
            {
                SaveAsDocument( sender, e );
                return;
            }
            else if ( ret == System.Windows.Forms.DialogResult.Cancel )
            {
                return;
            }

            airportArea.Dispose( );
            airportArea = new AirportArea( );
            airportArea.Dock = DockStyle.Fill;
            grpSimulationArea.Controls.Add( airportArea );
            InitializeAttributes( );
        }

        // --------------------------------------------------------------------

        private int Serialize( string filename )
        {
            List<ControlNode> nodes = new List<ControlNode>( airportArea.Zones.Count );
            FileStream fs = null;
            int error = -1;

            foreach ( AirportZone zone in airportArea.Zones )
            {
                if ( zone.IsMarked )
                {
                    nodes.Add( new ControlNode( zone.Position, zone.BackgroundImage ) );
                }
            }

            try
            {
                fs = new FileStream( filename, FileMode.Create );
                BinaryFormatter formatter = new BinaryFormatter( );
                formatter.Serialize( fs, nodes );
            }
            catch ( Exception ex )
            {
                throw ex;
            }
            finally
            {
                if ( fs != null )
                {
                    fs.Close( );
                    error = 0;
                }
            }

            return error;
        }

        // --------------------------------------------------------------------

        private int Deserialize( string filename, ref List<AirportZone> zones )
        {
            if ( zones == null )
            {
                return -1;
            }

            List<ControlNode> nodes = new List<ControlNode>( );
            FileStream fs = null;
            int error = -1;

            try
            {
                fs = new FileStream( filename, FileMode.Open );
                BinaryFormatter formatter = new BinaryFormatter( );
                nodes = (List<ControlNode>)formatter.Deserialize( fs );
            }
            finally
            {
                if ( fs != null )
                {
                    fs.Close( );
                    error = 0;
                }
            }

            if ( error < 0 )
            {
                return -1;
            }

            foreach ( ControlNode node in nodes )
            {
                AirportZone ctrl = new AirportZone( node.Position );
                ctrl.BackgroundImage = node.BackGroundImage;
                ctrl.IsMarked = node.IsMarked;
                zones.Add( ctrl );
            }

            return error;
        }

        // --------------------------------------------------------------------

        private void SaveDocument( object sender, EventArgs e )
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
                    MessageBox.Show( ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }
            }
            else
            {
                SaveAsDocument( sender, e );
            }
        }

        // --------------------------------------------------------------------

        private void InitializeFileSystemWatcher( string filename )
        {
            fileSystemWatcher.Path = Path.GetDirectoryName( filename );
            fileSystemWatcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
            fileSystemWatcher.EnableRaisingEvents = true;
            currentFile = filename;
            fileSaved = true;
        }

        // --------------------------------------------------------------------

        private void FileChanged( object sender, FileSystemEventArgs e )
        {
            if ( e.FullPath == currentFile )
            {
                if ( Text.Contains( '*' ) )
                {
                    Text.Remove( Text.LastIndexOf( '*' ) );
                }

                Text = Text.Split( '-' )[ 0 ] + "- " + e.FullPath;
                fileModified = false;
                fileSaved = true;
            }
        }

        // --------------------------------------------------------------------

        private void FileCreated( object sender, FileSystemEventArgs e )
        {
            if ( e.FullPath == currentFile )
            {
                Text = Text.Split( '-' )[ 0 ] + "- " + e.FullPath;
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
                Text = Text.Split( '-' )[ 0 ] + "- " + e.FullPath;
            }
        }

        // --------------------------------------------------------------------

        private void InitializeAttributes( )
        {
            currentFile = String.Empty;
            fileSaved = false;
            fileModified = false;
            Text = "Handling Simulation 1.0 - New Document";
        }

        // --------------------------------------------------------------------

        private void DeleteSelectedZones( object sender, EventArgs e )
        {
            selectedZone.BackgroundImage = null;
            selectedZone.IsMarked = false;
            airportArea.Zones.Remove( selectedZone );
        }

        // --------------------------------------------------------------------

        private bool fileSaved;
        private bool fileModified;
        private string currentFile;
        private AirportZone selectedZone;
    }
}
