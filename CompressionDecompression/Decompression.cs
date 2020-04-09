using System.IO;
using System.IO.Compression;


namespace GZipTestProject
{
    /// <summary>
    /// Takes a byte array (with compressed data), decompress it and returns it as a byte array.
    /// </summary>
    public class Decompression
    {
        /// <summary>
        /// Takes a byte array (with compressed data), decompress it and returns it as a byte array.
        /// </summary>
        /// <param name="compressedData">compressed data byte array</param>
        /// <returns>decompressed byte array</returns>
        public static byte[] Decompress(byte[] compressedData) //Writes an input byte array into a memory stream, then GZipStream (for decompression), then into memory stream again for converting data into a byte array.
        {
            using (var compressedStream = new MemoryStream(compressedData))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }
    }



}
