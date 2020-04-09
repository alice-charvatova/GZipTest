using System;


namespace GZipTestProject
{   
    /// <summary>
    /// Wrapper class containing data (either compressed or decompressed) and an index ensuring correct order of data being written to the output file later on.
    /// </summary>
    
    [Serializable]
    public class IndexedDataObject
    {
        public byte[] DataBlock { get; set; }

        /// <summary>
        /// Index defines the precize order in which this data block should be written to the output file 
        /// (in order to be able to read the data from the file later on and reconstruct the original file data).
        /// </summary>
        public uint Index { get; set; }

        public IndexedDataObject(byte[] block, uint i)
        {
            DataBlock = block;
            Index = i;
        }

 
    }

}
