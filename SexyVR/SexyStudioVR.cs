using IllusionPlugin;
using System;
using UnityEngine;
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

        private bool vrEnabled = false;

        public void OnApplicationQuit() {
        }

        public void OnApplicationStart() {
            if (Environment.CommandLine.Contains("--vr")) {
                var manager = VRManager.Create<SexyStudioInterpreter>(new SexyStudioContext());
                manager.SetMode<SexyStudioSeatedMode>();
                vrEnabled = true;
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
            if (vrEnabled) {
                FixCameraTarget();
                FixCanvasLayer();
                //LogCameraProperties();
            }
        }

        private void LogCameraProperties() {
            Camera studioCam = (Camera) GameObject.FindGameObjectWithTag("Camera3D").GetComponent<Camera>();
            SteamVR_Camera vrCam = VRCamera.Instance.SteamCam;
            Logger.Info("rotation - vr={0} studio={1}", vrCam.transform.rotation, studioCam.transform.rotation);
        }

        private void FixCameraTarget() {
            // The target is always set to the camera transform, even in modes like Straight, Divert and Animation.
            foreach (NeckLookController neck in GameObject.FindObjectsOfType<NeckLookController>()) {
                if (neck.target != null && neck.target.gameObject != VRCamera.Instance.SteamCam.gameObject) {
                    neck.target = VRCamera.Instance.SteamCam.transform;
                    Logger.Info("Fixed neck target");
                }
            }
            foreach (EyeLookController eye in GameObject.FindObjectsOfType<EyeLookController>()) {
                if (eye.target != null && eye.target.gameObject != VRCamera.Instance.SteamCam.gameObject) {
                    eye.target = VRCamera.Instance.SteamCam.transform;
                    Logger.Info("Fixed eye target");
                }
            }
        }

        private void FixCanvasLayer() {
            // These canvas are in the "Default" layer, which is not showing in the vr cam. Reassign them to
            // the "UI" layer where all other menu canvas are put.
            foreach (Canvas canvas in GameObject.FindObjectsOfType<Canvas>()) {
                if (canvas.name.Equals("AnimeControlCanvas") || canvas.name.Equals("HAnimeControlCanvas")) {
                    canvas.gameObject.layer = LayerMask.NameToLayer("UI");
                }
            }
        }
    }
}
