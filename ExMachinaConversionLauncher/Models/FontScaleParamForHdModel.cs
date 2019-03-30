namespace ExMachinaConversionLauncher.Models
{
    internal class FontScaleParamForHdModel
    {
        internal readonly double ScaleFactor;
        internal readonly int WndFontSize;
        internal readonly int MicAndTooltipFontSize;

        public FontScaleParamForHdModel(double scaleFactor, int wndFontSize, int micAndTooltipFontSize)
        {
            ScaleFactor = scaleFactor;
            WndFontSize = wndFontSize;
            MicAndTooltipFontSize = micAndTooltipFontSize;
        }
    }
}
