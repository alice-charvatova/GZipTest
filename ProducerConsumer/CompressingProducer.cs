using System;
using System.Collections.Generic;
using System.IO;


namespace GZipTestProject
{
    /// <summary>
    /// Reads data in chunks (as byte[]) from the file stream and inserts them into a queue in the form of indexed data objects.
    /// </summary>
    public class CompressingProducer : Producer
    {
        public CompressingProducer(Queue<IndexedDataObject> queue) : base(queue)
        {
        }

        /// <summary>
        /// Reads data blocks (as byte[]) from an input file, assigns a unique number to each block (so that compressed data can later be saved to a file in the correct order)
        /// and inserts a new indexed data object (containing the input data block and the block number) to the queue.
        /// </summary>
        /// <param name="inputFileStream">input file</param>
        protected override void ReadFromFileAndEnqueue(FileStream inputFileStream)
        {
            byte[] block = new byte[Settings.blockSize];
            int bytesRead;
            uint i = 1;

            while ((bytesRead = (inputFileStream).Read(block, 0, Settings.blockSize)) > 0)     // read "blockSize" of bytes to a "block" (byte[])
            {
                if (bytesRead < block.Length)
                {
                    byte[] block2 = new byte[bytesRead];
                    Array.Copy(block, block2, bytesRead);
                    block = block2;
                }

                IndexedDataObject indexedDataObject = new IndexedDataObject(block, i);

                EnqueueObject(indexedDataObject);   // put the data block into the queue

                block = new byte[Settings.blockSize];

                i++;
            }
        }
    }
}
