using IllusionPlugin;
using System;
using VRGIN.Core;

namespace SexyVR {
    class SexyStudioVR : IEnhancedPlugin {
        public string[] Filter {
            get {
                return new string[] { "SexyBeachStudio_32" };
            }
        }

        public string Name {
            get {
                return "SexyStudioVR";
            }
        }

        public string Version {
            get {
                return "0.1";
            }
        }

        private Boolean InitVr = false;

        public void OnApplicationQuit() {
        }

        public void OnApplicationStart() {
            if (Environment.CommandLine.Contains("--vr")) {
                var manager = VRManager.Create<SexyStudioInterpreter>(new SexyStudioContext());
                manager.SetMode<SexyStudioSeatedMode>();
            }
            if (Environment.CommandLine.Contains("--verbose")) {
                Logger.Level = Logger.LogMode.Debug;
            }
        }

        public void OnFixedUpdate() {
        }

        public void OnLateUpdate() {
        }

        public void OnLevelWasInitialized(int level) {
        }

        public void OnLevelWasLoaded(int level) {
        }

        public void OnUpdate() {
        }
    }
}
