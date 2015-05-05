using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using DrawToolsLib;
using System.Diagnostics;

namespace Airport.Controls
{
    /// <summary>
    /// Interaction logic for AirportDrawing.xaml
    /// </summary>
    public partial class AirportDrawing : UserControl
    {
        /// <summary>
        /// Initialize a new instance of AiportDrawing
        /// </summary>
        public AirportDrawing( )
        {
            InitializeComponent( );
            selectedZones = new List<AirportZone>( airportArea.Rows * airportArea.Columns );
            selectedWays = new Dictionary<Way, List<Line>>( );

            currentConveyor = new Conveyor( null, null );

            zones = new List<AirportZone>( );
            graphicsLine = new Dictionary<Conveyor, GraphicsLine>( );

            drawingCanvas.SelectionDeleted +=
                new SelectionDeletedHandler( SelectionDeleted );
            drawingCanvas.AllGraphicsDeleted +=
                new AllSelectionDeletedHandler( AllGraphicsDeleted );

            ways = new Ways( );
            ResetSelection( );

            WayStartCreation += new WayStatusCreation( WayCreationStarted );
            WayEndCreation += new WayStatusCreation( WayCreationEnded );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the AiportLibrary.Controls.AirportArea
        /// </summary>
        public AirportArea AirportArea
        {
            get { return airportArea; }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the set of AirportZone contained in the AirportArea
        /// </summary>
        public List<AirportZone> Zones
        {
            get { return airportArea.Zones; }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the plotting mode
        /// </summary>
        public bool PlottingMode
        {
            get
            {
                return drawingCanvas.IsHitTestVisible;
            }
            set
            {
                CheckCurrentWay( );
                airportArea.IsHitTestVisible = !value;
                drawingCanvas.IsHitTestVisible = value;
                zones.Clear( );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the selection mode
        /// </summary>
        public SelectedMode SelectionMode
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Define the way how the AirportZone selection can be set.
        /// </summary>
        public enum SelectedMode
        {
            /// <summary>
            /// One component selection
            /// </summary>
            SelectOne,
            /// <summary>
            /// Multiple component selection
            /// </summary>
            SelectSeveral
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Select all the built AirportZone contained in the AirportArea
        /// </summary>
        public void SelectAll( )
        {
            foreach ( AirportZone zone in airportArea.Zones )
            {
                if ( zone.IsBuilt
                    && !selectedZones.Contains( zone ) )
                {
                    SetSelected( zone );
                }
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Unselect all the built AirportZone contained in the AirportArea
        /// </summary>
        public void UnSelectAll( )
        {
            ResetSelection( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Remove all the component contained in the AirportDrawing
        /// </summary>
        public void Clear( )
        {
            ResetSelection( );
            drawingCanvas.Clear( );
            airportArea.Clear( );
            ResetAttributes( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Fill the AirportDrawing through the set of Way containing the set of 
        /// AirportZone and draw the corresponding conveyors
        /// </summary>
        /// <param name="ways">Set of Way to draw</param>
        public void FillDrawingArea( List<Way> ways )
        {
            drawingCanvas.Clear( );
            this.ways.Children = ways;

            foreach ( Way way in this.ways.Children )
            {
                foreach ( Conveyor conveyor in way.Conveyors )
                {
                    GraphicsLine o = new GraphicsLine( conveyor.Start.Center,
                                                       conveyor.End.Center,
                                                       drawingCanvas.LineWidth,
                                                       Colors.Black,
                                                       drawingCanvas.ActualScale );

                    GraphicsLine line = drawingCanvas.Find( delegate( Visual v )
                    {
                        return ( v as GraphicsLine ).Start == o.Start
                               && ( v as GraphicsLine ).End == o.End
                               || ( v as GraphicsLine ).Start == o.End
                               && ( v as GraphicsLine ).End == o.Start;
                    }
                    ) as GraphicsLine;

                    line = line == null
                           ? o
                           : line;

                    graphicsLine.Add( conveyor, line );

                    if ( !drawingCanvas.Graphics.Contains( line ) )
                    {
                        drawingCanvas.Graphics.Add( line );
                    }
                }
            }

            UpdateInputOutput( );
            UpdateDestinations( );
            drawingCanvas.RefreshClip( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Re-initialize the selection area
        /// </summary>
        public void ResetSelection( )
        {
            contentWays.Children.Clear( );
            contentZones.Children.Clear( );
            selectedZones.Clear( );
            selectedWays.Clear( );

            for ( int i = 0;
                  i < airportArea.Rows * airportArea.Columns;
                  ++i )
            {
                contentZones.Children.Add( new Rectangle( ) );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Clear the selection trace
        /// </summary>
        public void ClearSelection( )
        {
            for ( int i = selectedZones.Count - 1; i >= 0; --i )
            {
                List<Way> tmp = FindWays( selectedZones[ i ] );

                foreach ( Way way in tmp )
                {
                    RemoveWay( way );
                }

                AirportArea.Remove( selectedZones[ i ] );
                selectedZones.RemoveAt( i );
            }

            ResetSelection( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the set of Way contained in the AirportDrawing
        /// </summary>
        public List<Way> Ways
        {
            get
            {
                UpdateDestinations( );
                return ways.Children;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Update the destination of each AirportZone linked through a conveyor
        /// </summary>
        private void UpdateDestinations( )
        {
            foreach ( Way w in ways.Children )
            {
                foreach ( AirportZone zone in w.Zones )
                {
                    zone.Destinations.Clear( );
                }
            }

            foreach ( Way way in ways.Children )
            {
                List<AirportZone> zones = way.Zones;

                for ( int i = zones.Count - 1; i >= 0; --i )
                {
                    if ( !( zones[ i ] is DestinationGate ) )
                    {
                        zones[ i ].AddDestination( zones[ zones.Count - 1 ] );
                        continue;
                    }

                    if ( !( zones[ i ] as DestinationGate ).PlaneIsPresent )
                    {
                        zones[ i ].AddDestination( ( zones[ i ] as DestinationGate ).Storage );
                    }
                }
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Display in circle label the name of each built AirportZone
        /// </summary>
        public void DisplayRankLabels( )
        {
            contentZones.Children.Clear( );
            selectedZones.Clear( );

            for ( int i = 0; i < airportArea.Zones.Count; ++i )
            {
                contentZones.Children.Add( new Label( ) );
            }

            List<AirportZone> zones = airportArea.Zones.FindAll(
                                        delegate( AirportZone zone )
                                        {
                                            return zone.IsBuilt
                                                   && ( zone is CheckIn
                                                   || zone is DestinationGate );
                                        }
            );

            foreach ( AirportZone zone in zones )
            {
                ( contentZones.Children[ zone.Index ] as Label ).Template =
                    Resources[ "CircleLabel" ] as ControlTemplate;

                ( contentZones.Children[ zone.Index ] as Label ).Content
                    = zone.Name;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Overlight a way
        /// </summary>
        /// <param name="way">Way to overlight</param>
        public void SetSelected( Way way )
        {
            List<Line> tmp = new List<Line>( );

            foreach ( Conveyor c in way.Conveyors )
            {
                Line line = new Line( );
                line.X1 = c.Start.Center.X;
                line.X2 = c.End.Center.X;
                line.Y1 = c.Start.Center.Y;
                line.Y2 = c.End.Center.Y;
                line.Stroke = Brushes.Red;
                line.StrokeThickness = 2;
                contentWays.Children.Add( line );
                tmp.Add( line );
            }

            if ( !selectedWays.ContainsKey( way ) )
            {
                selectedWays.Add( way, tmp );
            }

            SetSelected( way.Start );
            SetSelected( way.End );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Unlight a way
        /// </summary>
        /// <param name="way">Way to unlight</param>
        public void SetUnSelected( Way way )
        {
            if ( selectedWays.ContainsKey( way ) )
            {
                foreach ( Shape line in selectedWays[ way ] )
                {
                    contentWays.Children.Remove( line );
                }

                selectedWays.Remove( way );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Overlight the corresponding way of a AirportZone
        /// </summary>
        /// <param name="zone">AirportZone to overlight</param>
        public void SetSelected( AirportZone zone )
        {
            Rectangle rect = contentZones.Children[ zone.Index ] as Rectangle;
            SolidColorBrush myBrush = new SolidColorBrush( Colors.Transparent );
            rect.Fill = myBrush;
            rect.RadiusX = 7;
            rect.RadiusY = 7;
            rect.Stroke = Brushes.Red;
            rect.StrokeThickness = 2;

            selectedZones.Add( zone );
            contentZones.Children[ zone.Index ] = rect;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Overlight a set of Way.
        /// </summary>
        /// <param name="ways">List of Way to overlight</param>
        public void SetSelected( List<Way> ways )
        {
            foreach ( Way way in ways )
            {
                SetSelected( way );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Unlight the corresponding way of a AirportZone
        /// </summary>
        /// <param name="zone">AirportZone to unlight</param>
        private void SetUnSelected( AirportZone zone )
        {
            int index = ( airportArea.Columns * (int)zone.Position.Y + (int)zone.Position.X );
            Rectangle rect = contentZones.Children[ index ] as Rectangle;
            SolidColorBrush myBrush = new SolidColorBrush( Colors.Transparent );
            rect.Fill = myBrush;

            rect.Stroke = Brushes.Transparent;
            rect.StrokeThickness = 0;
            selectedZones.Remove( zone );
            contentZones.Children[ index ] = rect;

            foreach ( Way way in FindWays( zone ) )
            {
                SetUnSelected( way );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Unlight a set of Way.
        /// </summary>
        /// <param name="ways">List of Way to unlight.</param>
        private void SetUnSelected( List<Way> ways )
        {
            foreach ( Way way in ways )
            {
                SetUnSelected( way );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Event begining of a plotting line, determinates, if the current zone is built, 
        /// that a new way is creating.
        /// </summary>
        /// <param name="sender">Emitter object</param>
        /// <param name="e">Mouse button object</param>
        private void OnMouseDown( object sender, MouseButtonEventArgs e )
        {
            mouseWasDown = true;

            Point pos = e.GetPosition( airportArea );

            AirportZone zone = ( ( zone = airportArea.ClosestZone( currentZone, pos ) ) != null )
                                   ? zone
                                   : ( zone = airportArea.FindBy( pos ) ) != null
                                   ? zone
                                   : null;

            if ( currentZone is CheckIn
                || currentZone is Sorter
                && WayStartCreation != null )
            {
                WayStartCreation( this, currentZone );
            }

            if ( zone != null )
            {
                currentConveyor.Start = zone;
            }
            else
            {
                ResetSelection( );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Refresh the plotting of the current conveyor
        /// </summary>
        /// <param name="sender">Emitter object</param>
        /// <param name="e">Mouse event object</param>
        private void OnMouseMove( object sender, MouseEventArgs e )
        {
            Point pos = e.GetPosition( airportArea );

            if ( currentZone != null
                && currentConveyor.Start != null
                && currentZone.BelongsTo( pos ) )
            {
                return;
            }

            if ( ( currentZone = airportArea.ClosestZone( currentZone, pos ) ) != null
                                 ? false
                                 : ( currentZone = airportArea.FindBy( pos ) ) != null
                                 ? false
                                 : true )
            {
                drawingCanvas.Tool = ToolType.Pointer;
                return;
            }

            drawingCanvas.Tool = NeededTool( );

            if ( e.LeftButton == MouseButtonState.Pressed
                 && drawingCanvas.Tool == ToolType.Line )
            {
                drawingWay = true;
                currentConveyor.End = currentZone;

                if ( ConveyorIsDrawing != null )
                {
                    ConveyorIsDrawing( currentConveyor, e );
                }
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Display a rectangle on the corresponding zone indicating that a zone is selected.
        /// Click on it while it's already selected unselect the current zone.
        /// </summary>
        /// <param name="zone">Clicked AirportZone</param>
        /// <param name="e">Mouse event object</param>
        private void AirportZoneOnMouseDown( AirportZone zone, MouseButtonEventArgs e )
        {
            if ( SelectionMode == SelectedMode.SelectOne
                 && selectedZones.Count != 0
                 && !selectedZones.Contains( zone ) )
            {
                ResetSelection( );
            }

            if ( !zone.IsBuilt )
            {
                ResetSelection( );
                return;
            }

            if ( ( contentZones.Children[ zone.Index ] as Rectangle ).StrokeThickness < 2 )
            {
                SetSelected( zone );
                SetSelected( FindWays( zone ) );
            }
            else
            {
                SetUnSelected( zone );
                SetUnSelected( FindWays( zone ) );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// All conveyors corresponding to the zone newZone follow as they are valid.
        /// </summary>
        /// <param name="oldZone">Drag'n drop on the same area, corresponds to the source AirportZone.</param>
        /// <param name="newZone">Final destination of the dragged AirportZone.</param>
        private void NewAirportZoneWasDropped( AirportZone oldZone, AirportZone newZone )
        {
            List<Way> ways = FindWays( oldZone );

            if ( oldZone != null
                && ways.Count != 0 )
            {
                SetUnSelected( oldZone );

                foreach ( Way way in ways )
                {
                    foreach ( Conveyor conveyor in way.Conveyors )
                    {
                        if ( !conveyor.Start.Equals( oldZone )
                             && !conveyor.End.Equals( oldZone ) )
                        {
                            continue;
                        }

                        conveyor.Start = conveyor.Start.Equals( oldZone )
                                            ? newZone
                                            : conveyor.Start;

                        conveyor.End = conveyor.End.Equals( oldZone )
                                            ? newZone
                                            : conveyor.End;

                        GraphicsBase result = graphicsLine[ conveyor ];

                        if ( conveyor.IsValid )
                        {
                            ( result as GraphicsLine ).Start =
                                    conveyor.Start.Center;
                            ( result as GraphicsLine ).End =
                                    conveyor.End.Center;
                        }
                        else
                        {
                            RemoveWay( way );
                        }
                    }

                    UpdateDestinations( );
                    drawingCanvas.RefreshClip( );
                }
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Change the current cursor into the default pointer.
        /// </summary>
        /// <param name="sender">Emitter object</param>
        /// <param name="e">Mouse event object</param>
        private void OnMouseLeave( object sender, MouseEventArgs e )
        {
            drawingCanvas.Tool = ToolType.Pointer;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Finalize the conveyor creation, if the current AirportZone is a Destination Gate object
        /// so, it finalize the creation of a Way with all the conveyor previously created.
        /// </summary>
        /// <param name="sender">Emitter object</param>
        /// <param name="e">Mouse button event object</param>
        private void OnMouseUp( object sender, MouseButtonEventArgs e )
        {
            GraphicsBase o = null;

            if ( currentZone != null
                && currentConveyor.IsValid
                && drawingCanvas.Count != 0 )
            {
                GraphicsLine line = ( drawingCanvas.Last( ) as GraphicsLine );
                line.Start = currentConveyor.Start.Center;
                line.End = currentConveyor.End.Center;

                Conveyor conveyor = ways.Conveyors.Find( delegate( Conveyor tmp )
                {
                    return currentConveyor.Start.Equals( tmp )
                           && currentConveyor.End.Equals( tmp );
                }
                );

                if ( conveyor == null )
                {
                    graphicsLine.Add( currentConveyor.Clone( ), line );
                }

                if ( currentZone is DestinationGate
                     && WayEndCreation != null )
                {
                    WayCreationEnded( this, currentZone );
                }

                drawingCanvas.Tool = NeededTool( );
                drawingCanvas.RefreshClip( );
                zones.Add( currentZone );
            }
            else if ( drawingWay
                     && ( o = drawingCanvas.LastActive( ) ) != null )
            {
                drawingCanvas.Graphics.Remove( o );
            }

            mouseWasDown = false;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Delete all the way contained in DrawingArea.
        /// </summary>
        private void AllGraphicsDeleted( )
        {
            graphicsLine.Clear( );
            ways.Children.Clear( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Delete selected ways through graphics object contained in the DrawingCanvas.
        /// </summary>
        /// <param name="graphics">List of GraphicsBase object deleted from the DrawingCanvas.</param>
        private void SelectionDeleted( List<GraphicsBase> graphics )
        {
            foreach ( GraphicsBase g in graphics )
            {
                RemoveWay( g as GraphicsLine );
            }

            UpdateDestinations( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Indicates if there is any conveyor linked to the specified AirportZone
        /// </summary>
        /// <param name="zone">AirportZone object to test.</param>
        /// <returns>True if at least one conveyor is linked, otherwise false.</returns>
        private bool HasConveyor( AirportZone zone )
        {
            foreach ( KeyValuePair<Conveyor, GraphicsLine> kvp in graphicsLine )
            {
                if ( kvp.Key.Start.Equals( zone )
                    || kvp.Key.End.Equals( zone ) )
                {
                    return true;
                }
            }

            return false;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Redraw all the graphics conveyors contained/selected in the DrawingCanvas
        /// </summary>
        /// <param name="drawingContext">The drawing instructions for a specific element.
        /// This context is provided to the layout system.</param>
        protected override void OnRender( DrawingContext drawingContext )
        {
            Dictionary<Way, List<Line>> ways = 
                new Dictionary<Way,List<Line>>( selectedWays );

            ResetSelection( );
                
            foreach ( KeyValuePair<Conveyor, GraphicsLine> kvp in graphicsLine )
            {
                kvp.Value.Start = kvp.Key.Start.Center;
                kvp.Value.End = kvp.Key.End.Center;

                foreach ( KeyValuePair<Way, List<Line>> tmp in ways )
                {
                    SetSelected( tmp.Key );
                }
            }

            drawingCanvas.RefreshClip( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Set all the input and output of the conveyors contained in a Way object.
        /// </summary>
        /// <param name="way">Way object to set input and output</param>
        private void SetInputOutput( Way way )
        {
            foreach ( Conveyor conveyor in way.Conveyors )
            {
                if ( conveyor.Start is AirportZone
                     && conveyor.End is Sorter )
                {
                    if ( conveyor.Start is CheckIn )
                    {
                        ( conveyor.Start as CheckIn ).Output = conveyor;
                    }

                    ( conveyor.End as Sorter ).Input = conveyor;
                }

                if ( conveyor.Start is Sorter
                    && conveyor.End is AirportZone )
                {
                    ( conveyor.Start as Sorter ).AddOutput( conveyor );
                }
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Update all the input and output of the ways contained in the DrawingArea.
        /// </summary>
        private void UpdateInputOutput( )
        {
            foreach ( Way way in Ways )
            {
                SetInputOutput( way );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Set the current conveyor extremities, the current zone to null and 
        /// delete all the ways contained in the DrawingArea.
        /// </summary>
        private void ResetAttributes( )
        {
            currentConveyor.Start = null;
            currentConveyor.End = null;
            currentZone = null;
            ways.Children.Clear( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Determinate which cursor to use according to the hovered zone.
        /// </summary>
        /// <returns>ToolType.Line if it's possible to draw a conveyor, otherwise ToolType.Pointer.</returns>
        private DrawToolsLib.ToolType NeededTool( )
        {
            return ( ( mouseWasDown
                       && currentConveyor.Start != null
                       && currentConveyor.Start.IsBuilt )
                       || ( currentZone is CheckIn
                       && ( currentZone as CheckIn ).Output == null )
                       || ( currentZone is Sorter
                       && !( currentZone as Sorter ).IsFull
                       && HasConveyor( currentZone ) )
                       || ( mouseWasDown
                       && drawingWay )
                       || ( drawingWay
                       && HasConveyor( currentZone ) ) )
                       ? ToolType.Line
                       : ToolType.Pointer;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Create a Way object with AirportZone objects since a CheckIn was cliked.
        /// </summary>
        /// <returns>A Way object, the validity of this object is not controlled yet.</returns>
        private Way CreateWay( )
        {
            List<Conveyor> conveyors = new List<Conveyor>( );
            for ( int i = 0; i < zones.Count - 1; ++i )
            {
                Conveyor conveyor = new Conveyor( zones[ i ], zones[ i + 1 ] );

                foreach ( KeyValuePair<Conveyor, GraphicsLine> kvp in graphicsLine )
                {
                    if ( ( kvp.Key.Start.Equals( conveyor.Start )
                        && kvp.Key.End.Equals( conveyor.End ) )
                        || ( kvp.Key.Start.Equals( conveyor.End )
                        && kvp.Key.End.Equals( conveyor.Start ) ) )
                    {
                        conveyor = kvp.Key;
                        break;
                    }
                }

                conveyors.Add( conveyor );
            }

            return new Way( conveyors );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Check the validity of the Way created through the CreateWay function.
        /// </summary>
        private void CheckCurrentWay( )
        {
            Way way = CreateWay( );

            if ( !way.IsValid
                 && zones.Count > 0 )
            {
                RemoveWay( way );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Remove a Way object contained in the DrawingArea.
        /// </summary>
        /// <param name="way">Object to remove.</param>
        private void RemoveWay( Way way )
        {
            foreach ( Conveyor conveyor in way.Conveyors )
            {
                if ( ways.WaysInvolvedBy( conveyor ).Count < 2 )
                {
                    drawingCanvas.Remove( graphicsLine[ conveyor ] );
                    graphicsLine.Remove( conveyor );

                    CheckIn c = conveyor.Start is CheckIn
                                ? conveyor.Start as CheckIn
                                : conveyor.End as CheckIn;

                    if ( c != null
                         && c.Output == conveyor )
                    {
                        c.Output = null;
                    }
                }

                Sorter s = conveyor.Start is Sorter
                           ? conveyor.Start as Sorter
                           : conveyor.End as Sorter;

                if ( s != null
                     && s.Input == conveyor )
                {
                    s.Input = null;
                }

                if ( s != null
                     && s.Outputs.Contains( conveyor ) )
                {
                    s.Outputs.Remove( conveyor );
                }
            }

            ways.Children.Remove( way );

            if ( WayIsDeleted != null )
            {
                WayIsDeleted( this, way );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Remove a Way contained in the DrawingArea object through the GraphicsBase
        /// object associated to one of its conveyors.
        /// </summary>
        /// <param name="line">GraphicsBase object associated to one of the conveyors Way.</param>
        private void RemoveWay( GraphicsLine line )
        {
            List<Way> tmp = new List<Way>( );

            foreach ( KeyValuePair<Conveyor, GraphicsLine> kvp in graphicsLine )
            {
                if ( kvp.Value == line )
                {
                    foreach ( Way way in ways.Children )
                    {
                        if ( way.Conveyors.Contains( kvp.Key ) )
                        {
                            tmp.Add( way );
                        }
                    }
                }
            }

            foreach ( Way t in tmp )
            {
                RemoveWay( t );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Find all the Way object linked to an AirportZone.
        /// </summary>
        /// <param name="zone">AirportZone to test</param>
        /// <returns>A set of Way linked to the specified AirportZone it they're existing,
        /// otherwise null.</returns>
        private List<Way> FindWays( AirportZone zone )
        {
            return ways.Children.FindAll( delegate( Way w )
            {
                return w.Conveyors.Find( delegate( Conveyor c )
                {
                    return ( c.Start == zone
                             || c.End == zone );
                }
                ) != null ? true : false;
            }
            );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Emit when OnMouseUp is called on a Destination Gate, it indicates the end of the creation of a Way.
        /// It creates a new Way if this one is valid and update destination for all conveyors contained.
        /// </summary>
        /// <param name="sender">Emitter object.</param>
        /// <param name="zone">Last object of the current Way.</param>
        private void WayCreationEnded( AirportDrawing sender, AirportZone zone )
        {
            zones.Add( currentZone );
            Way way = CreateWay( );
            Way tmp = null;

            if ( way.IsValid )
            {
                SetInputOutput( way );
                ways.Add( way );
            }
            else if ( ( tmp = ways.GetAvailableWay( way.Start ) ) != null
                        && ( tmp = tmp.Append( way ) ).IsValid )
            {
                zones = tmp.Zones;
                SetInputOutput( tmp );
                ways.Add( tmp );
            }

            if ( ( way.IsValid
                || tmp.IsValid )
                && WayIsCreated != null )
            {
                WayIsCreated( this, way );
            }

            if ( ReleaseMouse != null )
            {
                ReleaseMouse( sender, null );
            }

            CheckCurrentWay( );
            UpdateDestinations( );

            zones.Clear( );
            currentConveyor.Start = currentConveyor.End = null;
            drawingWay = false;
            drawingCanvas.Tool = ToolType.Pointer;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Emit when OnMouseUP is called on a CheckIn or a Sorter object, it indicates 
        /// the begining of the creation of a Way.
        /// </summary>
        /// <param name="sender">Emitter object</param>
        /// <param name="zone">First object of the current Way.</param>
        private void WayCreationStarted( AirportDrawing sender, AirportZone zone )
        {
            if ( zone is CheckIn )
            {
                zones.Clear( );
                zones.Add( currentZone );
            }

            if ( zone is Sorter
                && zones.Count == 0 )
            {
                zones.Add( currentZone );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Occurs when a conveyor is drawing on the DrawningArea.
        /// </summary>
        public event ConveyorHandler ConveyorIsDrawing;
        /// <summary>
        /// Occurs when mouse up is occurs. 
        /// </summary>
        public event EventHandler ReleaseMouse;
        /// <summary>
        /// Occurs when a new Way is created.
        /// </summary>
        public event WayHandler WayIsCreated;
        /// <summary>
        /// Occurs when an existing Way is deleted.
        /// </summary>
        public event WayHandler WayIsDeleted;

        /// <summary>
        /// Occurs when a click is occuring on a CheckIn or Sorter object 
        /// </summary>
        public event WayStatusCreation WayStartCreation;
        /// <summary>
        /// Occurs when a click is occuring on a DestinationGate object
        /// </summary>
        public event WayStatusCreation WayEndCreation;


        private bool mouseWasDown;
        private bool drawingWay;

        private List<AirportZone> selectedZones;
        private Dictionary<Way, List<Line>> selectedWays;
        private List<AirportZone> zones;

        private Dictionary<Conveyor, GraphicsLine> graphicsLine;
        private Ways ways;
        private Conveyor currentConveyor;
        private AirportZone currentZone;
    }

    // --------------------------------------------------------------------

    /// <summary>
    /// Represents the method that will handle the event that has an object
    /// data and a Way data.
    /// </summary>
    /// <param name="sender">Emitter object</param>
    /// <param name="way">Way data</param>
    public delegate void WayHandler( object sender, Way way );
    /// <summary>
    /// Represents the method that will handle the event that has a Conveyor
    /// data and a MouseEventArgs object.
    /// </summary>
    /// <param name="sender">Conveyor data</param>
    /// <param name="e">MouseEventArgs data</param>
    public delegate void ConveyorHandler( Conveyor sender, MouseEventArgs e );
    /// <summary>
    /// Represents the method that will handle the event that has a AirportDrawing
    /// data and an AirportZone object.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="zone"></param>
    public delegate void WayStatusCreation( AirportDrawing sender, AirportZone zone );
}
