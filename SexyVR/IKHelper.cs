using Leap.Unity;
using Manager;
using RootMotion.FinalIK;
using System.Collections.Generic;
using UnityEngine;
using VRGIN.Core;

namespace SexyVR {

    class IKHelper {

        public static float GRAB_RANGE = 0.3f;

        public static Vector3 GetHandPosition(bool left) {
            return GetHandPosition(left ? VR.Mode.LeftHand : VR.Mode.RightHand);
        }

        public static Vector3 GetHandPosition(HandAttachments hand) {
            return hand.Palm.position;
        }

        public static Quaternion GetHandRotation(bool left) {
            return GetHandRotation(left ? VR.Mode.LeftHand : VR.Mode.RightHand);
        }

        public static Quaternion GetHandRotation(HandAttachments hand) {
            return hand.Palm.rotation;
        }

        public static Vector3 GetEffectorPosition(IKEffector effector) {
            return effector.bone.position;
        }

        public static Quaternion GetEffectorRotation(IKEffector effector) {
            return effector.bone.rotation;
        }

        public static Dictionary<FullBodyBipedEffector, IKEffector> GetEffectors(SexyActor actor) {
            return GetEffectors(actor.Actor);
        }

        public static Dictionary<FullBodyBipedEffector, IKEffector> GetEffectors(CharBody actor) {
            Dictionary<FullBodyBipedEffector, IKEffector> dict = new Dictionary<FullBodyBipedEffector, IKEffector>();
            // main game does not have ik set......................................
            Logger.Info("actor ik={0}", actor.getIK());
            dict.Add(FullBodyBipedEffector.LeftHand, actor.getIK().solver.leftHandEffector);
            dict.Add(FullBodyBipedEffector.LeftShoulder, actor.getIK().solver.leftShoulderEffector);
            dict.Add(FullBodyBipedEffector.LeftThigh, actor.getIK().solver.leftThighEffector);
            dict.Add(FullBodyBipedEffector.LeftFoot, actor.getIK().solver.leftFootEffector);
            dict.Add(FullBodyBipedEffector.RightHand, actor.getIK().solver.rightHandEffector);
            dict.Add(FullBodyBipedEffector.RightShoulder, actor.getIK().solver.rightShoulderEffector);
            dict.Add(FullBodyBipedEffector.RightThigh, actor.getIK().solver.rightThighEffector);
            dict.Add(FullBodyBipedEffector.RightFoot, actor.getIK().solver.rightFootEffector);
            return dict;
        }

        public static IEnumerable<EffectorAndActor> GetEffectorsToActors() {
            List<EffectorAndActor> effectorsToActors = new List<EffectorAndActor>();
            foreach (var actor in VR.Interpreter.Actors) {
                var charBody = (actor as SexyActor).Actor;
                foreach (var effector in GetEffectors(charBody).Values) {
                    effectorsToActors.Add(new EffectorAndActor(effector, charBody));
                }
            }
            return effectorsToActors;
        }

        public static float GetDistance(IKEffector effector, Vector3 toCheck) {
            return Vector3.Distance(GetEffectorPosition(effector), toCheck);
        }

        public static bool IsInRange(IKEffector effector, Vector3 toCheck) {
            return GetDistance(effector, toCheck) < GRAB_RANGE;
        }

        public static void UpdateAllIKPosition() {
            Studio.Instance.studioUIIK.IK_AnimeImitation();
            //VR.Interpreter.Actors.Select(actor => (actor as SexyActor).Actor)
        }

        public static void MakeVisible(CharBody actor) {
            foreach (IKEffector effector in actor.getIK().solver.effectors) {
                //effector.target = effector.bone;
                GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<EffectorVisible>().reference = effector;
                GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<BoneVisible>().reference = effector;
            }
        }

        public class EffectorVisible : ProtectedBehaviour {

            public IKEffector reference;

            public EffectorVisible() {
                transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                renderer.material.color = Color.green;
            }

            protected override void OnUpdate() {
                base.OnUpdate();
                transform.position = reference.position;
                transform.rotation = reference.rotation;
            }
        }

        public class BoneVisible : ProtectedBehaviour {

            public IKEffector reference;

            public BoneVisible() {
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                renderer.material.color = Color.red;
            }

            protected override void OnUpdate() {
                base.OnUpdate();
                transform.position = IKHelper.GetEffectorPosition(reference);
                transform.rotation = IKHelper.GetEffectorRotation(reference);
            }
        }
    }
}
