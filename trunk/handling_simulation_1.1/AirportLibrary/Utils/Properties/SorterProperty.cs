using System.Collections.Generic;
using System.ComponentModel;
using Airport.Controls;
using Airport.Tools;

namespace Airport.Utils.Properties
{
    /// <summary>
    /// Sorter's property class used by the PropertyGrid 
    /// </summary>
    public class SorterProperty : AirportZoneProperty
    {
        /// <summary>
        /// Initialize a new SorterProperty
        /// </summary>
        /// <param name="sorter">Sorter object needed to display the proper characteristics</param>
        /// <param name="packages">Packages going through the specified sorter</param>
        /// <param name="ways">Way going through the specified sorter</param>
        public SorterProperty( Sorter sorter, List<Package> packages, List< Way > ways )
            : base( sorter, packages, ways )
        {
            State = sorter.IsFull
                    ? "Undefined"
                    : this.ways.Count > 0 
                    && ( this.ways[ 0 ].End as DestinationGate ).PlaneIsPresent
                    ? "Ready"
                    : "Waiting";

            IsFull = sorter.IsFull;

            Input = this.ways.Count > 0
                    ? this.ways[ 0 ].Start.Name
                    : "Undefined";
        }

        // -------------------------------------------------------------------

        /// <summary>
        /// Indicates if the sorter's ouputs are all booked
        /// </summary>
        [DescriptionAttribute( "Indicate if the all the ouput are booked by a conveyor" ),
        CategoryAttribute( "Sorter Properties" )]
        [ReadOnlyAttribute( true )]
        public bool IsFull
        {
            get;
            private set;
        }

        // -------------------------------------------------------------------

        /// <summary>
        /// Display the name of the input AirportZone
        /// </summary>
        [DescriptionAttribute( "Conveyor start element of the input sorter" ),
        CategoryAttribute( "Sorter Properties" )]
        [ReadOnlyAttribute( true )]
        public string Input
        {
            get;
            private set;
        }

        // -------------------------------------------------------------------

        /// <summary>
        /// Indicates the name of the sorter's ouputs
        /// </summary>
        [Browsable( true )]
        [TypeConverter( typeof( ZoneConverter ) )]
        [DescriptionAttribute( "Conveyor end elements of the outputs sorter" ),
        CategoryAttribute( "Sorter Properties" )]
        public string Outputs
        {
            get
            {
                return output = GlobalZones.idZones.Count > 0
                       ? GlobalZones.idZones[ 0 ]
                       : "Empty";
            }
            set
            {
                output = value;
            }
        }

        // --------------------------------------------------------------------

        private string output;
    }
}
