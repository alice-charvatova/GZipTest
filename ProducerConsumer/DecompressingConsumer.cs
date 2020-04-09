using System.Collections.Generic;
using System.IO;


namespace GZipTestProject
{
    /// <summary>
    /// Retrieves indexed data objects from queue, decompresses their data and writes them as byte[] into the output file in the correct order based on indexes.
    /// </summary>
    public class DecompressingConsumer : Consumer
    {
        public DecompressingConsumer(Queue<IndexedDataObject> queue) : base(queue)
        {
        }

        /// <summary>
        /// Decompresses data inside the input indexedDataObject.
        /// </summary>
        /// <param name="dataObjectForDecompression">input data object with compressed byte[] and corresponding index</param>
        /// <returns>data object with byte[] and corresponding index</returns>
        protected override IndexedDataObject ProcessData(IndexedDataObject dataObjectForDecompression)
        {
            byte[] processedBlock = Decompression.Decompress(dataObjectForDecompression.DataBlock);
            dataObjectForDecompression.DataBlock = processedBlock;
            IndexedDataObject decompressedDataObject = dataObjectForDecompression;
            return decompressedDataObject;
        }

        /// <summary>
        /// Writes decompressed data from the given indexed data object to the output file (in correct order based on the data objects's index number) as byte[].
        /// </summary>
        /// <param name="outputDataObject">data object with byte[] to be written into the output file</param>
        /// <param name="outputFileStream">output file</param>
        protected override void WriteToFile(IndexedDataObject outputDataObject, FileStream outputFileStream)
        {
            outputFileStream.Write(outputDataObject.DataBlock, 0, outputDataObject.DataBlock.Length);
        }
    }
}
