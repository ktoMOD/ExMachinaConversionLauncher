using System;
using System.Globalization;
using System.IO;

namespace ExMachinaConversionLauncher.Services
{
    public class ErrorHandler
    {
        public void CallErrorWindows(Exception ex, string exceptionLocate)
        {
            var errorMessage = string.Format("{2}: Application was crashed at {0} function with exception: {1}",
                exceptionLocate, Environment.NewLine + ex.Message + " " + ex.InnerException, DateTime.Now);
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss",CultureInfo.InvariantCulture);
            File.WriteAllText("launcherError_" + dateTime + ".log", errorMessage);
            var errorWindow = new Error(errorMessage);
            errorWindow.ShowDialog();
        }
    }
}