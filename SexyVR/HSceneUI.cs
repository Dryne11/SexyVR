using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using VRGIN.Core;

namespace SexyVR {

    class HSceneUI : ProtectedBehaviour {

        private Canvas _Canvas;
        private CanvasScaler _Scaler;
        private RawImage _Image;
        private Camera _Camera;
        private bool _ShouldRenderMenu = false;
        private bool _IsRendering = false;
        private Texture2D _TextureClear;

        public static HSceneUI Create() {
            var ui = new GameObject("HSceneUI").AddComponent<HSceneUI>();
            return ui;
        }

        protected override void OnAwake() {
            base.OnAwake();

            _Canvas = gameObject.AddComponent<Canvas>();
            _Canvas.sortingOrder = 100;
            _Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _Scaler = gameObject.AddComponent<CanvasScaler>();
            _Scaler.dynamicPixelsPerUnit = 100;
            _TextureClear = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            _TextureClear.SetPixel(0, 0, Color.black); // I haven't found this black dot yet lol
            _Image = new GameObject().AddComponent<RawImage>();
            _Image.transform.SetParent(_Canvas.transform, false);
            _Image.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, Screen.height);
            _Image.texture = _TextureClear;
            var rectTransform = _Image.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.sizeDelta = new Vector2(1, 1);

            gameObject.layer = LayerMask.NameToLayer(VR.Context.UILayer);
        }

        protected override void OnLevel(int level) {
            base.OnLevel(level);
            if (!_IsRendering && level == 6) {
                _ShouldRenderMenu = true;
            } else if (_IsRendering) {
                _ShouldRenderMenu = false;
                StopRendering();
            }
        }

        protected override void OnFixedUpdate() {
            base.OnFixedUpdate();
            //if (!_Canvas.enabled) {
            //    _Canvas.enabled = true;
            //}
        }

        protected override void OnUpdate() {
            base.OnUpdate();
            if (_ShouldRenderMenu
                && !_IsRendering
                && GameObject.Find("UI Root") != null) {
                // We switched to HScene. Clone the hscene menu.
                StartRendering();
            }
        }

        private void StartRendering() {
            Logger.Info("StartRendering()");
            UIPanel panel = GameObject.Find("UI Root").GetComponent<UIPanel>();
            if (panel == null) {
                Logger.Warn("UI Root not found!");
                return;
            }

            // Grab the camera that sees the UIPanel and tell it to draw into a custom texture,
            // which is then displayed in this canvas image.
            FieldInfo fiCamera = typeof(UIRect).GetField("mCam", BindingFlags.NonPublic | BindingFlags.Instance);
            _Camera = fiCamera.GetValue(panel) as Camera;
            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
            // Set the camera background to black with an alpha of 0 to get transparency and
            // no smearing when the menu plays its 'animations'.
            _Camera.backgroundColor = new Color(0f, 0f, 0f, 0f);
            _Camera.clearFlags = CameraClearFlags.SolidColor;
            _Camera.targetTexture = rt;
            _Image.texture = rt;
            _Image.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);

            _IsRendering = true;
            _Canvas.enabled = true;
        }

        private void StopRendering() {
            Logger.Info("StopRendering()");
            _IsRendering = false;
            _Canvas.enabled = false;
            RenderTexture oldTexture = _Image.texture as RenderTexture;
            // Camera might be destroyed already.
            if (_Camera != null) {
                _Camera.targetTexture = null;
            }
            _Image.texture = _TextureClear;
            _Image.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
            Destroy(oldTexture);
        }
    }
}
