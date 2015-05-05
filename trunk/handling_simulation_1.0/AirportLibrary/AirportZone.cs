using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Airport.Forms
{
    public class AirportZone : Control
    {
        public AirportZone( )
            : this( new Point( 0, 0 ) )
        {
        }

        // --------------------------------------------------------------------

        public AirportZone( Point position )
        {
            Id = Guid.NewGuid( );
            Position = position;
            DrawGrid = true;
            AllowDrop = true;
            BackgroundImageLayout = ImageLayout.Center;
        }

        // --------------------------------------------------------------------

        public AirportZone( AirportZone zone )
        {
            Id = zone.Id;
            Position = zone.Position;
            DrawGrid = zone.DrawGrid;
            BackgroundImage = zone.BackgroundImage;
            IsMarked = zone.IsMarked;
            Location = zone.Location;
            AllowDrop = zone.AllowDrop;
            BackgroundImageLayout = zone.BackgroundImageLayout;
            Size = zone.Size;
        }

        // --------------------------------------------------------------------

        protected override void OnPaint( System.Windows.Forms.PaintEventArgs e )
        {
            if ( DrawGrid
                && Airport.Utils.Monitor.Instance( ).AllowRefresh )
            {
                System.Windows.Forms.ControlPaint.DrawGrid( e.Graphics,
                                                            e.ClipRectangle,
                                                            Size,
                                                            System.Drawing.Color.Transparent );
            }
        }

        // --------------------------------------------------------------------

        public override string ToString( )
        {
            return GetType( ).ToString( ) + Position.ToString( );
        }

        // --------------------------------------------------------------------

        public AirportZone Clone( )
        {
            return new AirportZone( this );
        }

        // --------------------------------------------------------------------

        public bool DrawGrid
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        public Point Position
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        public Guid Id
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        public bool IsMarked
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        public Point Center
        {
            get
            {
                return new Point( Location.X + ( Size.Width / 2 ),
                                  Location.Y + ( Height / 2 ) ); ;
            }
        }

        // --------------------------------------------------------------------
    }
}
