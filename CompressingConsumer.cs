using System.Collections.Generic;
using System.IO;


namespace GZipTestProject
{
    /// <summary>
    /// Reads indexed data objects from the queue, compresses their data and writes (serializes) the resulting data objects in the correct order to the output file.
    /// </summary>
    public class CompressingConsumer : Consumer
    {
        public CompressingConsumer(Queue<IndexedDataObject> queue) : base(queue)
        {
        }

        /// <summary>
        /// Compresses data inside the input indexedDataObject.
        /// </summary>
        /// <param name="dataObjectForCompression">input data object with byte[] and corresponding index</param>
        /// <returns>data object with compressed byte[] and corresponding index</returns>
        protected override IndexedDataObject ProcessData(IndexedDataObject dataObjectForCompression)
        {
            byte[] processedBlock = Compression.Compress(dataObjectForCompression.DataBlock);
            dataObjectForCompression.DataBlock = processedBlock;
            IndexedDataObject compressedDataObject = dataObjectForCompression;
            return compressedDataObject;
        }


        /// <summary>
        /// Writes compressed data from the given data object to a file (using object serialization).
        /// </summary>
        /// <param name="outputDataObject">data object to be written (serialized) into the output file</param>
        /// <param name="outputFileStream">output file</param>
        protected override void WriteToFile(IndexedDataObject outputDataObject, FileStream outputFileStream)
        {
            Serialization.Serialize(outputDataObject, outputFileStream);
        }
    }
}
