using System;
using System.Globalization;
using System.IO;
using System.Windows;

namespace ExMachinaConversionLauncher.Models
{
    public class ErrorHandler
    {
        private static Window _mainindow;

        public ErrorHandler(Window mainindow)
        {
            _mainindow = mainindow;
        }

        public void CallErrorWindows(Exception ex, string exeptionLocate)
        {
            var errorMessage = String.Format("{2}: Application was crashad at {0} function with exeption: {1}",
                exeptionLocate, System.Environment.NewLine + ex.Message + " " + ex.InnerException, DateTime.Now);
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss",CultureInfo.InvariantCulture);
            File.WriteAllText("launcherError_" + dateTime + ".log", errorMessage);
            var errorWindow = new Error(_mainindow, errorMessage);
            errorWindow.ShowDialog();
        }
    }
}