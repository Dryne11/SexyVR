using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRGIN.Core;

namespace SexyVR {
    class SexyStudioInterpreter : GameInterpreter {

        private bool _isStudio = true;
        public bool IsStudio { get { return _isStudio; } private set { _isStudio = value; } }
        private string _targetCameraName = null;
        private bool _lookForCamera = false;

        private List<IActor> _Actors = new List<IActor>();
        public override IEnumerable<IActor> Actors {
            get { return _Actors; }
        }

        protected override void OnStart() {
            base.OnStart();
            Logger.Level = Logger.LogMode.Info;
            IsStudio = Environment.CommandLine.Contains(SexyStudioVR._StudioExecutable);
            Logger.Info("studio={0}", _isStudio);
            gameObject.AddComponent<OSPManager>();
            StartCoroutine(fixThings());
        }

        protected override void OnLevel(int level) {
            base.OnLevel(level);
            _targetCameraName = CameraNameForLevel(level);
            _lookForCamera = true;
            Logger.Info("OnLevel({0}) - ({1})", level, _targetCameraName);
        }

        protected override void OnUpdate() {
            base.OnUpdate();
            CleanActors();
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

        private void CleanActors() {
            _Actors = Actors.Where(a => a.IsValid).ToList();
            foreach (var female in FindObjectsOfType<CharBody>()) {
                AddActor(female);
            }
        }

        private void AddActor(CharBody member) {
            if (!member.GetComponent<Marker>()) {
                _Actors.Add(new SexyActor(member));
            }
        }

        private string CameraNameForLevel(int level) {
            switch (level) {
                case 1:
                    // In Maingame, level 1 is the Logo. Studio only has level 1.
                    return IsStudio ? "Main Camera_Prefab" : null;
                case 2:
                    return null;
                case 3: //  3D-World (Main)
                case 5: //  Intro-Scene (Main)
                case 10: // Character creation
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

        public IEnumerator fixThings() {
            while (true) {
                FixCameraTarget();
                FixCanvasLayer();
                yield return new WaitForSeconds(5.0f);
            }
        }

        private void FixCameraTarget() {
            // The target is always set to the camera transform, even in modes like Straight, Divert and Animation.
            // TODO in the main game this get's spammed all over. Someone resets the target we just set?
            NeckLookController[] necks = GameObject.FindObjectsOfType<NeckLookController>();
            foreach (NeckLookController neck in necks) {
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

        public override bool IsIgnoredCanvas(Canvas canvas) {
            // Very big canvas that fills the whole screen in the Maingame and has no use.
            return "Sonner".Equals(LayerMask.LayerToName(canvas.gameObject.layer))
                || "BackGround".Equals(canvas.name);
        }
    }
}
