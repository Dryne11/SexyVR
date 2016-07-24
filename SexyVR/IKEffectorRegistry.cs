using RootMotion;
using RootMotion.FinalIK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRGIN.Core;

namespace SexyVR {

    class IKEffectorRegistry {

        private static List<IKEffector> _IkEffectors = new List<IKEffector>();
        public static IEnumerable<IKEffector> IkEffectors { get { return _IkEffectors; } }

        public static void Register(SexyActor actor) {
            IKSolverFullBodyBiped solver = actor.Actor.getIK().solver;
            foreach (IKEffector effector in solver.effectors) {
                Register(effector);
            }
        }

        public static void Register(IKEffector effector) {
            _IkEffectors.Add(effector);
        }
    }
}
