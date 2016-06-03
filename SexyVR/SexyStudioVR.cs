using IllusionPlugin;
using System;
using UnityEngine;
using VRGIN.Controls;
using VRGIN.Core;
using VRGIN.Helpers;

namespace SexyVR {
    class SexyStudioVR : IEnhancedPlugin {

        public static readonly string _StudioExecutable = "SexyBeachStudio_32";
        public static readonly string _MaingameExecutable = "SexyBeachPR_32";

        public string[] Filter {
            get {
                return new string[] { _StudioExecutable, _MaingameExecutable };
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

        private IShortcut shortcutLogCameras = new KeyboardShortcut(new KeyStroke("F8"), LogCameras);

        public void OnApplicationQuit() {
        }

        public void OnApplicationStart() {
            if (Environment.CommandLine.Contains("--vr")) {
                Logger.Info("started");
                var manager = VRManager.Create<SexyStudioInterpreter>(new SexyStudioContext());
                manager.SetMode<SexyStudioSeatedMode>();
                vrEnabled = true;
            }
            if (Environment.CommandLine.Contains("--verbose")) {
                Logger.Level = Logger.LogMode.Debug;
            }
        }

        public static void LogCameras() {
            Logger.Info("Dumping cameras");
            foreach (Camera cam in GameObject.FindObjectsOfType<Camera>()) {
                Logger.Info("{0} ({1})", cam, cam.name);
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
            shortcutLogCameras.Evaluate();

            if (vrEnabled) {
                FixCameraTarget();
                FixCanvasLayer();
            }
        }

        private void FixCameraTarget() {
            // The target is always set to the camera transform, even in modes like Straight, Divert and Animation.
            // TODO in the main game this get's spammed all over. Someone resets the target we just set?
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
                if (canvas.name.Equals("AnimeControlCanvas") // Studio
                    || canvas.name.Equals("HAnimeControlCanvas") // Studio
                    ) {
                    canvas.gameObject.layer = LayerMask.NameToLayer("UI");
                }
            }
        }
    }
}
