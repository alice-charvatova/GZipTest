using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace GZipTestProject
{
    /// <summary>
    /// Reads index data objects from a queue, processes them and writes the resulting data to the file output stream.
    /// </summary>
    public abstract class Consumer 
    {
        private readonly object lockQueue;
        private readonly object lockOutputFile;
        private readonly Queue<IndexedDataObject> queue;

    
        public Consumer(Queue<IndexedDataObject> queue)
        {
            lockQueue = ProducerConsumer.GetLockQueue();
            lockOutputFile = ProducerConsumer.GetLockOutputFile();
            this.queue = queue;
        }

        /// <summary>
        /// Takes data objects from a queue, processes them (compression/decompression) and writes them to the output file.
        /// </summary>
        /// <param name="outputFileStreamObject">output file stream</param>
        public void ConsumeData(object outputFileStreamObject)  //takes data from a queue
        {
            try
            {

                FileStream outputFileStream = (FileStream)outputFileStreamObject;

                    while (true)    // keeps the consumer thread running
                    {
                        IndexedDataObject dataElementFromQueue = null;

                        lock (lockQueue)
                        {
                            if (queue.Count > 0)    // queue not empty
                            {
                                dataElementFromQueue = queue.Dequeue();

                                Monitor.PulseAll(lockQueue);    // data removed from a queue => wake up the producer to put more data into the queue

                                if (dataElementFromQueue == null)       // null object in queue means end of file reached, nothing more to read 
                                {
                                    ProducerConsumer.lastBlockRead = true;  // last block in the queue (there are no more bytes to read from the input file)
                                    return;     // Producer ended, all objects from the queue have been already processed => this thread ends
                            }
                        }
                            else
                            {
                                if (ProducerConsumer.lastBlockRead)
                                {
                                    return;     // Producer ended, all objects from the queue have been already processed (or are processed by other threads) => this thread ends
                                }
                                Monitor.Wait(lockQueue);
                            }
                        }

                    // data object was read from the queue => we can release the lock and enable access to the queue again to the producer or other consumer threads

                    if (dataElementFromQueue != null)
                        {
                            IndexedDataObject processedData = ProcessData(dataElementFromQueue);

                            lock (lockOutputFile)
                            {
                                // thread waits for previous data blocks to be written to the file before it can write its own data block to the file
                                while ((processedData.Index - 1) != ProducerConsumer.GetProcessedBlocksCount())
                                {
                                    Monitor.Wait(lockOutputFile);
                                }
   
                                WriteToFile(processedData, outputFileStream);
                                ProducerConsumer.IncrementProcessedBlocksCount();
                                Monitor.PulseAll(lockOutputFile);

                            }
                            
                        }
                    }
            }
            catch (DirectoryNotFoundException)
            {
                ErrorsChecker.SetError(new DirectoryNotFoundException("Error message: The output directory you're looking for doesn't exist."));
            }
            catch (DriveNotFoundException)
            {
                ErrorsChecker.SetError(new DriveNotFoundException("Error message: The output drive you're looking for doesn't exist."));
            }
            catch (PathTooLongException)
            {
                ErrorsChecker.SetError(new PathTooLongException("Error message: The output path is longer or fully qualified file name is longer than the system-defined maximum length."));
            }
            catch (UnauthorizedAccessException)
            {
                ErrorsChecker.SetError(new UnauthorizedAccessException("Error message: Access to the output directory was denied."));
            }
            catch (OutOfMemoryException)
            {
                ErrorsChecker.SetError(new OutOfMemoryException("Error message: There is not enough memory to continue the execution of the program."));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ErrorsChecker.SetError(new Exception("Error message: An error occured during the execution of the program."));
            }

        }

        /// <summary>
        /// Retrieves an indexed data object from the queue nad processes it (compresses or decompresses the data).
        /// </summary>
        /// <param name="indexedDataObject"></param>
        /// <returns>data object with compressed or decompressed byte[] and corresponding index</returns>
        protected abstract IndexedDataObject ProcessData(IndexedDataObject indexedDataObject);

        /// <summary>
        /// Writes the processed data into the output file.
        /// </summary>
        /// <param name="outputDataObject">data object to be written in the output file</param>
        /// <param name="outputFileStream">output file</param>
        protected abstract void WriteToFile(IndexedDataObject outputDataObject, FileStream outputFileStream);
    }
}
