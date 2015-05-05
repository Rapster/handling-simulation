using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Airport.Utils;

namespace Airport.Forms
{
    class PaintPanel : Panel
    {
        public PaintPanel( )
            : base( )
        {
            ZoneStart = new AirportZone( );
            ZoneEnd = new AirportZone( );
        }

        // --------------------------------------------------------------------

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return createParams;
            }
        }

        // --------------------------------------------------------------------

        protected override void OnPaint( PaintEventArgs e )
        {
            foreach ( Conveyor conveyor in conveyors )
            {
                e.Graphics.DrawLine( Pens.Black,
                                     conveyor.Start.Location,
                                     conveyor.End.Location );
            }

            if ( Draw )
            {
                e.Graphics.DrawLine( Pens.Black, ZoneStart.Center,
                                                 ZoneEnd.Center );
            }
        }

        // --------------------------------------------------------------------

        protected override void OnPaintBackground( PaintEventArgs e )
        {
        }

        // --------------------------------------------------------------------

        public bool Draw
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        public AirportZone ZoneStart
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        public AirportZone ZoneEnd
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        public void AddConveyor( Conveyor conveyor )
        {
            if ( conveyor.Start != null
                 && conveyor.End != null )
            {
                conveyor.Start.Location = conveyor.Start.Center;
                conveyor.End.Location = conveyor.End.Center;
                conveyors.Add( conveyor );
            }
        }

        // --------------------------------------------------------------------

        private List<Conveyor> conveyors = new List< Conveyor >( );
        
    }
}
