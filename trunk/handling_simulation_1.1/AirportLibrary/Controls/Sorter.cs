using System;
using Airport.Utils;
using System.Collections.Generic;

namespace Airport.Controls
{
    /// <summary>
    /// Serves as a merging/separating point for conveyor lines.
    /// </summary>
    public class Sorter : AirportZone
    {
        /// <summary>
        /// Initialize a new instance of Sorter.
        /// </summary>
        public Sorter( )
            : this( null,
                    new Index( 0, 0 ),
                    String.Empty,
                    false )
        {
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Instanciate a new instance of a Sorter
        /// </summary>
        /// <param name="area">Parent reference to the AirportArea object that contains the zone.</param>
        /// <param name="position">X and Y coordinates within the visual grid.</param>
        /// <param name="image">Background image</param>
        /// <param name="isBuilt">Zone concretly present in its parent</param>
        public Sorter( AirportArea area,
                       Index position,
                       string image,
                       bool isBuilt )
            : base( area,
                    position,
                    image,
                    isBuilt )
        {
            Outputs = new List<Conveyor>( 2 );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Get component's name
        /// </summary>
        public override string Name
        {
            get
            {
                return "So" + Rank.ToString( );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the unique input conveyor
        /// </summary>
        public Conveyor Input
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the conveyors located in output. Maximum output is 2.
        /// </summary>
        public List<Conveyor> Outputs
        {
            get;
            private set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Add a new conveyor in the ouputs, if this one is not full
        /// </summary>
        /// <param name="conveyor">Conveyor to add</param>
        public void AddOutput( Conveyor conveyor )
        {
            if ( !Outputs.Contains( conveyor )
                 && !IsFull )
            {
                Outputs.Add( conveyor );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Remove a output contained in the ouputs
        /// </summary>
        /// <param name="conveyor">Conveyor to remove</param>
        public void RemoveOutput( Conveyor conveyor )
        {
            if ( Outputs.Contains( conveyor ) )
            {
                Outputs.Remove( conveyor );
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets the state of the sorter, indicating if the sorter contained the max number of output.
        /// </summary>
        public bool IsFull
        {
            get
            {
                return Outputs.Count == Outputs.Capacity;
            }
        }
    }
}
