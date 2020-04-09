using System.IO;
using System.IO.Compression;


namespace GZipTestProject
{
    /// <summary>
    /// Takes a byte array and returns it compressed (again as a byte array).
    /// </summary>
    public class Compression
    {
        /// <summary>
        /// Takes a byte array and returns it compressed (again as a byte array).
        /// </summary>
        /// <param name="data">original byte array</param>
        /// <returns>compressed byte array</returns>
        public static byte[] Compress(byte[] data)     
        {
            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                zipStream.Write(data, 0, data.Length);   
                zipStream.Close();
                return compressedStream.ToArray();    // Converts zipped data from memory stream to a byte array.
            }
        }
    }
}
