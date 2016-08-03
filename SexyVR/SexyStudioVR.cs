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
                return "0.5-Snapshot";
            }
        }

        private IShortcut shortcutLogCameras = new KeyboardShortcut(new KeyStroke("F8"), LogCameras);

        public void OnApplicationQuit() {
        }

        public void OnApplicationStart() {
            if (Environment.CommandLine.Contains("--vr")) {
                Logger.Info("started");
                var manager = VRManager.Create<SexyStudioInterpreter>(new SexyStudioContext());
                manager.SetMode<SexyStudioSeatedMode>();
                if (Environment.CommandLine.Contains(_MaingameExecutable)) {
                    HSceneUI ui = HSceneUI.Create();
                    ui.transform.SetParent(VRGUI.Instance.transform, false);
                }
            }
            if (Environment.CommandLine.Contains("--verbose")) {
                Logger.Level = Logger.LogMode.Debug;
            }
        }

        private static LeapMenu GUI = null;
        public static bool menuVisible = false;

        public static void LogCameras() {
            if(GUI == null) {
                GUI = new GameObject("LeapMenu").AddComponent<LeapMenu>();
                Logger.Info("create leapmenu");
            }
            menuVisible = !menuVisible;
            Logger.Info("menuVisible = {0}", menuVisible);
            //MouseWorldCursor.Instance.Visible = !MouseWorldCursor.Instance.Visible;

            //AnimeIKCtrl[] ikCtrls = GameObject.FindObjectsOfType<AnimeIKCtrl>();
            //FieldInfo ikTargets = typeof(AnimeIKCtrl).GetField(
            //    "_ikTarget",
            //    BindingFlags.Instance | BindingFlags.NonPublic);
            //Logger.Info("Found {0} ikCtrls", ikCtrls.Length);
            //foreach (AnimeIKCtrl ikCtrl in ikCtrls) {
            //    Logger.Info("getting value");
            //    var something = ikTargets.GetValue(ikCtrl);
            //    Logger.Info("got value");
            //    Logger.Info("the value is {0}", something);
            //    AnimeIKCtrl.IKTargetCorrection[] targets = ikTargets.GetValue(ikCtrl) as AnimeIKCtrl.IKTargetCorrection[];
            //    Logger.Info("Found {0} target corrections", targets.Length);
            //    foreach (AnimeIKCtrl.IKTargetCorrection target in targets) {
            //        Logger.Info("target={0}", target.TargetBone.position);
            //    }
            //}

            //foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>()) {
            //    if ("Cube".Equals(go.name)) {
            //        foreach (Component comp in go.GetComponents<Component>()) {
            //            Logger.Info("Cube component: {0} activeSelf={1}",
            //                comp,
            //                comp.gameObject.activeSelf);
            //        }
            //        foreach (Component comp in go.GetComponentsInChildren<Component>()) {
            //            Logger.Info("Cube.child ({0}) component: {1} activeSelf={2}",
            //                comp.gameObject,
            //                comp,
            //                comp.gameObject.activeSelf);
            //        }
            //        foreach (Component comp in go.GetComponentsInParent<Component>()) {
            //            Logger.Info("Cube.parent ({0}) component: {1} activeSelf={2}",
            //                comp.gameObject,
            //                comp,
            //                comp.gameObject.activeSelf);
            //        }
            //    }
            //}
            //FieldInfo fiCamera = typeof(SelectedMouseScript).GetField(
            //    "camera",
            //    BindingFlags.Instance | BindingFlags.NonPublic);
            //foreach (SelectedMouseScript script in GameObject.FindObjectsOfType<SelectedMouseScript>()) {
            //    Logger.Info("Found Script for {0} - camera={1}", script.target.gameObject, fiCamera.GetValue(script));
            //    foreach (Collider collider in script.target.gameObject.GetComponentsInChildren<Collider>()) {
            //        Logger.Info("child.collider ({0}.{1}) ena={2}",
            //            collider.gameObject, collider, collider.enabled);
            //    }
            //}

            //Vector3 targetPos = Vector3.zero;
            //foreach (Camera cam in GameObject.FindObjectsOfType<Camera>()) {
            //    if("OgjControllerCamera".Equals(cam.name)) {
            //        cam.transform.SetParent(VRCamera.Instance.SteamCam.head);
            //        Logger.Info("Moved camera");
            //    }
            //}

            //Logger.Info("--");
            //foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>()) {
            //    if (go.name.Equals("Cube")) {
            //        Logger.Info("Checking Cube with position {0}", go.transform.position);
            //        foreach (Camera cam in GameObject.FindObjectsOfType<Camera>()) {
            //            Vector3 screenPoint = cam.WorldToViewportPoint(go.transform.position);
            //            bool onCameraScreen = screenPoint.z > 0
            //                && screenPoint.x > 0 && screenPoint.x < 1
            //                && screenPoint.y > 0 && screenPoint.y < 1;
            //            Logger.Info("{0} ({1}) sees the cube={2}", cam, cam.name, onCameraScreen);
            //        }
            //    }
            //}

            //sb_HScene[] sbHScenes = GameObject.FindObjectsOfType<sb_HScene>();
            //Logger.Info("Found {0} sb_HScenes", sbHScenes.Length);
            //foreach (sb_HScene scene in sbHScenes) {
            //    FieldInfo fieldFemale = typeof(sb_HScene).GetField("m_Female", BindingFlags.NonPublic | BindingFlags.Instance);
            //    FieldInfo fieldMale = typeof(sb_HScene).GetField("m_Male", BindingFlags.NonPublic | BindingFlags.Instance);
            //    Logger.Info("scene.IsYes={0} m_Female={1} m_Male={2}",
            //        scene.IsYes, fieldFemale.GetValue(scene), fieldMale.GetValue(scene));
            //}

            //HSceneManager manager = Singleton<HSceneManager>.Instance;
            //Logger.Info("HSceneManager: PlayHScene={0} Female={1} Male={2}",
            //    manager.PlayHScene, manager.Female, manager.Male);
            //NeckLookController[] necks = GameObject.FindObjectsOfType<NeckLookController>();
            //Logger.Info("Dumping {0} characters", necks.Length);
            //foreach (NeckLookController neck in necks) {
            //    CharBody chara = neck.GetComponentInParent<CharBody>();
            //    if (chara != null) {
            //        Logger.Info("Found character: {0}", chara);
            //    } else {
            //        Logger.Info("NeckLookController without character?");
            //    }
            //}

            //Logger.Info("Dumping cameras");
            //foreach (Camera cam in GameObject.FindObjectsOfType<Camera>()) {
            //    Logger.Info("{0} ({1})", cam, cam.name);
            //}
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
        }
    }
}
