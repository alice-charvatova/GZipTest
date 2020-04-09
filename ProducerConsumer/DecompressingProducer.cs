using System.Collections.Generic;
using System.IO;

namespace GZipTestProject
{
    /// <summary>
    /// Reads (deserializes) indexed data objects from the file stream and inserts them into a queue.
    /// </summary>
    public class DecompressingProducer : Producer
    {
        public DecompressingProducer(Queue<IndexedDataObject> queue) : base(queue)
        {
        }

        /// <summary>
        /// Reads (deserializes) indexed data objects wth compressed byte arrays from an input file and inserts them to the queue.
        /// </summary>
        /// <param name="inputFileStream">input file</param>
        protected override void ReadFromFileAndEnqueue(FileStream inputFileStream)
        {
            while (inputFileStream.Position != inputFileStream.Length)
            {
                IndexedDataObject compressedDataObject = Deserialization.Deserialize(inputFileStream);
                EnqueueObject(compressedDataObject);   // put the data block into the queue
            }
        }
   
    }
}
