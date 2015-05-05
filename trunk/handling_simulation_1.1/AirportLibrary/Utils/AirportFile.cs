using System;
using System.Collections.Generic;
using Airport.Controls;
using System.Xml.Serialization;
using System.Xml;
using Airport.Utils.Factories;
using Airport.Tools;

namespace Airport.Utils
{
    /// <summary>
    /// XML file containing a list of Way, AirportZone and Package.
    /// </summary>
    [Serializable]
    public class AirportFile : IXmlSerializable
    {
        /// <summary>
        /// Initialize a new instance of AirportFile. Necessary for the deserialization.
        /// </summary>
        public AirportFile( )
            : this( new List<AirportZone>( ),
                    new List<Way>( ),
                    new List<Package>( ) )
        {
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Initialize a new instance of AirportFile. Necessary for the serialization.
        /// </summary>
        /// <param name="zones">Zones to serialize</param>
        /// <param name="ways">Ways to serialize, with the conveyor children</param>
        /// <param name="packages">Packages to serialize</param>
        public AirportFile( List<AirportZone> zones,
                            List<Way> ways,
                            List<Package> packages )
        {
            Zones = zones;
            Ways = ways;
            Packages = packages;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the set of AirportZone to serialize/unserialize
        /// </summary>
        public List<AirportZone> Zones
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the set of Way to serialize/unserialize
        /// </summary>
        public List<Way> Ways
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the set of Package to serialize/unserialize
        /// </summary>
        public List<Package> Packages
        {
            get;
            set;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// This method is reserved and should not be used. When implementing the
        /// IXmlSerializable interface, you should return Nothing (Nothing in Visual Basic)
        /// from this method, and instead, if specifying a custom schema is required, apply
        /// the XmlSchemaProviderAttribute to the class.
        /// </summary>
        /// <returns>An XmlSchema that describes the XML representation of the object that 
        /// is produced by the WriteXml method and consumed by the ReadXml method.</returns>
        public System.Xml.Schema.XmlSchema GetSchema( )
        {
            throw new NotImplementedException( );
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Generates an object AirportFile from its XML representation.
        /// </summary>
        /// <example>
        /// <code lang="c#">
        /// FileStream fs = null;
        ///
        /// using ( fs = new FileStream( filename, FileMode.Open ) )
        /// {
        ///     XmlSerializer serializer = new XmlSerializer( typeof( AirportFile ) );
        ///     AirportFile file = serializer.Deserialize( fs ) as AirportFile;
        /// }
        /// </code>
        /// </example>
        /// <param name="reader">Represents a reader that provides fast access,
        /// non-cached, forward-only data to XML.</param>
        public void ReadXml( System.Xml.XmlReader reader )
        {
            Zones.Clear( );
            Ways.Clear( );
            Packages.Clear( );

            reader.Read( ); //Skip AirportFile
            reader.Read( );

            while ( reader.MoveToContent( ) == XmlNodeType.Element )
            {
                AirportFactory factory =
                    AirportFactory.GetFactory( Type.GetType( reader.Name.ToString( ) ) );

                Index pos = new Index( reader[ "Position" ] );
                reader.Read( );
                string image = reader.ReadElementString( "Image" );

                AirportZone zone = factory.CreateZone( null, pos, image, true );
                
                if ( zone is DestinationGate )
                {
                    ( zone as DestinationGate ).PlaneIsPresent =
                        Convert.ToBoolean( reader.ReadElementString( "PlaneIsPresent" ) );
                }

                reader.Read( );
                Zones.Add( zone );
            }

            reader.Read( );
            reader.Read( );

            while ( reader.MoveToContent( ) == XmlNodeType.Element )
            {
                List<Conveyor> conveyors = new List<Conveyor>( );

                while ( reader.MoveToContent( ) == XmlNodeType.Element
                        && reader.Read( ) )
                {
                    string str = reader.ReadElementString( );

                    Conveyor conveyor = new Conveyor( );
                    conveyor.Start = Zones.Find( delegate( AirportZone zone )
                    {
                        return zone.Position == new Index( str );
                    }
                    );

                    if ( conveyor.Start == null )
                    {
                        conveyor.Start = new AirportZone( null,
                                                          new Index( str ),
                                                          String.Empty );
                    }

                    str = reader.ReadElementString( );
                    conveyor.End = Zones.Find( delegate( AirportZone zone )
                    {
                        return zone.Position == new Index( str );
                    }
                    );

                    if ( conveyor.End == null )
                    {
                        conveyor.End = new AirportZone( null,
                                                        new Index( str ),
                                                        String.Empty );
                    }

                    if ( !Zones.Contains( conveyor.Start ) )
                    {
                        Zones.Add( conveyor.Start );
                    }

                    if ( !Zones.Contains( conveyor.End ) )
                    {
                        Zones.Add( conveyor.End );
                    }

                    conveyors.Add( conveyor );
                    reader.Read( );
                }

                Ways.Add( new Way( conveyors ) );
                reader.Read( );
            }

            reader.Read( );
            reader.Read( );

            while ( reader.MoveToContent( ) == XmlNodeType.Element )
            {
                List<Way> ways = Ways.FindAll( delegate( Way tmp )
                {
                    return tmp.Start.Position == new Index( reader[ "Start" ] )
                           && tmp.End.Position == new Index( reader[ "End" ] );
                }
                );

                Way way = Airport.Controls.Ways.ShortestWay( ways );
                reader.Read( );

                int amount = reader.ReadElementContentAsInt( );
                reader.Read( );

                Packages.Add( new Package( amount, way ) );
            }

            // Update destinations
            foreach ( Way way in Ways )
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
        /// Converts an object AiportFile into its XML representation.
        /// </summary>
        /// <example>
        /// <code lang="c#">
        /// AirportFile file = new AirportFile( zones, ways, packages );
        /// using ( fs = new FileStream( filename, FileMode.Create ) )
        /// {
        ///     XmlSerializer serializer = new XmlSerializer( typeof( AirportFile ) );
        ///     serializer.Serialize( fs, file );
        /// }
        /// </code>
        /// </example>
        /// <param name="writer">Represents a writer that provides a fast, no cache,
        /// forward only to generate data streams or files containing XML data.</param>
        public void WriteXml( System.Xml.XmlWriter writer )
        {
            writer.WriteStartElement( "Zones" );

            foreach ( AirportZone zone in Zones )
            {
                writer.WriteStartElement( zone.GetType( ).ToString( ) );
                writer.WriteAttributeString( "Position", zone.Position.ToString( ) );
                writer.WriteElementString( "Image", zone.Background );

                if ( zone is DestinationGate )
                {
                    writer.WriteElementString( "PlaneIsPresent",
                                               ( zone as DestinationGate ).PlaneIsPresent.ToString( ) );
                }

                writer.WriteEndElement( );
            }

            writer.WriteEndElement( );
            writer.WriteStartElement( "Ways" );

            foreach ( Way way in Ways )
            {
                writer.WriteStartElement( "Way" );

                foreach ( Conveyor c in way.Conveyors )
                {
                    writer.WriteStartElement( "Conveyor" );
                    writer.WriteElementString( "Start",
                                               c.Start.Position.ToString( ) );

                    writer.WriteElementString( "End",
                                               c.End.Position.ToString( ) );
                    writer.WriteEndElement( );
                }

                writer.WriteEndElement( );
            }

            writer.WriteEndElement( );
            writer.WriteStartElement( "Packages" );

            foreach ( Package pkg in Packages )
            {
                writer.WriteStartElement( "Package" );
                writer.WriteAttributeString( "Start",
                                             pkg.Start.Position.ToString( ) );

                writer.WriteAttributeString( "End",
                                             pkg.End.Position.ToString( ) );

                writer.WriteElementString( "Amount",
                                           pkg.Luggages.Capacity.ToString( ) );

                writer.WriteEndElement( );
            }

            writer.WriteEndElement( );
        }

        // --------------------------------------------------------------------
    }
}
