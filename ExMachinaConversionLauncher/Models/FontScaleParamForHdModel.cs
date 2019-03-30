namespace ExMachinaConversionLauncher.Models
{
    public class FontScaleParamForHdModel
    {
        public readonly double ScaleFactor;
        public readonly int WndFontSize;
        public readonly int MicAndTooltipFontSize;

        public FontScaleParamForHdModel(double scaleFactor, int wndFontSize, int micAndTooltipFontSize)
        {
            ScaleFactor = scaleFactor;
            WndFontSize = wndFontSize;
            MicAndTooltipFontSize = micAndTooltipFontSize;
        }
    }
}
