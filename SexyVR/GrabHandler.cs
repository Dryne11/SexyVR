using Leap.Unity;
using RootMotion;
using RootMotion.FinalIK;
using System.Linq;
using UnityEngine;
using VRGIN.Core;
using VRGIN.Helpers;

namespace SexyVR {

    class GrabHandler : ProtectedBehaviour {

        private PinchDetector pinchDetector;
        private HandAttachments hand;
        private bool grabbing = false;
        private IKEffector currentEffector = null;
        private Quaternion startingRotation = Quaternion.identity;

        protected override void OnStart() {
            base.OnStart();
            hand = GetComponent<HandAttachments>();
            SetUpDetectors();
        }

        protected override void OnUpdate() {
            base.OnUpdate();
            if (grabbing && currentEffector != null) {
                currentEffector.target.position = hand.Thumb.position;
                //Logger.Info("startingRot = {0} currentRot = {1} result = {2}",
                //    startingRotation,
                //    hand.transform.rotation,
                //    Quaternion.Inverse(startingRotation) * hand.Thumb.transform.rotation);
                currentEffector.target.rotation = Quaternion.Inverse(hand.Thumb.transform.rotation) * startingRotation;
            }
        }

        protected void SetUpDetectors() {
            var detectorHolder = UnityHelper.CreateGameObjectAsChild("Grab Detector Holder", transform).gameObject;
            pinchDetector = detectorHolder.AddComponent<PinchDetector>();
            pinchDetector._handModel = hand;
            pinchDetector.OnActivate.AddListener(OnPinchStart);
            pinchDetector.OnDeactivate.AddListener(OnPinchEnd);
        }

        protected void OnPinchStart() {
            if (grabbing) {
                return;
            }
            var closestEffector = FindClosestIkEffector();
            if (closestEffector != null
                && Vector3.Distance(closestEffector.position, hand.Thumb.position) < 0.1f) {
                grabbing = true;
                currentEffector = closestEffector;
                if (currentEffector.target == null) {
                    currentEffector.target = new GameObject("TargetForEffector").transform;
                }
                startingRotation = hand.Thumb.transform.rotation;
            }
        }

        protected void OnPinchEnd() {
            currentEffector = null;
            grabbing = false;
        }

        protected IKEffector FindClosestIkEffector() {
            return IKEffectorRegistry.IkEffectors
                .OrderBy(ef => Vector3.Distance(hand.Thumb.transform.position, ef.position)).
                FirstOrDefault();
        }
    }
}
