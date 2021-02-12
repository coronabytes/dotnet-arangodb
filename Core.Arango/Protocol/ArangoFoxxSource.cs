using System.IO;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   Foxx service installation source
    /// </summary>
    public class ArangoFoxxSource
    {
        internal ArangoFoxxSource()
        {

        }

        internal string Url { get; set; }
        internal string JavaScript { get; set; }
        internal Stream ZipArchive { get; set; }

        // TODO: Broken?
        // public static ArangoFoxxSource FromUrl(string url) => new ArangoFoxxSource {Url = url};
        public static ArangoFoxxSource FromZip(Stream zip) => new ArangoFoxxSource {ZipArchive = zip};
        public static ArangoFoxxSource FromJavaScript(string script) => new ArangoFoxxSource {JavaScript = script};
    }
}