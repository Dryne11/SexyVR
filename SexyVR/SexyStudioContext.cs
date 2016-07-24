using System;
using UnityEngine;
using VRGIN.Core;
using VRGIN.Visuals;

namespace SexyVR {
    class SexyStudioContext : IVRManagerContext {

        private DefaultMaterialPalette _Materials;
        private VRSettings _Settings;
        private string[] _IgnoredCanvas = new string[0];

        public SexyStudioContext() {
            _Materials = new DefaultMaterialPalette();
            _Settings = VRSettings.Load<VRSettings>("vr_settings.xml");
            // XXX The only available shader wtf??
            _Materials.StandardShader = Shader.Find("Diffuse");
            //_Materials.StandardShader = Shader.Find("Marmoset/Specular IBL");
            Logger.Info("StandardShader is {0}", _Materials.StandardShader);
        }

        public string GuiLayer {
            get {
                return "Ignore Raycast";
            }
        }

        public string[] IgnoredCanvas {
            get {
                return _IgnoredCanvas;
            }
        }

        public IMaterialPalette Materials {
            get {
                return _Materials;
            }
        }

        public Color PrimaryColor {
            get {
                return Color.cyan;
            }
        }

        public VRSettings Settings {
            get {
                return _Settings;
            }
        }

        public int UILayerMask {
            get {
                return LayerMask.GetMask(UILayer);
            }
        }

        public string UILayer {
            get {
                return "UI";
            }
        }

        public bool SimulateCursor {
            get {
                return true;
            }
        }

        public bool GUIAlternativeSortingMode {
            get {
                // Canvas that overlap other canvas (exit dialog, scene load gallery) are not fully operable if
                // we use the alternative sorting mode.
                return false;
            }
        }

        public string InvisibleLayer {
            get {
                return "ScreenShot";
            }
        }

        public Type VoiceCommandType {
            get {
                return typeof(SexyVoiceCommand);
            }
        }
    }
}