using System;
using System.Globalization;
using System.IO;

namespace ExMachinaConversionLauncher.Services
{
    public class ErrorHandler
    {
        public void CallErrorWindows(Exception ex, string exceptionLocate)
        {
            var errorMessage =
                $"{DateTime.Now}: Application was crashed at {exceptionLocate} function with exception: {Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.InnerException}{Environment.NewLine}{ex.StackTrace}";
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss",CultureInfo.InvariantCulture);
            File.WriteAllText("launcherError_" + dateTime + ".log", errorMessage);
            var errorWindow = new Error(errorMessage);
            errorWindow.ShowDialog();
        }
    }
}