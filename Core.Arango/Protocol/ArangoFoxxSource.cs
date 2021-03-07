using System.IO;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   Foxx service installation source
    /// </summary>
    public class ArangoFoxxSource
    {
        private ArangoFoxxSource()
        {
        }

        internal string Url { get; private set; }
        internal string JavaScript { get; private set; }
        internal Stream ZipArchive { get; private set; }

        // TODO: Broken?
        // public static ArangoFoxxSource FromUrl(string url) => new ArangoFoxxSource {Url = url};

        /// <summary>
        ///   Foxx service from zip archive
        /// </summary>
        public static ArangoFoxxSource FromZip(Stream zip) => new ArangoFoxxSource {ZipArchive = zip};

        /// <summary>
        ///    Foxx service from single javascript file
        /// </summary>
        public static ArangoFoxxSource FromJavaScript(string script) => new ArangoFoxxSource {JavaScript = script};
    }
}