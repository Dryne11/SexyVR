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
            // We don't need this anymore as we take everything "live" in the IKHelper.
            // However if we have performance problems, use the lookup registry again.
            //IKEffectorRegsitry.Register(actor);
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
                // In the main game we don't have this ObjHead. However, where are the eyes?
                return Actor.GetObjHead() != null ? Actor.GetObjHead().transform : Actor.transform;
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
