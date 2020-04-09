using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace GZipTestProject
{
    /// <summary>
    /// Serializes a compressed data object to the given file stream.
    /// </summary>
    public class Serialization
    {
        /// <summary>
        /// Serializes a compressed data object to the given file stream.
        /// </summary>
        /// <param name="compressedDataObject">object with a compressed byte[] and corresponding index</param>
        /// <param name="outputStream">output file stream</param>
        public static void Serialize(IndexedDataObject compressedDataObject, FileStream outputStream)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(outputStream, compressedDataObject);
        }
    }
}
 
