using System;


namespace GZipTestProject
{
    /// <summary>
    /// Checks number of input parameters and type of the first parameter (action type), also writes info about exceptions caught in Producer or Consumer. 
    /// </summary>
    public class ErrorsChecker
    {
        private static string errorMessage = null;
        public static Exception Error { get; set; }

        /// <summary>
        /// Checks number of input parameters and type of the first parameter (action type)
        /// </summary>
        /// <param name="args">input parameters</param>
        /// <returns>true if there's no problem with program input arguments, false otherwise</returns>
        public static bool CheckInputParameters(string[] args)
        {
            if (args.Length != 3)
            {
                errorMessage = $"Error message: Incorrect number of arguments passed to the program{Environment.NewLine}";
                errorMessage += GetRunInputArgumentsInfo();
                SetError(new InvalidOperationException(errorMessage));
            }

            if (!"compress".Equals(args[0].ToLower()) && !"decompress".Equals(args[0].ToLower()))
            {
                errorMessage = $"Error message: Program started with an incorrect action argument{Environment.NewLine}";
                errorMessage += GetRunInputArgumentsInfo();
                SetError(new InvalidOperationException(errorMessage));
            }


            return true;
        }

        /// <summary>
        /// Provides info about input parametres requirements.
        /// </summary>
        /// <returns>info about input parametres requirements</returns>
        private static string GetRunInputArgumentsInfo()
        {
            string argumentInfo = $"The application takes 3 agguments and is supposed to be executed in the following way.{Environment.NewLine}";
            argumentInfo += $"1) for compressing run: {Environment.NewLine}";
            argumentInfo += $"GZipTest.exe compress [original file name] [archive file name]{Environment.NewLine}";
            argumentInfo += $"2) for decompressing run:{Environment.NewLine}";
            argumentInfo += "GZipTest.exe compress [original file name] [archive file name]";
            return argumentInfo;
        }

        /// <summary>
        /// Writes info about exceptions caught in Producer or Consumer threads. If an exception occurs, program is terminated and returns error code (1).
        /// </summary>
        /// <param name="e">exeption caught in Producer or Consumer threads</param>
        public static void SetError(Exception e)
        {
            Error = e;
            Console.WriteLine(Error.Message);
            Console.WriteLine("Execution of the program failed.");
            Environment.Exit(1);
           
        }
    }
}
