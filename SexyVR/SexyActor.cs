using RootMotion;
using RootMotion.FinalIK;
using System.Collections.Generic;
using UnityEngine;
using VRGIN.Core;

namespace SexyVR {

    class SexyActor : DefaultActor<CharBody> {

        public SexyActor(CharBody nativeActor) : base(nativeActor) {
        }

        protected override void Initialize(CharBody actor) {
            base.Initialize(actor);

            foreach (var bone in SoftCustomBones) {
                DynamicColliderRegistry.RegisterDynamicBone(bone);
            }
            foreach (var bone in SoftBones) {
                DynamicColliderRegistry.RegisterDynamicBone(bone);
            }
            foreach (var effector in actor.getIK().solver.effectors) {
                IKEffectorRegistry.Register(effector);
            }
        }

        public IEnumerable<DynamicBone> SoftBones {
            get {
                return Actor.GetComponentsInChildren<DynamicBone>();
            }
        }

        public IEnumerable<DynamicBone_Custom> SoftCustomBones {
            get {
                return Actor.GetComponentsInChildren<DynamicBone_Custom>();
            }
        }

        public override Transform Eyes {
            get {
                return Actor.GetObjHead().transform;
            }
        }

        public override bool HasHead {
            get {
                return true;
            }

            set { }
        }
    }
}
