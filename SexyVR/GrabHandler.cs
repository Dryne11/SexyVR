using Leap.Unity;
using System.Linq;
using UnityEngine;
using VRGIN.Core;
using VRGIN.Helpers;

namespace SexyVR {

    class GrabHandler : ProtectedBehaviour {

        private const float FIST_START_THRESHOLD = 1f; // Hold the fist one second to start grabbing.
        private HandAttachments hand;
        private bool checkGrabTarget = false;
        private bool grabbing = false;
        private bool fisting = false;
        private float fistStart = 0f;
        private EffectorAndActor currentEffector = null;
        private Quaternion startingRotation = Quaternion.identity;

        protected override void OnStart() {
            base.OnStart();
            hand = GetComponent<HandAttachments>();
            SetUpDetectors();
        }

        protected void SetUpDetectors() {
            var fistDetector = UnityHelper.CreateGameObjectAsChild("fistdetector", transform)
                .gameObject.AddComponent<ExtendedFingerDetector>();
            fistDetector.HandModel = hand;
            fistDetector.Thumb = PointingState.Either;
            fistDetector.Index = PointingState.NotExtended;
            fistDetector.Middle = PointingState.NotExtended;
            fistDetector.Ring = PointingState.NotExtended;
            fistDetector.Pinky = PointingState.NotExtended;
            fistDetector.OnActivate.AddListener(OnFistStart);
            fistDetector.OnDeactivate.AddListener(OnFistEnd);
        }

        protected void OnFistStart() {
            Logger.Info("OnFistStart");
            // We cannot check for a target at this place. All effectors are at a dummy position and not
            // in sync with the animation. We need to wait until Unity puts us in the On(Late)Update method.
            checkGrabTarget = true;
        }

        protected void OnFistEnd() {
            Logger.Info("OnFistEnd");
            fisting = false;
            grabbing = false;
            currentEffector = null;
            startingRotation = Quaternion.identity;
        }

        protected void testGrabTarget() {
            fistStart = Time.time;
            var closest = FindIkEffectorAndActorInRange();
            if (closest != null) {
                fisting = true;
                currentEffector = closest;
                startingRotation = IKHelper.GetHandRotation(hand);
            } else {
                Logger.Info("No effector in range");
            }
        }

        protected override void OnLateUpdate() {
            base.OnUpdate();
            if (checkGrabTarget) {
                // See OnFistStart()
                checkGrabTarget = false;
                testGrabTarget();
            }
            // After our OnUpdate, someone resets the position in their OnUpdate.
            // Using OnLateUpdate works, but only while grabbing.
            // After assigning HAnimation to the girl, it works in OnUpdate without any foreign resets.
            if (grabbing && currentEffector != null) {
                var effector = currentEffector.Effector;
                effector.positionWeight = 1f; // XXX weight 1 too strong? deformation possible. experiment with RotationLimit.
                var handPos = IKHelper.GetHandPosition(hand);
                var handRot = IKHelper.GetHandRotation(hand);
                effector.position = handPos;
                var newRot = Quaternion.Inverse(handRot) * startingRotation;
                effector.rotation = newRot;
            } else if (fisting && Time.time - fistStart > FIST_START_THRESHOLD) {
                Logger.Info("now grabbing {0}", currentEffector == null ? "nothing" : "something");
                grabbing = true;
            }
        }

        //protected StudioFemale FindClosestStudioFemale() {
        //    return Singleton<Studio>.Instance.FemaleList
        //        .Select(entry => entry.Value)
        //        .OrderBy(studioF => Vector3.Distance(CurrentHandPosition(), studioF.female.gameObject.transform.position))
        //        .FirstOrDefault();
        //}

        //protected SexyActor FindClosestActor() {
        //    return VR.Interpreter.Actors
        //        .Cast<SexyActor>()
        //        .OrderBy(actor => Vector3.Distance(CurrentHandPosition(), actor.Actor.gameObject.transform.position))
        //        .FirstOrDefault();
        //}

        //protected IKEffector FindClosestIkEffectorInRange(SexyActor actor) {
        //    if (actor.Actor.getIK() == null) {
        //        Logger.Info("No IK present");
        //        return null;
        //    }
        //    // Initially the EKEffector.position is at a weird position. We have to check the bone.
        //    var effectorCandidate = actor.Actor.getIK().solver.effectors
        //        .OrderBy(ef => Vector3.Distance(CurrentHandPosition(), ef.bone.position))
        //        .FirstOrDefault();
        //    if (Vector3.Distance(CurrentHandPosition(), effectorCandidate.bone.position) < grabRange) {
        //        return effectorCandidate;
        //    }
        //    return null;
        //}

        //protected EffectorTarget FindClosestEffectorTarget(StudioFemale female) {
        //    return female.ikCtrl.drivingRig.effectorTargets
        //        .OrderBy(et => Vector3.Distance(CurrentHandPosition(), et.target.position))
        //        .FirstOrDefault();
        //}

        //protected IKEffector FindClosestIkEffectorInRange() {
        //    var handPosition = CurrentHandPosition();
        //    var effector = VR.Interpreter.Actors
        //       .Cast<SexyActor>()
        //       .SelectMany(actor => actor.Actor.getIK().solver.effectors)
        //       // Initially the EKEffector.position is at a weird position. We have to check the bone.
        //       .OrderBy(ef => Vector3.Distance(handPosition, ef.bone.position))
        //       .FirstOrDefault();
        //    return effector != null && Vector3.Distance(handPosition, effector.bone.position) < grabRange
        //        ? effector
        //        : null;
        //}

        protected EffectorAndActor FindIkEffectorAndActorInRange() {
            var handPosition = IKHelper.GetHandPosition(hand);
            var effectorToActor = IKHelper.GetEffectorsToActors()
               .OrderBy(pair => IKHelper.GetDistance(pair.Effector, handPosition))
               .FirstOrDefault();
            return effectorToActor == null
                ? null
                : effectorToActor;
        }
    }
}