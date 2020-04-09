using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;


namespace GZipTestProject
{
    /// <summary>
    /// Creates (and starts) producer and consumer threads.
    /// </summary>
    public class ProducerConsumer
    {
        private readonly static object lockQueue = new object();
        private readonly static object lockOutputFile = new object();

        internal static bool lastBlockRead = false;
        private static uint processedBlocksCount = 0;

        private Thread producerThread;
        private readonly Thread[] consumerThreadArray = new Thread[Environment.ProcessorCount];

        /// <summary>
        /// Starts producer and consumer threads.
        /// </summary>
        /// <param name="actionType">input action type parameter (compress/decompress)</param>
        public void RunProducerConsumer(ActionType actionType)
        {
            using (FileStream outputFileStream = new FileStream(Program.GetOutputFile(), FileMode.Create, FileAccess.Write))
            {
                Queue<IndexedDataObject> queue = new Queue<IndexedDataObject>();
                Producer producer;
                Consumer consumer;

                if (actionType == ActionType.Compress)
                {
                    // create a PRODUCER reading byte arrays from the input file and inserting them as indexed data objects into the queue
                    producer = new CompressingProducer(queue);

                    // create a CONSUMER reading indexed data objects from the queue, compressing them and writing (serializing) them into an output file
                    consumer = new CompressingConsumer(queue);
                }
                else
                {
                    // create a PRODUCER reading (deserializing) indexed data objects (with compressed data) from the input file and inserting them into the queue
                    producer = new DecompressingProducer(queue);

                    // create a CONSUMER reading indexed data objects (with compressed data) from the queue, decompressing them and writing them into an output file
                    consumer = new DecompressingConsumer(queue);
                }

                producerThread = new Thread(producer.ProduceData)
                {
                    IsBackground = true
                };
                producerThread.Start();

                for (int i = 0; i < Environment.ProcessorCount; i++)
                {
                    consumerThreadArray[i] = new Thread(consumer.ConsumeData)
                    {
                        IsBackground = true
                    };
                    consumerThreadArray[i].Start(outputFileStream);
                }

                if (producerThread != null) 
                { 
                    producerThread.Join(); 
                }

                for (int i = 0; i < Environment.ProcessorCount; i++)
                {
                    if (consumerThreadArray[i] != null)
                    {
                        consumerThreadArray[i].Join(); // wait for consumer threads to finish before the main thread may continue
                    }         
                }
            }
        }


        public static object GetLockQueue()
        {
            return lockQueue;
        }

        public static object GetLockOutputFile()
        {
            return lockOutputFile;
        }

        public static uint GetProcessedBlocksCount()
        {
            return processedBlocksCount;
        }

        public static void IncrementProcessedBlocksCount()
        {
            processedBlocksCount++;
        }
    }
}
