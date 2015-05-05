using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Airport.Utils;
using System.Collections.Generic;

namespace Airport.Controls
{
    /// <summary>
    /// The basic graphics object in the simulation, serves as a link between the graphical and model subsystem.
    /// Can only exist as an element in the collection within AirportArea.
    /// May contain 0 or 1 component. 
    /// </summary>
    public class AirportZone : System.Windows.Controls.ContentControl,
                               IEquatable<AirportZone>,
                               IComparable<AirportZone>
    {
        /// <summary>
        /// Initialize a new instance of a AirportZone
        /// </summary>
        /// <param name="area">Parent reference to the AirportArea object that contains the zone.</param>
        /// <param name="position">X and Y coordinates within the visual grid.</param>
        /// <param name="image">Background image</param>
        /// <param name="isBuilt">Zone concretly present in its parent</param>
        public AirportZone( AirportArea area,
                            Index position,
                            string image,
                            bool isBuilt = false )
        {
            Area = area;
            Position = position;
            Background = image;
            AllowDrop = true;
            IsBuilt = isBuilt;
            BorderBrush = Brushes.Transparent;
            Destinations = new List<AirportZone>( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Initialize a new instance of a AirportZone
        /// </summary>
        public AirportZone( )
            : this( null,
                    new Index( 0, 0 ),
                    string.Empty,
                    false )
        {
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// X and Y coordinates within the visual grid.
        /// </summary>
        public Index Position
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Indicates whether there is a component in the AirportZone.
        /// </summary>
        public bool IsBuilt
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// An URI to the bitmap image that is displayed within the AirportZone.
        /// </summary>
        public new string Background
        {
            get
            {
                return ( Content != null )
                        ? ( Content as Image ).Source.ToString( )
                        : null;
            }
            set
            {
                if ( value != String.Empty
                     && value != null )
                {
                    Content = new Image
                    {
                        Source = new BitmapImage( new Uri( value ) )
                    };

                    return;
                }

                Content = null;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// X and Y coordinates on the screen (pixel values).
        /// </summary>
        public Point Location
        {
            get
            {
                Point pt = new Point( 0, 0 );

                if ( Area != null
                     && Area is Visual
                     && Area.Children.Contains( this ) )
                {
                    pt = Area.TransformToVisual( this ).Transform( pt );
                    pt = new Point( pt.X * -1, pt.Y * -1 );
                }

                return pt;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// X and Y coordinates of the center of the AirportZone
        /// </summary>
        public Point Center
        {
            get
            {
                Point pt = Location;
                return new Point( pt.X + ( RenderSize.Width / 2 ),
                                  pt.Y + ( RenderSize.Height / 2 ) );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Checks whether a point is within the rectangle of the AirportZone
        /// </summary>
        /// <param name="location">Point to be checked against the AirportZone</param>
        /// <returns></returns>
        public bool BelongsTo( Point location )
        {
            Point loc = Location;

            return ( location.X >= loc.X
                     && location.X < loc.X + RenderSize.Width
                     && location.Y >= loc.Y
                     && location.Y < loc.Y + RenderSize.Height )
                     ? true
                     : false;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Parent reference to the AirportArea object that contains the zone.
        /// </summary>
        public AirportArea Area
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// A list of all the destination gates that this airport zone is connected to.
        /// </summary>
        public List<AirportZone> Destinations
        {
            get;
            private set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Zero-based index within the AirportArea Children collection. 
        /// </summary>
        public int Index
        {
            get
            {
                int index = 0;
                return Area != null
                       && ( index = Area.Columns * Position.Y + Position.X ) >= 0
                       && index < Area.Children.Count
                       ? index
                       : -1;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// The unique ID of the destination gate.
        /// One-based version of the index of the AirportZone object within the AirportArea Children collection.
        /// 0 if the AirportZone does not contain a component yet.
        /// </summary>
        public virtual int Rank
        {
            get
            {
                int rank = -1;

                if ( Area != null
                    && Area.Children.Contains( this ) )
                {
                    List<AirportZone> zones = Area.Zones.FindAll(
                        delegate( AirportZone zone )
                        {
                            return zone.IsBuilt
                                   && zone.GetType( ) == GetType( );
                        }
                    );

                    zones.Sort( );
                    rank = zones.IndexOf( this );
                }

                return rank + 1;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Compares the current instance with another zone of the same type
        /// and returns an integer that indicates whether the current instance 
        /// precedes, follows, or occurs in the same position in the sort order 
        /// as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance. </param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo( AirportZone other )
        {
            return Index.CompareTo( other.Index );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Checks whether a zone is in the same position and of the same type.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals( AirportZone other )
        {
            return other != null
                   && Position == other.Position
                   && GetType( ) == other.GetType( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Adds a destination to the destinations list, when a new conveyor line is created.
        /// </summary>
        /// <param name="zone"></param>
        public void AddDestination( AirportZone zone )
        {
            if ( !Destinations.Contains( zone ) )
            {
                Destinations.Add( zone );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Get component's name
        /// </summary>
        public new virtual string Name
        {
            get
            {
                return "A" + Rank.ToString( );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// When overridden in a derived class, participates in rendering operations 
        /// that are directed by the layout system. The rendering instructions for this 
        /// element are not used directly when this method is invoked, and are instead 
        /// preserved for later asynchronous use by layout and drawing.
        /// </summary>
        /// <param name="drawingContext">The drawing instructions for a specific element.
        /// This context is provided to the layout system.</param>
        protected override void OnRender( DrawingContext drawingContext )
        {

            drawingContext.DrawRectangle( Brushes.White, new Pen( BorderBrush, 0.2 ),
                                          new Rect( RenderSize ) );
        }

        // --------------------------------------------------------------------
    }
}
