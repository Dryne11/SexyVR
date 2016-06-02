using IllusionPlugin;
using System;
using UnityEngine;
using VRGIN.Controls;
using VRGIN.Core;
using VRGIN.Helpers;

namespace SexyVR {
    class SexyStudioVR : IEnhancedPlugin {
        public string[] Filter {
            get {
                return new string[] { "SexyBeachStudio_32", "SexyBeachPR_32" };
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
        private bool vrStarted = false;
        // needed in the main game while we are looking at the main menu or changing scenes
        public static bool vrCamAvailable = false;
        private IShortcut shortcutLogCameras = new KeyboardShortcut(new KeyStroke("F8"), LogCameras);

        public void OnApplicationQuit() {
        }

        public void OnApplicationStart() {
            if (Environment.CommandLine.Contains("--vr")) {
                Logger.Info("started");
                //StartVR();
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

        public static bool IsVrCamera(Camera cam) {
            if (cam == null) {
                return false;
            }
            switch (cam.name) {
                case "Main Camera_Prefab": // Studio and Main 3D-World
                case "Main HsceneCamera": // Main HScene
                    return true;
                default:
                    return false;
            }
        }

        private bool HasVrCamera() {
            GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag("Camera3D");
            // In the main game, a camera named "Character Camera" is present right after the map
            // is loaded. It disappears shortly after. We are looking for a different camera:
            return gameObjectWithTag != null && IsVrCamera(gameObjectWithTag.GetComponent<Camera>());
        }

        private static void StartVR() {
            var manager = VRManager.Create<SexyStudioInterpreter>(new SexyStudioContext());
            manager.SetMode<SexyStudioSeatedMode>();
        }

        public void OnFixedUpdate() {
            // TODO at some point we can get the starting menu of the main game into vr when copying
            // a NULL camera to VRCamera, investigate...
            if (vrEnabled && !vrCamAvailable) {
                vrCamAvailable = HasVrCamera();
                if (vrCamAvailable && vrStarted) {
                    // Copy the new cam (old one had been removed by the game on scene change)
                    Logger.Info("Found a camera, copy");
                    VRCamera.Instance.Copy(VRManager.Instance.Interpreter.FindCamera());
                } else if (vrCamAvailable) {
                    // First start, init everything
                    Logger.Info("Found a camera, starting VR");
                    StartVR();
                    vrStarted = true;
                }
            }
        }

        public void OnLateUpdate() {
        }

        public void OnLevelWasInitialized(int level) {
        }

        public void OnLevelWasLoaded(int level) {
        }

        public void OnUpdate() {
            shortcutLogCameras.Evaluate();

            if (vrEnabled && vrCamAvailable) {
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
