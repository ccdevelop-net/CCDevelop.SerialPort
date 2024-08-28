using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CCDevelop.SerialPort.Linux.Helpers {
  internal static class SttyExecution {
    #region PRIVATE - Constants
    /// <summary>
    /// The path to the stty executable
    /// </summary>
    private const string SttyPath = "/bin/stty";
    #endregion

    #region PUBLIC - Static Functions
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the stty executable is present on the system.
    /// </summary>
    /// <returns>Return <see cref="true"/> if platform is compatible </returns>
    public static bool IsPlatformCompatible() {
      return File.Exists(SttyPath);
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Calls stty with the list of stty arguments
    /// </summary>
    /// <param name="arguments"></param>
    /// <returns>Result string</returns>
    public static string SetTtyWithParam(IEnumerable<string> arguments) {
      // Concatinate all the argument strings into a single value that can be passed to the stty executable
      string argumentsString = string.Join(" ", arguments);

      // Call the stty executable with the stringle argument string and return the result produced by stty
      return CallStty(argumentsString);
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Calls the stty command with the parameters given.
    /// </summary>
    /// <param name="sttyParams"></param>
    /// <returns>Output string</returns>
    public static string CallStty(string sttyParams) {
      // Create the new process to run the the stty executable
      Process process = new Process();
      process.StartInfo.FileName = SttyPath;

      // Don't shell execute, we want to run the executable directly
      process.StartInfo.UseShellExecute = false;

      // Redirect stdout and stderr so we can capture it
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardError  = true;

      // Set the arguments to the given stty parameters
      process.StartInfo.Arguments = sttyParams;

      // Start the stty process
      process.Start();

      // Always call ReadToEnd() before WaitForExit() to avoid a deadlock
      // Read the entirety of stdout and stderr and wait for the streams to close
      Task<string> readOutput = process.StandardOutput.ReadToEndAsync();
      Task<string> readError  = process.StandardError.ReadToEndAsync();
      Task.WaitAll(readOutput, readError);
      string outputString = readOutput.Result;
      string errorString  = readError.Result;

      // Now that the stdout and stderr streams have closed,
      // wait for the process to exit.
      process.WaitForExit();

      // If stty produced anything on stderr, it means that stty had trouble setting the values
      // that were passed to it.
      //
      // If stderr was not empty, throw it as an exception
      if (errorString.Trim().Length > 0) {
        throw new InvalidOperationException(errorString);
      }

      // Return the stdout of stty
      return outputString;
    }
    //------------------------------------------------------------------------------------------------------------------
    #endregion
  }
}