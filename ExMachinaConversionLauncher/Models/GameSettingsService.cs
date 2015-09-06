using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace ExMachinaConversionLauncher.Models
{
    class GameSettingsService
    {
        internal double LsViewDistanceDivider { get; set; }
        internal int MultiSamplesNum { get; set; }
        internal int PostEffectBloom { get; set; }

        internal int TexturesFilter { get; set; }
        internal string ShaderMacro1 { get; set; }

        internal bool DsShadows { get; set; }
        internal int DetShadowTexSz { get; set; }
        internal double ShadowBlurCoeff { get; set; }
        internal int LgtShadowTexSz { get; set; }

        internal double GrassDrawDist { get; set; }
        internal int WaterQuality { get; set; }
        internal double Gamma { get; set; }

        internal int DesiredHeight { get; set; }
        internal int DesiredWidth { get; set; }
        internal int Height { get; set; }
        internal int Width { get; set; }

        internal int Volume { get; set; }
        internal int Volume2D { get; set; }
        internal int Volume3D { get; set; }

        internal double Fov { get; set; }
        internal bool AutoPlayVideo { get; set; }
        internal bool DoNotLoadMainmenuLevel { get; set; }
        internal bool SwitchCameraAllow { get; set; }
        internal int CamSpeed { get; set; }
        internal int ProjectorsFarDist { get; set; }

        private readonly ErrorHandler _errorHandler;

        public GameSettingsService(ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }


        internal void GetDataFromFile()
        {
            try
            {
                var gameConfig = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\data\config.cfg");
                foreach (var line in gameConfig)
                {

                    string value;
                    switch (GetKey(line))
                    {
                        case "lsViewDistanceDivider":
                            value = GetValue(line);
                            LsViewDistanceDivider = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                            break;
                        case "r_multiSamplesNum":
                            value = GetValue(line);
                            MultiSamplesNum = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case "g_postEffectBloom":
                            value = GetValue(line);
                            PostEffectBloom = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case "g_texturesFilter":
                            value = GetValue(line);
                            TexturesFilter = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case "shaderMacro1":
                            value = GetValue(line);
                            ShaderMacro1 = value;
                            break;
                        case "dsShadows":
                            value = GetValue(line);
                            DsShadows = BooleanValue(value);
                            break;
                        case "detShadowTexSz":
                            value = GetValue(line);
                            DetShadowTexSz = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case "g_shadowBlurCoeff":
                            value = GetValue(line);
                            ShadowBlurCoeff = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                            break;
                        case "lgtShadowTexSz":
                            value = GetValue(line);
                            LgtShadowTexSz = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case "g_grassDrawDist":
                            value = GetValue(line);
                            GrassDrawDist = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                            break;
                        case "r_waterQuality":
                            value = GetValue(line);
                            WaterQuality = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case "gammaGamma":
                            value = GetValue(line);
                            Gamma = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                            break;
                        case "r_desiredHeight":
                            value = GetValue(line);
                            DesiredHeight = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case "r_desiredWidth":
                            value = GetValue(line);
                            DesiredWidth = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case "r_height":
                            value = GetValue(line);
                            Height = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case "r_width":
                            value = GetValue(line);
                            Width = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case "mus_Volume":
                            value = GetValue(line);
                            Volume = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case "snd_2dVolume":
                            value = GetValue(line);
                            Volume2D = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case "snd_3dVolume":
                            value = GetValue(line);
                            Volume3D = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case "fov":
                            value = GetValue(line);
                            Fov = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                            break;
                        case "autoPlayVideo":
                            value = GetValue(line);
                            AutoPlayVideo = BooleanValue(value);
                            break;
                        case "DoNotLoadMainmenuLevel":
                            value = GetValue(line);
                            DoNotLoadMainmenuLevel = BooleanValue(value);
                            break;
                        case "camSpeed":
                            value = GetValue(line);
                            CamSpeed = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case "g_projectorsFarDist":
                            value = GetValue(line);
                            ProjectorsFarDist = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case "g_switchCameraAllow":
                            value = GetValue(line);
                            SwitchCameraAllow = BooleanValue(value);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "GetDataFromFile");
            }
        }

        private string GetKey(string line)
        {
            try
            {
                line = line.Trim('\t');
                var equally = line.IndexOf("=\"", StringComparison.InvariantCulture);
                return equally > 0 ? line.Substring(0, equally) : string.Empty;
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "GetKey");
                return null;
            }
        }

        private string GetValue(string line)
        {
            try
            {
                var equally = line.IndexOf("=\"", StringComparison.InvariantCulture);
                var firstQuote = line.IndexOf("\"", StringComparison.InvariantCulture);
                var lastQuote = line.IndexOf("\"", equally + 2, StringComparison.InvariantCulture);
                var value = line.Substring(firstQuote + 1, lastQuote - firstQuote - 1);
                return value;
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "GetValue");
                return null;
            }
        }

        private bool BooleanValue(string value)
        {
            try
            {
                switch (value.ToLower())
                {
                    case "yes":
                    case "true":
                    case "1":
                        return true;
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "BooleanValue");
                return false;
            }
        }

        internal void SaveSettingsToConfig()
        {
            try
            {
                var gameConfig = File.ReadAllText(Directory.GetCurrentDirectory() + @"\data\config.cfg");
                Dictionary<string, string> parametrsList = new Dictionary<string, string>();

                try
                {
                    parametrsList = new Dictionary<string, string>()
                    {
                        {"lsViewDistanceDivider",String.Format(CultureInfo.InvariantCulture, "{0:N2}", LsViewDistanceDivider)},
                        {"r_multiSamplesNum",Convert.ToString(MultiSamplesNum, CultureInfo.InvariantCulture)},
                        {"g_postEffectBloom",Convert.ToString(PostEffectBloom, CultureInfo.InvariantCulture)},
                        {"g_texturesFilter",Convert.ToString(TexturesFilter, CultureInfo.InvariantCulture)},
                        {"shaderMacro1",ShaderMacro1},
                        {"dsShadows",Convert.ToString(DsShadows, CultureInfo.InvariantCulture).ToLower()},
                        {"detShadowTexSz",Convert.ToString(DetShadowTexSz, CultureInfo.InvariantCulture)},
                        {"g_shadowBlurCoeff",String.Format(CultureInfo.InvariantCulture, "{0:N2}", ShadowBlurCoeff)},
                        {"lgtShadowTexSz",Convert.ToString(LgtShadowTexSz, CultureInfo.InvariantCulture)},
                        {"g_grassDrawDist",String.Format(CultureInfo.InvariantCulture, "{0:N2}", GrassDrawDist)},
                        {"r_waterQuality",Convert.ToString(WaterQuality, CultureInfo.InvariantCulture)},
                        {"gammaGamma",String.Format(CultureInfo.InvariantCulture, "{0:N2}", Gamma)},
                        {"r_desiredHeight",Convert.ToString(DesiredHeight, CultureInfo.InvariantCulture)},
                        {"r_desiredWidth",Convert.ToString(DesiredWidth, CultureInfo.InvariantCulture)},
                        {"r_height",Convert.ToString(Height, CultureInfo.InvariantCulture)},
                        {"r_width",Convert.ToString(Width, CultureInfo.InvariantCulture)},
                        {"mus_Volume",Convert.ToString(Volume, CultureInfo.InvariantCulture)},
                        {"snd_2dVolume",Convert.ToString(Volume2D, CultureInfo.InvariantCulture)},
                        {"snd_3dVolume",Convert.ToString(Volume3D, CultureInfo.InvariantCulture)},
                        {"fov",String.Format(CultureInfo.InvariantCulture, "{0:N2}", Fov)},
                        {"autoPlayVideo",Convert.ToString(AutoPlayVideo, CultureInfo.InvariantCulture).ToLower()},
                        {"DoNotLoadMainmenuLevel",Convert.ToString(DoNotLoadMainmenuLevel, CultureInfo.InvariantCulture).ToLower()},
                        {"camSpeed",Convert.ToString(CamSpeed, CultureInfo.InvariantCulture)},
                        {"g_projectorsFarDist",Convert.ToString(ProjectorsFarDist, CultureInfo.InvariantCulture)},
                        {"g_switchCameraAllow",Convert.ToString(SwitchCameraAllow, CultureInfo.InvariantCulture).ToLower()}
                    };
                }
                catch (Exception ex)
                {
                    _errorHandler.CallErrorWindows(ex, "SaveSettingsToConfig - parametrsList");
                }

                foreach (var parametr in parametrsList)
                {
                    var startIndex = gameConfig.IndexOf(parametr.Key, StringComparison.InvariantCulture) + parametr.Key.Length + 2;
                    var endIndex = gameConfig.IndexOf("\"", startIndex, StringComparison.InvariantCulture);

                    var gameConfigStringBuilder = new StringBuilder(gameConfig);
                    gameConfigStringBuilder.Remove(startIndex, endIndex - startIndex);
                    gameConfigStringBuilder.Insert(startIndex, parametr.Value);
                    gameConfig = gameConfigStringBuilder.ToString();
                }

                File.WriteAllText(Directory.GetCurrentDirectory() + @"\data\config.cfg", gameConfig);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "SaveSettingsToConfig");
            }
        }

    }
}
