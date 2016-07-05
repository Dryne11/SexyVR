using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Valve.VR;
using VRGIN.Controls;
using VRGIN.Core;
using VRGIN.Helpers;
using VRGIN.Modes;

namespace SexyVR {
    class SexyStudioSeatedMode : SeatedMode {

        private static bool _ControllerFound = false;
        //private PropertyInfo _IllusionCameraRotation = typeof(BaseCameraControl).GetProperty("Rot");
        //private BaseCameraControl _IllusionCamera;

        protected override void OnEnable() {
            Logger.Info("Enter seated mode");
            SteamVR_Utils.Event.Listen("device_connected", OnDeviceConnected);
        }

        protected override void OnDisable() {
            Logger.Info("Leave seated mode");
            SteamVR_Utils.Event.Remove("device_connected", OnDeviceConnected);

        }

        public override ETrackingUniverseOrigin TrackingOrigin {
            get {
                return ETrackingUniverseOrigin.TrackingUniverseSeated;
            }
        }

        private void OnDeviceConnected(object[] args) {
            if (!_ControllerFound) {
                var index = (uint) (int) args[0];
                var connected = (bool) args[1];
                Logger.Info("Device connected: {0}", index);

                if (connected && index > OpenVR.k_unTrackedDeviceIndex_Hmd) {
                    var system = OpenVR.System;
                    if (system != null && system.GetTrackedDeviceClass(index) == ETrackedDeviceClass.Controller) {
                        _ControllerFound = true;

                        // Switch to standing mode
                        if (VR.Manager.Mode is SexyStudioSeatedMode) {
                            ChangeMode();
                        }
                    }
                }
            }
        }

        protected virtual void ChangeMode() {
            //VR.Manager.SetMode<SexyStandingMode>();
        }

        protected override IEnumerable<IShortcut> CreateShortcuts() {
            return base.CreateShortcuts().Concat(new IShortcut[] {
                new MultiKeyboardShortcut(new KeyStroke("Ctrl + C"), new KeyStroke("Ctrl + C"), ChangeMode ),
            });
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnLevel(int level) {
            base.OnLevel(level);
            //_IllusionCamera = FindObjectOfType<BaseCameraControl>();
            //Logger.Info("SeatedMode.OnLevel() camera is {0}", _IllusionCamera);
        }

        protected override void CorrectRotationLock() {
            //if (_IllusionCamera) {
            //    Logger.Info("SeatedMode.CorrectRotationLock()");
            //    var my = VR.Camera.SteamCam.origin;


            //    Logger.Info("PropertyInfo is {0}", _IllusionCameraRotation);
            //    Logger.Info("Setting to {0}", my.eulerAngles);

            //    _IllusionCameraRotation.SetValue(_IllusionCamera, my.eulerAngles, null);

            //    //Vector3 b = my.rotation * (Vector3.back * _IllusionCamera.Distance);
            //    //my.position = _IllusionCamera.Focus + b;
            //}
        }

        protected override void SyncCameras() {
            //if (_IllusionCamera) {
            //    Logger.Info("SeatedMode.SyncCameras()");
            //    var my = VR.Camera.SteamCam.origin;

            //    //_IllusionCamera.Set(
            //    //    my.position + my.forward,
            //    //    Quaternion.LookRotation(my.forward, my.up).eulerAngles,
            //    //1);
            //}
        }
    }
}
