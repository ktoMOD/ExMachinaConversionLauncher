using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Xml;

namespace ExMachinaConversionLauncher.Services
{
    class UserSettingsService
    {
        public double LsViewDistanceDivider { get; set; }
        public int MultiSamplesNum { get; set; }
        public int PostEffectBloom { get; set; }

        public int TexturesFilter { get; set; }
        public string ShaderMacro1 { get; set; }

        public bool DsShadows { get; set; }
        public int DetShadowTexSz { get; set; }
        public double ShadowBlurCoeff { get; set; }
        public int LgtShadowTexSz { get; set; }

        public double GrassDrawDist { get; set; }
        public int WaterQuality { get; set; }
        public double Gamma { get; set; }

        public int DesiredHeight { get; set; }
        public int DesiredWidth { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public int Volume { get; set; }
        public int Volume2D { get; set; }
        public int Volume3D { get; set; }

        public double Fov { get; set; }
        public bool AutoPlayVideo { get; set; }
        public bool DoNotLoadMainmenuLevel { get; set; }
        public bool SwitchCameraAllow { get; set; }
        public int CamSpeed { get; set; }
        public int ProjectorsFarDist { get; set; }

        private readonly string _pathToMainDirectory = ((App)Application.Current).PathToMainDirectory;
        private readonly ErrorHandler _errorHandler;

        public UserSettingsService(ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }


        public void GetDataFromFile()
        {
            try
            {
                using (var xmlFile = new FileStream($@"{_pathToMainDirectory}\data\config.cfg", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var xDoc = new XmlDocument();
                    xDoc.Load(xmlFile);

                    var xmlNode = xDoc.GetElementsByTagName("config");
                    if (xmlNode.Count == 0)
                    {
                        throw new Exception("Launcher.config tag <config> could not be empty.");
                    }

                    var xmlAttributeCollection = xmlNode[0].Attributes;

                    if (xmlAttributeCollection == null) return;

                    LsViewDistanceDivider = Convert.ToDouble(xmlAttributeCollection["lsViewDistanceDivider"].Value, CultureInfo.InvariantCulture);
                    MultiSamplesNum = Convert.ToInt32(xmlAttributeCollection["r_multiSamplesNum"].Value, CultureInfo.InvariantCulture);
                    PostEffectBloom = Convert.ToInt32(xmlAttributeCollection["g_postEffectBloom"].Value, CultureInfo.InvariantCulture);
                    TexturesFilter = Convert.ToInt32(xmlAttributeCollection["g_texturesFilter"].Value, CultureInfo.InvariantCulture);
                    ShaderMacro1 = xmlAttributeCollection["shaderMacro1"].Value;
                    DsShadows = Convert.ToBoolean(xmlAttributeCollection["dsShadows"].Value, CultureInfo.InvariantCulture);
                    DetShadowTexSz = Convert.ToInt32(xmlAttributeCollection["detShadowTexSz"].Value, CultureInfo.InvariantCulture);
                    ShadowBlurCoeff = Convert.ToDouble(xmlAttributeCollection["g_shadowBlurCoeff"].Value, CultureInfo.InvariantCulture);
                    LgtShadowTexSz = Convert.ToInt32(xmlAttributeCollection["lgtShadowTexSz"].Value, CultureInfo.InvariantCulture);
                    GrassDrawDist = Convert.ToDouble(xmlAttributeCollection["g_grassDrawDist"].Value, CultureInfo.InvariantCulture);
                    WaterQuality = Convert.ToInt32(xmlAttributeCollection["r_waterQuality"].Value, CultureInfo.InvariantCulture);
                    Gamma = Convert.ToDouble(xmlAttributeCollection["gammaGamma"].Value, CultureInfo.InvariantCulture);
                    DesiredHeight = Convert.ToInt32(xmlAttributeCollection["r_desiredHeight"].Value, CultureInfo.InvariantCulture);
                    DesiredWidth = Convert.ToInt32(xmlAttributeCollection["r_desiredWidth"].Value, CultureInfo.InvariantCulture);
                    Height = Convert.ToInt32(xmlAttributeCollection["r_height"].Value, CultureInfo.InvariantCulture);
                    Width = Convert.ToInt32(xmlAttributeCollection["r_width"].Value, CultureInfo.InvariantCulture);
                    Volume = Convert.ToInt32(xmlAttributeCollection["mus_Volume"].Value, CultureInfo.InvariantCulture);
                    Volume2D = Convert.ToInt32(xmlAttributeCollection["snd_2dVolume"].Value, CultureInfo.InvariantCulture);
                    Volume3D = Convert.ToInt32(xmlAttributeCollection["snd_3dVolume"].Value, CultureInfo.InvariantCulture);
                    Fov = Convert.ToDouble(xmlAttributeCollection["fov"].Value, CultureInfo.InvariantCulture);
                    AutoPlayVideo = Convert.ToBoolean(xmlAttributeCollection["autoPlayVideo"].Value, CultureInfo.InvariantCulture);
                    DoNotLoadMainmenuLevel = Convert.ToBoolean(xmlAttributeCollection["DoNotLoadMainmenuLevel"].Value, CultureInfo.InvariantCulture);
                    CamSpeed = Convert.ToInt32(xmlAttributeCollection["camSpeed"].Value, CultureInfo.InvariantCulture);
                    ProjectorsFarDist = Convert.ToInt32(xmlAttributeCollection["g_projectorsFarDist"].Value, CultureInfo.InvariantCulture);
                    SwitchCameraAllow = Convert.ToBoolean(xmlAttributeCollection["g_switchCameraAllow"].Value, CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "GetDataFromFile");
            }
        }
    }
}
