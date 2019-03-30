using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Xml;

namespace ExMachinaConversionLauncher.Services
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

        private readonly string _pathToMainDirectory = ((App)Application.Current).PathToMainDirectory;
        private readonly ErrorHandler _errorHandler;

        public GameSettingsService(ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }


        internal void GetDataFromFile()
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

        internal Dictionary<string, string> PrepareSettingsParameters()
        {
            try
            {
                var settingsParameters = new Dictionary<string, string>()
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
                return settingsParameters;
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "PrepareSettingsParameters");
                return new Dictionary<string, string>();
            }
        }
    }
}
