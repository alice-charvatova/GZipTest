namespace GZipTestProject
{
    public class Settings
    {
        // size of a data block being read from the input file in order to be compressed
        public const int blockSize = 1024 * 1024;

        // max number of objects allowed in the queue
        public const int maxQueueSize = 30;
    }
}
