using RootMotion.FinalIK;
using System.Collections.Generic;
using VRGIN.Core;

namespace SexyVR {

    class IKEffectorRegsitry {

        private static List<EffectorAndActor> _effectorsToActors = new List<EffectorAndActor>();
        public static IEnumerable<EffectorAndActor> EffectorsToActors {
            get { return _effectorsToActors; }
        }

        public static void Register(CharBody actor) {
            actor.getIK().solver.OnPostInitiate += new IKSolver.UpdateDelegate(PostInitiateCalled);
            foreach (IKEffector effector in actor.getIK().solver.effectors) {
                _effectorsToActors.Add(new EffectorAndActor(effector, actor));
            }
        }

        public static void PostInitiateCalled() {
            Logger.Info("OnPostInitiate Event. Can we use this for something?");
        }
    }

    public class EffectorAndActor {
        public readonly IKEffector Effector;
        public readonly CharBody Actor;

        public EffectorAndActor(IKEffector effector, CharBody actor) {
            Effector = effector;
            Actor = actor;
        }
    }
}
