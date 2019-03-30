using System;
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

                    LsViewDistanceDivider = Convert.ToDouble(xmlAttributeCollection["lsViewDistanceDivider"].Value);
                    MultiSamplesNum = Convert.ToInt32(xmlAttributeCollection["r_multiSamplesNum"].Value);
                    PostEffectBloom = Convert.ToInt32(xmlAttributeCollection["g_postEffectBloom"].Value);
                    TexturesFilter = Convert.ToInt32(xmlAttributeCollection["g_texturesFilter"].Value);
                    ShaderMacro1 = xmlAttributeCollection["shaderMacro1"].Value;
                    DsShadows = Convert.ToBoolean(xmlAttributeCollection["dsShadows"].Value);
                    DetShadowTexSz = Convert.ToInt32(xmlAttributeCollection["detShadowTexSz"].Value);
                    ShadowBlurCoeff = Convert.ToDouble(xmlAttributeCollection["g_shadowBlurCoeff"].Value);
                    LgtShadowTexSz = Convert.ToInt32(xmlAttributeCollection["lgtShadowTexSz"].Value);
                    GrassDrawDist = Convert.ToDouble(xmlAttributeCollection["g_grassDrawDist"].Value);
                    WaterQuality = Convert.ToInt32(xmlAttributeCollection["r_waterQuality"].Value);
                    Gamma = Convert.ToDouble(xmlAttributeCollection["gammaGamma"].Value);
                    DesiredHeight = Convert.ToInt32(xmlAttributeCollection["r_desiredHeight"].Value);
                    DesiredWidth = Convert.ToInt32(xmlAttributeCollection["r_desiredWidth"].Value);
                    Height = Convert.ToInt32(xmlAttributeCollection["r_height"].Value);
                    Width = Convert.ToInt32(xmlAttributeCollection["r_width"].Value);
                    Volume = Convert.ToInt32(xmlAttributeCollection["mus_Volume"].Value);
                    Volume2D = Convert.ToInt32(xmlAttributeCollection["snd_2dVolume"].Value);
                    Volume3D = Convert.ToInt32(xmlAttributeCollection["snd_3dVolume"].Value);
                    Fov = Convert.ToDouble(xmlAttributeCollection["fov"].Value);
                    AutoPlayVideo = Convert.ToBoolean(xmlAttributeCollection["autoPlayVideo"].Value);
                    DoNotLoadMainmenuLevel = Convert.ToBoolean(xmlAttributeCollection["DoNotLoadMainmenuLevel"].Value);
                    CamSpeed = Convert.ToInt32(xmlAttributeCollection["camSpeed"].Value);
                    ProjectorsFarDist = Convert.ToInt32(xmlAttributeCollection["g_projectorsFarDist"].Value);
                    SwitchCameraAllow = Convert.ToBoolean(xmlAttributeCollection["g_switchCameraAllow"].Value);
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "GetDataFromFile");
            }
        }
    }
}
