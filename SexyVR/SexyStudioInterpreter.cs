using System.Collections.Generic;
using UnityEngine;
using VRGIN.Core;

namespace SexyVR {
    class SexyStudioInterpreter : GameInterpreter {

        private List<IActor> _Actors = new List<IActor>();
        public override IEnumerable<IActor> Actors {
            get { return _Actors; }
        }

        protected override void OnStart() {
            base.OnStart();

            gameObject.AddComponent<OSPManager>();

            //if (IsUIOnlyScene) {
            //    GameControl ctrl = GameObject.FindObjectOfType<GameControl>();
            //    ctrl.MapDataCtrl.ChangeMap(ctrl.MapDataCtrl.Datas.ElementAt(1).Value, ctrl, VRCamera.Instance.camera, false, false);
            //}
        }

        protected override void OnLevel(int level) {
            base.OnLevel(level);

            //if (IsUIOnlyScene) {
            //    GameControl ctrl = GameObject.FindObjectOfType<GameControl>();
            //    ctrl.MapDataCtrl.ChangeMap(ctrl.MapDataCtrl.Datas.ElementAt(1).Value, ctrl, VRCamera.Instance.camera, false, false);
            //}
        }

        //private bool IsUIOnlyScene {
        //    get {
        //        return !GameObject.FindObjectOfType<IllusionCamera>();
        //    }
        //}

        protected override void OnUpdate() {
            base.OnUpdate();
            FindCamera();
        }

        public override Camera FindCamera() {
            //Logger.Info("Found {0} cameras:", Camera.allCamerasCount);
            //foreach (Camera cam in Camera.allCameras) {
            //    Logger.Info("{0}", cam);
            //}
            //Logger.Info("current={0} -- main={1}", Camera.current, Camera.main);

            GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag("Camera3D");
            Camera cam = (Camera) gameObjectWithTag.GetComponent<Camera>();
            Logger.Info("using camera {0} -- pos {1} -- rot {2}", cam, cam.transform.position, cam.transform.rotation);
            return cam;
        }
    }
}
