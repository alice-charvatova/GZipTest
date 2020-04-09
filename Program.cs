using System;
using System.Diagnostics;


namespace GZipTestProject
{
    public class Program
    {
        private static ActionType actionType = ActionType.Undefined;
        private static string inputFile;
        private static string outputFile;


        public static int Main(string[] args)
        {
            
                // check the command line arguments passed to the application and set appropriate action type (compress/decompress) if everything is ok
                ErrorsChecker.CheckInputParameters(args);


                if ("compress".Equals(args[0].ToLower()))
                {
                    SetActionType(ActionType.Compress);
                }
                else
                {
                    SetActionType(ActionType.Decompress);
                }

                inputFile = args[1];
                outputFile = args[2];

                // Console.WriteLine("Version using all CPU cores, code refactored, abstract classes utilized");
                Console.WriteLine($"input file: {inputFile}");
                Console.WriteLine($"output file: {outputFile}");

                Stopwatch watch = new Stopwatch();
                watch.Start();

                ProducerConsumer producerConsumer = new ProducerConsumer();
                producerConsumer.RunProducerConsumer(actionType);

                watch.Stop();
                if (actionType == ActionType.Compress)
                {
                    Console.WriteLine($"Time needed for compression: {watch.Elapsed} s.");
                }
                else
                {
                    Console.WriteLine($"Time needed for decompression: {watch.Elapsed} s.");
                }
           
                Console.WriteLine("Application finished successfully.");
                return 0;

        }

        public static void SetActionType(ActionType p_actionType)
        {
            actionType = p_actionType;
        }

        public static ActionType GetActionType()
        {
            return actionType;
        }

        public static string GetInputFile()
        {
            return inputFile;
        }

        public static string GetOutputFile()
        {
            return outputFile;
        }

        
    }
}
