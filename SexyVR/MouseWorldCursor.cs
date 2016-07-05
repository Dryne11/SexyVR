using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using VRGIN.Core;

namespace SexyVR {
    class MouseWorldCursor : ProtectedBehaviour {

        private static MouseWorldCursor _Instance;
        private readonly float maxRaycastDistance = 10f;
        private List<Collider> modifiedColliders = new List<Collider>();
        public Camera targetCamera = null;
        private bool isVisible = false;
        public bool Visible {
            get {
                return isVisible;
            }
            set {
                isVisible = value;
                if (isVisible) {
                    findCamera();
                    showCursor();
                } else {
                    hideCursor();
                }
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
            // We don't want to raycast ourself.
            GetComponent<Collider>().enabled = false;
        }

        private void showCursor() {
            transform.localScale = new Vector3(.05f, .05f, .05f);
        }

        private void hideCursor() {
            transform.localScale = new Vector3(0f, 0f, 0f);
        }

        private void enableStudioColliders() {
            // Enable the colliders for character movement and rotation in the studio.
            foreach (SelectedMouseScript script in GameObject.FindObjectsOfType<SelectedMouseScript>()) {
                foreach (Collider collider in script.target.gameObject.GetComponentsInChildren<Collider>()) {
                    if (!collider.enabled) {
                        collider.enabled = true;
                        modifiedColliders.Add(collider);
                    }
                }
            }
        }

        private void disableModifiedStudioColliders() {
            // Disable the colliders for character movement and rotation in the studio if we enabled them,
            // so we don't interfere with the game logic.
            foreach (Collider collider in modifiedColliders) {
                collider.enabled = false;
            }
        }

        protected override void OnUpdate() {
            base.OnUpdate();
            if (isVisible && targetCamera != null) {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
                    // We don't want to see the ugly cursor when touching the girl.
                    hideCursor();
                    return;
                } else {
                    showCursor();
                }
                if ((VR.Interpreter as SexyStudioInterpreter).IsStudio) {
                    modifiedColliders.Clear();
                    enableStudioColliders();
                }
                Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo = new RaycastHit();
                float distance = maxRaycastDistance;
                if (Physics.Raycast(ray, out hitInfo, distance)) {
                    if (hitInfo.collider.gameObject != this.gameObject) {
                        distance = hitInfo.distance;
                    }
                }
                if ((VR.Interpreter as SexyStudioInterpreter).IsStudio) {
                    disableModifiedStudioColliders();
                }
                Vector3 v3 = Input.mousePosition;
                v3.z = distance - .2f;
                transform.position = targetCamera.ScreenToWorldPoint(v3);
            }
        }

        private void findCamera() {
            string targetName = (VR.Interpreter as SexyStudioInterpreter).IsStudio
                ? "OgjControllerCamera"
                : "Main HsceneCamera";
            Camera[] allCameras = GameObject.FindObjectsOfType<Camera>();
            for (int i = 0; i < allCameras.Length && targetCamera == null; ++i) {
                Camera inspect = allCameras[i];
                if (targetName.Equals(inspect.name)) {
                    targetCamera = inspect;
                }
            }
            if (targetCamera == null) {
                targetCamera = Camera.main;
            }
            if (targetCamera == null) {
                Logger.Warn("Could not find a reference camera for the MouseWorldCursor");
            }
        }
    }
}
