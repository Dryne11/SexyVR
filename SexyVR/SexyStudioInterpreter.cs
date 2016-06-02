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
        }

        public override Camera FindCamera() {
            SexyStudioVR.LogCameras();
            GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag("Camera3D");
            if (gameObjectWithTag != null) {
                Camera cam = gameObjectWithTag.GetComponent<Camera>();
                if (SexyStudioVR.IsVrCamera(cam)) {
                    cam.cullingMask = int.MaxValue; // just give us all please?
                    return cam;
                }
            }
            // Happens after OnLevel. Tell the SexyStudioVR to look for a valid camera again:
            SexyStudioVR.vrCamAvailable = false;
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
