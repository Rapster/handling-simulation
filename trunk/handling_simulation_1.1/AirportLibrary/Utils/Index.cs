using System;
using System.Drawing;

namespace Airport.Utils
{
    /// <summary>
    /// A 2d point-like structure, expanded with a modified constructor
    /// </summary>
    public struct Index
    {
        /// <summary>
        /// Initializes a new instance of the Index struct with the specified coordinates.
        /// </summary>
        /// <param name="x">The horizontal position of the point.</param>
        /// <param name="y">The vertical position of the point.</param>
        public Index( int x, int y )
            : this( )
        {
            X = x;
            Y = y;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the Index struct with the specified System.Drawing.Point.
        /// </summary>
        /// <param name="point">Represents an ordered pair of integer x- and y-coordinates that defines a point in a two-dimensional plane.</param>
        public Index( Point point )
            : this( )
        {
            X = point.X;
            Y = point.Y;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Creates an index object from a string. Allows either ',' or ';' as a separator.
        /// Used when reading point values from a file.
        /// </summary>
        /// <param name="point">A point in the "x;y" or "x,y" format</param>
        public Index( string point )
            : this( )
        {
            if ( ( point != null 
                 && !point.Contains( ";" )
                 && !point.Contains( "," ) ) 
                 || point == null )
            {
                throw new FormatException( "separator not found " );
            }

            char c = point.Contains( ";" )
                     ? ';'
                     : ',';

            string[ ] values = point.Split( new char[ ] { c } );

            if( values.Length != 2 )
            {
                throw new FormatException( );
            }

            X = Convert.ToInt32( values[ 0 ] );
            Y = Convert.ToInt32( values[ 1 ] );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Returns a string representation separated by a ';'
        /// </summary>
        /// <returns>A string representation</returns>
        public override string ToString( )
        {
            return X + ";" + Y;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Compare the equality between two Index instance
        /// </summary>
        /// <param name="lhs">First Index</param>
        /// <param name="rhs">Seconds Index</param>
        /// <returns>True if X and Y are equals each other, otherwise false</returns>
        public static bool operator ==( Index lhs, Index rhs )
        {
            return lhs.X == rhs.X
                   && lhs.Y == rhs.Y;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Compare the difference between two Index instance
        /// </summary>
        /// <param name="lhs">First Index</param>
        /// <param name="rhs">Second Index</param>
        /// <returns></returns>
        public static bool operator !=( Index lhs, Index rhs )
        {
            return !( lhs == rhs );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">object to compare</param>
        /// <returns>True if X and Y are equals each other, otherwise false</returns>
        public override bool Equals( object obj )
        {
            return this == (Index)obj;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode( )
        {
            return X.GetHashCode( ) ^ Y.GetHashCode( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the horizontal position of the point.
        /// </summary>
        public int X
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the vertical position of the point.
        /// </summary>
        public int Y
        {
            get;
            set;
        }

        // --------------------------------------------------------------------
    }
}
