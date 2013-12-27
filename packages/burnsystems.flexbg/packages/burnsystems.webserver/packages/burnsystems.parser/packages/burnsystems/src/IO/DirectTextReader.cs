//-----------------------------------------------------------------------
// <copyright file="DirectTextReader.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.IO
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// Dieser TextReader wird benötigt, wenn ein Teil des Streams als Text
    /// behandelt werden soll. Wird dieser TextReader nicht mehr benötigt,
    /// so wird nicht der darunterliegende Stream geschlossen. 
    /// </summary>
    public class DirectTextReader
    {
        /// <summary>
        /// Stream to be used for direct streaming
        /// </summary>
        private Stream stream;

        /// <summary>
        /// Initializes a new instance of the DirectTextReader class.
        /// </summary>
        /// <param name="stream">Stream to be used</param>
        public DirectTextReader(Stream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        /// Reads one line, and returns it. This method moves the stream only to the place of 
        /// the end of the line. This means, that no buffering will be used. It will finish the 
        /// reading of line, if a \r was received. A \n at first position will be skipped
        /// </summary>
        /// <returns>Line, which was read</returns>
        public string ReadLine()
        {
            using (var memoryStream = new MemoryStream())
            {
                int currentByte;

                while ((currentByte = this.stream.ReadByte()) != -1)
                {
                    if (currentByte == 10)
                    {
                        continue;
                    }

                    if (currentByte == 13)
                    {
                        break;
                    }

                    memoryStream.WriteByte((byte)currentByte);
                }

                var bytes = memoryStream.GetBuffer();
                return Encoding.UTF8.GetString(bytes, 0, (int)memoryStream.Length);
            }
        }
    }
}
