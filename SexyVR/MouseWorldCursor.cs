using System.Reflection;
using UnityEngine;
using VRGIN.Core;

namespace SexyVR {
    class MouseWorldCursor : ProtectedBehaviour {

        private static MouseWorldCursor _Instance;
        public Camera targetCamera = null;
        private bool isVisible = false;
        public bool Visible {
            get {
                return isVisible;
            }
            set {
                transform.localScale = value
                    ? new Vector3(.1f, .1f, .1f)
                    : new Vector3(0f, 0f, 0f);
                isVisible = value;
            }
        }

        public static MouseWorldCursor Instance {
            get {
                if (_Instance == null) {
                    _Instance = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<MouseWorldCursor>();
                }
                return _Instance;
            }
        }

        protected override void OnAwake() {
            base.OnAwake();
            Camera[] allCameras = GameObject.FindObjectsOfType<Camera>();
            for (int i = 0; i < allCameras.Length && targetCamera == null; ++i) {
                Camera inspect = allCameras[i];
                if ("OgjControllerCamera".Equals(inspect.name)) {
                    targetCamera = inspect;
                }
            }
        }

        protected override void OnUpdate() {
            base.OnUpdate();
            if (isVisible && targetCamera != null) {
                Vector3 v3 = Input.mousePosition;
                v3.z = 10f;
                transform.position = targetCamera.ScreenToWorldPoint(v3);
            }
        }
    }
}
