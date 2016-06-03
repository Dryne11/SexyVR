using System;
using System.Collections.Generic;
using UnityEngine;
using VRGIN.Core;

namespace SexyVR {
    class SexyStudioInterpreter : GameInterpreter {

        private bool _isStudio = true;
        private string _targetCameraName = null;
        private bool _lookForCamera = false;

        private List<IActor> _Actors = new List<IActor>();
        public override IEnumerable<IActor> Actors {
            get { return _Actors; }
        }

        protected override void OnStart() {
            base.OnStart();
            _isStudio = Environment.CommandLine.Contains(SexyStudioVR._StudioExecutable);
            gameObject.AddComponent<OSPManager>();
        }

        protected override void OnLevel(int level) {
            base.OnLevel(level);
            _targetCameraName = CameraNameForLevel(level);
            _lookForCamera = true;
            Logger.Info("OnLevel({0}) - ({1})", level, _targetCameraName);
        }

        protected override void OnFixedUpdate() {
            base.OnFixedUpdate();
            if (_lookForCamera) {
                if (_targetCameraName == null) {
                    _lookForCamera = false;
                    // menu and stuff works without a 3D-Camera
                    VRCamera.Instance.Copy(null);
                } else {
                    Camera camera = FindCamera();
                    if (camera != null) {
                        _lookForCamera = false;
                        VRCamera.Instance.Copy(camera);
                    }
                }
            }
        }

        private string CameraNameForLevel(int level) {
            switch (level) {
                case 1:
                    // In Maingame, level 1 is the Logo. Studio only has level 1.
                    return _isStudio ? "Main Camera_Prefab" : null;
                case 2:
                    return null;
                case 3: // 3D-World (Main)
                case 5: // Intro-Scene (Main)
                    return "Main Camera_Prefab";
                case 6: // 'Something erotic' (Main)
                    return "Main HsceneCamera";
                default:
                    return null;
            }
        }

        public override Camera FindCamera() {
            if (_targetCameraName != null) {
                foreach (Camera camera in GameObject.FindObjectsOfType<Camera>()) {
                    if (_targetCameraName.Equals(camera.name)) {
                        // Quick hack to display all canvas. Else we don't catch everything.
                        camera.cullingMask = int.MaxValue;
                        return camera;
                    }
                }
            }
            return null;
        }

        public override IEnumerable<Camera> FindSubCameras() {
            // Simply adding all available cameras still does not give us the 'right' culling mask.
            // See hack in FindCamera().
            //foreach (Camera cam in GameObject.FindObjectsOfType<Camera>()) {
            //    if (!SexyStudioVR.TARGET_CAMERA.Equals(cam.name)) {
            //        yield return cam;
            //    }
            //}
            yield break;
        }
    }
}
