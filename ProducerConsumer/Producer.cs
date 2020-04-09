using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;


namespace GZipTestProject
{
    /// <summary>
    /// Reads data from the file stream and inserts them into a queue in the form of an indexed data object.
    /// </summary>
    public abstract class Producer
    {
        private readonly object lockQueue;
        private readonly Queue<IndexedDataObject> queue;


        public Producer(Queue<IndexedDataObject> queue)
        {
            lockQueue = ProducerConsumer.GetLockQueue();
            this.queue = queue;
        }

        /// <summary>
        /// Reads data from a file and puts it into a queue for a consumer.
        /// </summary>
        public void ProduceData()
        {
            try
            {

                FileInfo inputFile = new FileInfo(Program.GetInputFile());
                using (FileStream inputFileStream = inputFile.OpenRead())
                {
                    ReadFromFileAndEnqueue(inputFileStream);

                    EnqueueObject(null);       // null object is attached to the end of the queue where there are no more bytes to read from the input file => signal the consumer to exit
                }
            }
            catch (DirectoryNotFoundException)
            {
                ErrorsChecker.SetError(new DirectoryNotFoundException("Error message: The input directory you're looking for doesn't exist."));
            }
            catch (DriveNotFoundException)
            {
                ErrorsChecker.SetError(new DriveNotFoundException("Error message: The input drive you're looking for doesn't exist."));
            }
            catch (FileNotFoundException)
            {
                ErrorsChecker.SetError(new FileNotFoundException("Error message: The input file you're looking for doesn't exist."));
            }
            catch (FileLoadException)
            {
                ErrorsChecker.SetError(new FileLoadException("Error message: The input file you're looking has been found but it can't be loaded."));
            }
            catch (PathTooLongException)
            {
                ErrorsChecker.SetError(new PathTooLongException("Error message: The input path is longer or fully qualified file name is longer than the system-defined maximum length."));
            }
            catch (UnauthorizedAccessException)
            {
                ErrorsChecker.SetError(new UnauthorizedAccessException("Error message: Access to the input directory or file was denied."));
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
        /// Reads from file and puts elements in a queue
        /// </summary>
        /// <param name="inputFileStream">input file</param>
        protected abstract void ReadFromFileAndEnqueue(FileStream inputFileStream);

        /// <summary>
        /// puts the indexed data object into a queue
        /// </summary>
        /// <param name="data"></param>
        protected void EnqueueObject(IndexedDataObject data)
        {
            lock (lockQueue)
            {
                while (queue.Count >= Settings.maxQueueSize)
                {
                    Monitor.Wait(lockQueue);
                }

                queue.Enqueue(data);

                if (queue.Count == 1)
                {
                    // wake up any blocked dequeue, i.e. the consumer threads
                    Monitor.PulseAll(lockQueue);
                }
            }
        }
    }
}
