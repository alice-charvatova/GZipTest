using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace GZipTestProject
{
    /// <summary>
    /// Deserializes a compressed data object from the given file stream.
    /// </summary>
    public class Deserialization
    {
        /// <summary>
        /// Deserializes a compressed data object from the given file stream.
        /// </summary>
        /// <param name="inputStream">input file stream</param>
        /// <returns>object with a compressed byte[] and corresponding index</returns>
        public static IndexedDataObject Deserialize(FileStream inputStream)
        {
            IFormatter formatter = new BinaryFormatter();
            IndexedDataObject compressedDataObject = (IndexedDataObject)formatter.Deserialize(inputStream);
            
            return compressedDataObject;
        }
    }
}
 
