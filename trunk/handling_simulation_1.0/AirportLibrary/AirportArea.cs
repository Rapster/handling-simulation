using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Airport.Utils;

namespace Airport.Forms
{
    public partial class AirportArea : Panel
    {
        public AirportArea( )
        {
            InitializeComponent( );
            CreateTable( );
            paintPanel.BringToFront( );
            mouseWasDown = false;
        }

        // --------------------------------------------------------------------

        public void ResizeTable( )
        {
            // Review
            Controls.Clear( );
            Size size = new Size( (int)Math.Round( (double)Width / nbColumns ),
                                  (int)Math.Round( (double)Height / nbRows ) );

            foreach ( AirportZone zone in zones )
            {
                zone.Size = size;
                zone.Location = new Point( zone.Position.X * zone.Size.Width,
                                           zone.Position.Y * zone.Size.Height );
                Controls.Add( zone );
                Controls.SetChildIndex( zone, 0 );
            }

            Controls.Add( paintPanel );
            paintPanel.BringToFront( );
        }

        // --------------------------------------------------------------------

        private void CreateTable( )
        {
            Size size = new Size( (int)Math.Round( (double)Width / nbColumns ),
                                  (int)Math.Round( (double)Height / nbRows ) );

            zones = new List<AirportZone>( nbRows * nbColumns );

            for ( int j = 0; j < nbRows; ++j )
            {
                for ( int i = 0; i < nbColumns; ++i )
                {
                    AirportZone zone = new AirportZone( new Point( i, j ) );
                    zone.Size = size;
                    zone.Location = new Point( i * size.Width,
                                               j * size.Height );
                    SetEventsToZone( zone );
                    zones.Add( zone );
                    Controls.Add( zone );
                    Controls.SetChildIndex( zone, 0 );
                }
            }
        }

        // --------------------------------------------------------------------

        public void PopulateTable( List<AirportZone> zones )
        {
            foreach ( AirportZone zone in zones )
            {
                int index = ( nbColumns * zone.Position.Y + zone.Position.X );
                SetEventsToZone( this.zones[ index ] = zone );
            }

            ResizeTable( );
        }

        // --------------------------------------------------------------------

        public List<AirportZone> Zones
        {
            get { return zones; }
        }

        // --------------------------------------------------------------------

        public AirportZone FindAirportZone( Point location )
        {
            foreach ( AirportZone zone in zones )
            {
                if ( zone.IsMarked
                    && location.X >= zone.Location.X
                    && location.X <= zone.Location.X + zone.Width
                    && location.Y >= zone.Location.Y
                    && location.Y <= zone.Location.Y + zone.Height )
                {
                    return zone;
                }
            }

            return null;
        }

        // --------------------------------------------------------------------

        public bool Plotting
        {
            get { return paintPanel.Enabled; }
            set { paintPanel.Enabled = value; }
        }

        // --------------------------------------------------------------------

        private void AirportLostFocus( object sender, EventArgs e )
        {
            if ( AirportZoneLostFocus != null
                && ( sender as AirportZone ).IsMarked )
            {
                AirportZoneIsSelected( sender as AirportZone, null );
            }
        }

        // --------------------------------------------------------------------

        private void AirportZoneClick( object sender, EventArgs e )
        {
            if ( AirportZoneIsSelected != null
                && ( sender as AirportZone ).IsMarked )
            {
                AirportZoneIsSelected( sender as AirportZone, e );
            }
            else if ( AirportZoneIsNotSelected != null )
            {
                AirportZoneIsNotSelected( sender as AirportZone, e );
            }
        }

        // --------------------------------------------------------------------

        private void AirportZoneMouseMove( object sender, MouseEventArgs e )
        {
            if ( mouseWasDown
                 && !Plotting
                 && ( sender as AirportZone ).IsMarked )
            {
                lzone = sender as AirportZone;
                lzone.DoDragDrop( lzone.BackgroundImage, DragDropEffects.Move );
            }

            mouseWasDown = false;

        }

        // --------------------------------------------------------------------

        private void PaintPanelMouseMove( object sender, MouseEventArgs e )
        {
            if ( paintPanel.Draw )
            {
                paintPanel.ZoneEnd.Location = e.Location;
                Refresh( );
            }

            if ( ConveyorAngleChanged != null )
            {
                ConveyorAngleChanged( new Conveyor( paintPanel.ZoneStart,
                                                    paintPanel.ZoneEnd ), e );
            }
        }

        // --------------------------------------------------------------------

        private void AirportZoneMouseDown( object sender, MouseEventArgs e )
        {
            if ( !Plotting
                && ( sender as AirportZone ).IsMarked )
            {
                Cursor.Current = Cursors.Hand;
                mouseWasDown = true;
            }
        }

        // --------------------------------------------------------------------

        private void PaintPanelMouseDown( object sender, MouseEventArgs e )
        {
            if ( ( paintPanel.ZoneStart = FindAirportZone( e.Location ) ) != null )
            {
                paintPanel.ZoneStart = paintPanel.ZoneStart.Clone( );
                paintPanel.Draw = true;
            }
        }

        // --------------------------------------------------------------------

        public void AirportZoneDragEnter( object sender, DragEventArgs e )
        {
            if ( !Plotting
                 && !( sender as AirportZone ).IsMarked )
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        // --------------------------------------------------------------------

        public void AirportZoneDragDrop( object rzone, DragEventArgs e )
        {
            AirportZone tmp = ( rzone as AirportZone );
            int index = ( nbColumns * tmp.Position.Y + tmp.Position.X );

            if ( index >= zones.Count )
            {
                throw new IndexOutOfRangeException( );
            }

            tmp.BackgroundImage = e.Data.GetData( DataFormats.Bitmap ) as Image;
            AirportZone result = zones.Find( delegate( AirportZone zone )
            {
                return ( lzone != null )
                    ? zone.Id == lzone.Id
                    : false;
            }
            );

            if ( result != null )
            {
                result.BackgroundImage = null;
                result.IsMarked = false;
            }

            tmp.IsMarked = true;
            SetEventsToZone( zones[ index ] = tmp );
            lzone = null;

            if ( DropNewAirportZone != null )
            {
                DropNewAirportZone( tmp, e );
            }
        }

        // --------------------------------------------------------------------

        public List<Conveyor> ShortestWay( CheckIn checkIn, DestinationGate destination )
        {
            throw new System.NotImplementedException( );
        }

        // --------------------------------------------------------------------

        private void PaintPanelMouseUp( object sender, MouseEventArgs e )
        {
            paintPanel.Draw = false;

            if ( ( paintPanel.ZoneEnd = FindAirportZone( e.Location ) ) != null
                   && paintPanel.ZoneEnd.IsMarked )
            {
                paintPanel.ZoneEnd = paintPanel.ZoneEnd.Clone( );
                Conveyor conveyor = new Conveyor( paintPanel.ZoneStart.Clone( ),
                                                  paintPanel.ZoneEnd.Clone( ) );
                if ( conveyor.IsValid )
                {
                    paintPanel.AddConveyor( conveyor );
                }

                Refresh( );
            }
        }

        // --------------------------------------------------------------------

        private void SetEventsToZone( AirportZone zone )
        {
            zone.DragDrop += new DragEventHandler( AirportZoneDragDrop );
            zone.DragEnter += new DragEventHandler( AirportZoneDragEnter );
            zone.MouseDown += new MouseEventHandler( AirportZoneMouseDown );
            zone.MouseMove += new MouseEventHandler( AirportZoneMouseMove );
            zone.Click += new EventHandler( AirportZoneClick );
        }

        // --------------------------------------------------------------------

        public delegate void AirportHandler( AirportZone subject, EventArgs e );
        public delegate void ConveyorHandler( Conveyor subject, EventArgs e );
        public event AirportHandler AirportZoneIsSelected;
        public event AirportHandler AirportZoneIsNotSelected;
        public event AirportHandler AirportZoneLostFocus;
        public event AirportHandler DropNewAirportZone;
        public event ConveyorHandler ConveyorAngleChanged;

        // --------------------------------------------------------------------

        private List<AirportZone> zones;
        private int nbColumns = 15;
        private int nbRows = 15;
        private bool mouseWasDown;
        private AirportZone lzone;
    }
}
