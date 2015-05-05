using System;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Input;
using Airport.Utils;
using Airport.Tools;
using Airport.Utils.Factories;

namespace Airport.Controls
{
    /// <summary>
    /// Container parent where airport zones can be drag displaying as a grid.
    /// </summary>
    public class AirportArea : UniformGrid
    {
        /// <summary>
        /// Initialize a new instance of a AirportArea
        /// </summary>
        /// <param name="rows">Number of rows to create inside the area</param>
        /// <param name="columns">Number of columns to create inside the area</param>
        public AirportArea( int rows, int columns )
            : base( )
        {
            CreateTable( rows, columns );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Initialize a new instance of a AirportArea
        /// </summary>
        public AirportArea( )
            : this( 15, 15 )
        {
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Populates the entire table with empty AirportZone objects.
        /// </summary>
        /// <param name="rows">Number of rows to create inside the area</param>
        /// <param name="columns">Number of columns to create inside the area</param>
        public void CreateTable( int rows = 15, int columns = 15 )
        {
            Rows = rows;
            Columns = columns;

            for ( int j = 0; j < Rows; ++j )
            {
                for ( int i = 0; i < Columns; ++i )
                {
                    AirportZone zone = new AirportZone( this,
                                                        new Index( i, j ),
                                                        String.Empty );
                    Children.Add( zone );
                    SetEventsToZone( zone );
                }
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Remove the first occurrence of the specified object
        /// </summary>
        /// <param name="zone">Object to remove</param>
        public void Remove( AirportZone zone )
        {
            UnSetEventsFromZone( zone );

            Insert( new AirportZone( this,
                                     zone.Position,
                                     String.Empty ) );

            SetEventsToZone( Children[ zone.Index ] as AirportZone );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Removes all children
        /// </summary>
        public void Clear( )
        {
            Children.Clear( );
            CreateTable( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Populates the Children collection using the entries within a List of AirportZone objects
        /// </summary>
        /// <param name="zones"></param>
        public void FillTable( List<AirportZone> zones )
        {
            foreach ( AirportZone zone in zones )
            {
                Insert( zone );
            }

            UpdateLayout( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the children as a list of AirportZone
        /// </summary>
        public List<AirportZone> Zones
        {
            get
            {
                List<AirportZone> tmp = new List<AirportZone>( Children.Count );

                foreach ( AirportZone zone in Children )
                {
                    tmp.Add( zone );
                }

                return tmp;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the zone located to the left of the specified object.
        /// </summary>
        /// <param name="zone">Specified AirportZone</param>
        /// <returns>An AirportZone if the found object is valid otherwise null</returns>
        public AirportZone LeftZone( AirportZone zone )
        {
            int index = ( Columns * zone.Position.Y + zone.Position.X - 1 );
            return ( index < 0 )
                     ? null
                     : Children[ index ] as AirportZone;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the zone located to the right of the specified object.
        /// </summary>
        /// <param name="zone">Specified AirportZone</param>
        /// <returns>An AirportZone if the found object is valid otherwise null</returns>
        public AirportZone RightZone( AirportZone zone )
        {
            int index = ( Columns * zone.Position.Y + zone.Position.X + 1 );
            return ( index < Children.Count )
                     ? Children[ index ] as AirportZone
                     : null;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the zone located above of the specified object.
        /// </summary>
        /// <param name="zone">Specified AirportZone</param>
        /// <returns>An AirportZone if the found object is valid otherwise null</returns>
        public AirportZone AboveZone( AirportZone zone )
        {
            int index = ( Columns * ( zone.Position.Y - 1 ) ) + zone.Position.X;
            return ( index < 0 )
                     ? null
                     : Children[ index ] as AirportZone;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the zone located below of the specified object.
        /// </summary>
        /// <param name="zone">Specified AirportZone</param>
        /// <returns>An AirportZone if the found object is valid otherwise null</returns>
        public AirportZone BelowZone( AirportZone zone )
        {
            int index = ( Columns * ( zone.Position.Y + 1 ) ) + zone.Position.X;
            return ( index < Children.Count )
                     ? Children[ index ] as AirportZone
                     : null;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the zone located to the left corner above of the specified object.
        /// </summary>
        /// <param name="zone">Specified AirportZone</param>
        /// <returns>An AirportZone if the found object is valid otherwise null</returns>
        public AirportZone LeftAboveZone( AirportZone zone )
        {
            int index = ( Columns * ( zone.Position.Y - 1 ) )
                        + zone.Position.X - 1;

            return ( index < 0 )
                     ? null
                     : Children[ index ] as AirportZone;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the zone located to the right corner above of the specified object.
        /// </summary>
        /// <param name="zone">Specified AirportZone</param>
        /// <returns>An AirportZone if the found object is valid otherwise null</returns>
        public AirportZone RightAboveZone( AirportZone zone )
        {
            int index = ( Columns * ( zone.Position.Y - 1 ) )
                          + zone.Position.X + 1;

            return ( index < 0 )
                     ? null
                     : Children[ index ] as AirportZone;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the zone located to the left corner below of the specified object.
        /// </summary>
        /// <param name="zone">Specified AirportZone</param>
        /// <returns>An AirportZone if the found object is valid otherwise null</returns>
        public AirportZone LeftBelowZone( AirportZone zone )
        {
            int index = ( Columns * ( zone.Position.Y + 1 ) )
                          + zone.Position.X - 1;

            return ( index < Children.Count )
                     ? Children[ index ] as AirportZone
                     : null;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the zone located to the right corner below of the specified object.
        /// </summary>
        /// <param name="zone">Specified AirportZone</param>
        /// <returns>An AirportZone if the found object is valid otherwise null</returns>
        public AirportZone RightBelowZone( AirportZone zone )
        {
            int index = ( Columns * ( zone.Position.Y + 1 ) )
                          + zone.Position.X + 1;

            return ( index < Children.Count )
                     ? Children[ index ] as AirportZone
                     : null;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Returns the closest zone of the specified object
        /// </summary>
        /// <param name="zone">Specified object</param>
        /// <returns>Closest AirportZone if it exits otherwise null</returns>
        public AirportZone ClosestZone( AirportZone zone )
        {
            AirportZone tmp = null;

            return zone != null
                   && ( ( ( tmp = LeftZone( zone ) ) != null )
                   || ( ( tmp = LeftAboveZone( zone ) ) != null )
                   || ( ( tmp = AboveZone( zone ) ) != null )
                   || ( ( tmp = RightAboveZone( zone ) ) != null )
                   || ( ( tmp = RightZone( zone ) ) != null )
                   || ( ( tmp = RightBelowZone( zone ) ) != null )
                   || ( ( tmp = BelowZone( zone ) ) != null )
                   || ( ( tmp = LeftBelowZone( zone ) ) != null ) )
                   ? tmp
                   : null;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Returns the closest zone of the specified object and according to a position
        /// </summary>
        /// <param name="zone">Specified object</param>
        /// <param name="position">Specified positino</param>
        /// <returns>Closest AirportZone if it exits otherwise null</returns>
        public AirportZone ClosestZone( AirportZone zone, Point position )
        {
            AirportZone tmp = null;

            return zone != null
                   && ( ( ( tmp = LeftZone( zone ) ) != null
                   && tmp.BelongsTo( position ) )
                   || ( ( tmp = LeftAboveZone( zone ) ) != null
                   && tmp.BelongsTo( position ) )
                   || ( ( tmp = AboveZone( zone ) ) != null
                   && tmp.BelongsTo( position ) )
                   || ( ( tmp = RightAboveZone( zone ) ) != null
                   && tmp.BelongsTo( position ) )
                   || ( ( tmp = RightZone( zone ) ) != null
                   && tmp.BelongsTo( position ) )
                   || ( ( tmp = RightBelowZone( zone ) ) != null
                   && tmp.BelongsTo( position ) )
                   || ( ( tmp = BelowZone( zone ) ) != null
                   && tmp.BelongsTo( position ) )
                   || ( ( tmp = LeftBelowZone( zone ) ) != null
                   && tmp.BelongsTo( position ) ) )
                   ? tmp
                   : null;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Find the correspond AirportZone contained in the Area according to a position
        /// </summary>
        /// <param name="location">Specified position</param>
        /// <returns>The corresponding AirportZone if it exists, otherwise null</returns>
        public AirportZone FindBy( Point location )
        {
            foreach ( AirportZone zone in Zones )
            {
                if ( zone.BelongsTo( location ) )
                {
                    return zone;
                }
            }

            return null;
        }

        // --------------------------------------------------------------------
        
        /// <summary>
        /// Links up an already-created AirportZone object to the Children collection.
        /// Removes the previous zone that occupied the same index.
        /// </summary>
        /// <param name="zone">Object to insert in the area</param>
        private void Insert( AirportZone zone )
        {
            Children.RemoveAt( zone.Index );
            SetEventsToZone( zone );
            Children.Insert( zone.Index, zone );
        }

        // --------------------------------------------------------------------
        
        /// <summary>
        /// Hooks event handlers to Mouse events (DragEnter, Drop, MouseDown/Up/Enter/Leave)
        /// </summary>
        /// <param name="zone">Specific AirportZone to set up</param>
        private void SetEventsToZone( AirportZone zone )
        {
            zone.DragEnter += new System.Windows.DragEventHandler( ZoneMouseDragEnter );
            zone.Drop += new System.Windows.DragEventHandler( ZoneMouseDrop );
            zone.MouseDown += new System.Windows.Input.MouseButtonEventHandler( ZoneMouseDown );
            zone.MouseMove += new MouseEventHandler( ZoneMouseMove );
            zone.MouseUp += new MouseButtonEventHandler( ZoneMouseUp );
            zone.MouseEnter += new MouseEventHandler( ZoneMouseEnter );
            zone.MouseLeave += new MouseEventHandler( ZoneMouseLeave );
        }

        // --------------------------------------------------------------------
        
        /// <summary>
        /// Modify the cursor's state into the default Arrow 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event type</param>
        private void ZoneMouseLeave( object sender, MouseEventArgs e )
        {
            Cursor = Cursors.Arrow;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Modify the cursor's state into the Hand cursor if the hover zone is built
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event type</param>
        private void ZoneMouseEnter( object sender, MouseEventArgs e )
        {
            AirportZone zone = ( sender as AirportZone );
            if ( zone.IsBuilt )
            {
                Cursor = Cursors.Hand;
                zone.ToolTip = zone.Name;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Event handler at the end of a Mouse Click sequence.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event type</param>
        private void ZoneMouseUp( object sender, MouseButtonEventArgs e )
        {
            if ( mouseWasDown
                && e.LeftButton == MouseButtonState.Released
                && ClickOnAirportZone != null )
            {
                ClickOnAirportZone( sender as AirportZone, e );
            }

            mouseWasDown = false;
            Cursor = Cursors.Arrow;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Unsuscribe events attached to the specified AirportZone
        /// </summary>
        /// <param name="zone">Specified object</param>
        private void UnSetEventsFromZone( AirportZone zone )
        {
            zone.DragEnter -= new System.Windows.DragEventHandler( ZoneMouseDragEnter );
            zone.Drop -= new System.Windows.DragEventHandler( ZoneMouseDrop );
            zone.MouseDown -= new System.Windows.Input.MouseButtonEventHandler( ZoneMouseDown );
            zone.MouseMove -= new MouseEventHandler( ZoneMouseMove );
            zone.MouseUp -= new MouseButtonEventHandler( ZoneMouseUp );
            zone.MouseEnter -= new MouseEventHandler( ZoneMouseEnter );
            zone.MouseLeave -= new MouseEventHandler( ZoneMouseLeave );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Drag an AirportZone
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event type</param>
        private void ZoneMouseMove( object sender, MouseEventArgs e )
        {
            if ( ( sender as AirportZone ).IsBuilt
                  && e.LeftButton == MouseButtonState.Pressed )
            {
                lzone = sender as AirportZone;
                DataObject data = new DataObject( lzone.GetType( ), lzone );
                DragDrop.DoDragDrop( this, data, DragDropEffects.Move );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Change the current cursor into the Hand cursor in order to drag it
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event type</param>
        private void ZoneMouseDown( object sender, System.Windows.Input.MouseButtonEventArgs e )
        {
            if ( ( sender as AirportZone ).IsBuilt
                 && e.LeftButton == MouseButtonState.Pressed )
            {
                Cursor = Cursors.Hand;
            }

            mouseWasDown = true;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Drop the dragged AirportZone
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event type</param>
        private void ZoneMouseDrop( object sender, System.Windows.DragEventArgs e )
        {
            Type type = null;
            if ( ( type = GetTypeAirportZone( e ) ) != null )
            {
                AirportZone tmp = ( sender as AirportZone );
                tmp.Background =
                    ( e.Data.GetData( type ) as AirportZone ).Background;

                AirportFactory factory = AirportFactory.GetFactory( type );
                tmp = factory.CreateZone( this, tmp.Position, tmp.Background, true );
                Insert( tmp );

                AirportZone result = Zones.Find( delegate( AirportZone zone )
                {
                    return zone.Equals( lzone )
                           && lzone.IsBuilt;
                }
                );

                if ( result != null )
                {
                    Remove( result );
                }

                UpdateLayout( );
                lzone = null;

                if ( DropNewAirportZone != null )
                {
                    DropNewAirportZone( result, tmp );
                }

                Cursor = Cursors.Arrow;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Modify the AirportZone tooltip when the mouse is hovering it
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event type</param>
        private void ZoneMouseDragEnter( object sender, System.Windows.DragEventArgs e )
        {
            ( sender as AirportZone ).AllowDrop = ( sender as AirportZone ).IsBuilt
                                                  ? false
                                                  : true;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Get AirportZone type going through a drag event
        /// </summary>
        /// <param name="sender">Drag Event type</param>
        /// <returns>Exact type of the dragged object if it exists inside the AirportLibrary
        /// otherwise null
        /// </returns>
        private Type GetTypeAirportZone( System.Windows.DragEventArgs sender )
        {
            if ( sender.Data.GetDataPresent( typeof( CheckIn ) ) )
            {
                return typeof( CheckIn );
            }

            if ( sender.Data.GetDataPresent( typeof( DestinationGate ) ) )
            {
                return typeof( DestinationGate );
            }

            if ( sender.Data.GetDataPresent( typeof( Sorter ) ) )
            {
                return typeof( Sorter );
            }

            if ( sender.Data.GetDataPresent( typeof( Storage ) ) )
            {
                return typeof( Storage );
            }

            return null;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Event handler emitted when an AirportZone is dropped
        /// </summary>
        public event AirportHandler DropNewAirportZone;
        /// <summary>
        /// Event handler emitted when an AirportZone is clicked
        /// </summary>
        public event AirportClickHandler ClickOnAirportZone;

        private bool mouseWasDown;
        private AirportZone lzone;
    }

    /// <summary>
    /// Represents the method that will handle an event that has two AirportZone datas.
    /// </summary>
    /// <param name="oldzone">AirportZone source</param>
    /// <param name="newZone">AirportZone destination</param>
    public delegate void AirportHandler( AirportZone oldzone, AirportZone newZone );
    /// <summary>
    /// Represents the method that will handle an event that has one AirportZone and a MouseButtonEventArgs.
    /// </summary>
    /// <param name="subject">Clicked object</param>
    /// <param name="e">MouseButton event</param>
    public delegate void AirportClickHandler( AirportZone subject, MouseButtonEventArgs e );
}
