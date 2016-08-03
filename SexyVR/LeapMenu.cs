using RootMotion.FinalIK;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using VRGIN.Core;

namespace SexyVR {

    class LeapMenu : ProtectedBehaviour {

        private bool showGirlEffectors = false;
        private ObjectIDGenerator id = new ObjectIDGenerator();

        public LeapMenu() : base() {
        }

        private void OnGUI() {
            if (!SexyStudioVR.menuVisible) {
                return;
            }
            GUIStyle guiStyleLabel = new GUIStyle(GUI.skin.label);
            guiStyleLabel.fontSize = 16;
            GUIStyle guiStyleLabelInRange = new GUIStyle(guiStyleLabel);
            guiStyleLabelInRange.normal.textColor = Color.green;
            GUIStyle guiStyleLabelOutOfRange = new GUIStyle(guiStyleLabel);
            guiStyleLabelOutOfRange.normal.textColor = Color.red;
            GUIStyle guiStyleButton = new GUIStyle(GUI.skin.button);
            guiStyleButton.fontSize = 16;
            GUILayout.BeginArea(new Rect((float)Screen.width / 5f,
                70f,
                (float)Screen.width / 1.5f,
                (float)Screen.height / 1.5f));
            GUILayout.BeginVertical(new GUIStyle(GUI.skin.box));
            var leftHandPosition = IKHelper.GetHandPosition(true);
            var rightHandPosition = IKHelper.GetHandPosition(false);
            GUILayout.Label(string.Format("Left Hand:  {0}", format(leftHandPosition)), guiStyleLabel);
            GUILayout.Label(string.Format("Right Hand: {0}", format(rightHandPosition)), guiStyleLabel);
            if (GUILayout.Button("Visible", guiStyleButton)) {
                foreach (var actor in VR.Interpreter.Actors) {
                    IKHelper.MakeVisible((actor as SexyActor).Actor);
                }
            }
            var girlEffectorsText = showGirlEffectors ? "Hide girl effectors" : "Show girl effectors";
            if (GUILayout.Button(girlEffectorsText, guiStyleButton)) {
                showGirlEffectors = !showGirlEffectors;
            }
            if (showGirlEffectors) {
                if ((VR.Interpreter as SexyStudioInterpreter).IsStudio) {
                    // sucks, can't use this in the main game...
                    //IKHelper.UpdateAllIKPosition();
                }
                foreach (var actor in VR.Interpreter.Actors) {
                    GUILayout.Label(string.Format("Actor {0}", (actor as SexyActor).Actor.SaveFileName), guiStyleLabel);
                    Dictionary<FullBodyBipedEffector, IKEffector> dict = IKHelper.GetEffectors(actor as SexyActor);
                    // left
                    bool inRange = IKHelper.IsInRange(dict[FullBodyBipedEffector.LeftHand], leftHandPosition)
                        || IKHelper.IsInRange(dict[FullBodyBipedEffector.LeftHand], rightHandPosition);
                    GUILayout.Label(string.Format("L Hand:     {0}",
                        format(IKHelper.GetEffectorPosition(dict[FullBodyBipedEffector.LeftHand]))),
                            inRange ? guiStyleLabelInRange : guiStyleLabelOutOfRange);
                    inRange = IKHelper.IsInRange(dict[FullBodyBipedEffector.LeftShoulder], leftHandPosition)
                        || IKHelper.IsInRange(dict[FullBodyBipedEffector.LeftShoulder], rightHandPosition);
                    GUILayout.Label(string.Format("L Shoulder: {0}",
                        format(IKHelper.GetEffectorPosition(dict[FullBodyBipedEffector.LeftShoulder]))),
                            inRange ? guiStyleLabelInRange : guiStyleLabelOutOfRange);
                    inRange = IKHelper.IsInRange(dict[FullBodyBipedEffector.LeftThigh], leftHandPosition)
                        || IKHelper.IsInRange(dict[FullBodyBipedEffector.LeftThigh], rightHandPosition);
                    GUILayout.Label(string.Format("L Thigh:    {0}",
                        format(IKHelper.GetEffectorPosition(dict[FullBodyBipedEffector.LeftThigh]))),
                            inRange ? guiStyleLabelInRange : guiStyleLabelOutOfRange);
                    inRange = IKHelper.IsInRange(dict[FullBodyBipedEffector.LeftFoot], leftHandPosition)
                        || IKHelper.IsInRange(dict[FullBodyBipedEffector.LeftFoot], rightHandPosition);
                    GUILayout.Label(string.Format("L Foot:     {0}",
                        format(IKHelper.GetEffectorPosition(dict[FullBodyBipedEffector.LeftFoot]))),
                            inRange ? guiStyleLabelInRange : guiStyleLabelOutOfRange);
                    // right
                    inRange = IKHelper.IsInRange(dict[FullBodyBipedEffector.RightHand], leftHandPosition)
                        || IKHelper.IsInRange(dict[FullBodyBipedEffector.RightHand], rightHandPosition);
                    GUILayout.Label(string.Format("R Hand:     {0}",
                        format(IKHelper.GetEffectorPosition(dict[FullBodyBipedEffector.RightHand]))),
                            inRange ? guiStyleLabelInRange : guiStyleLabelOutOfRange);
                    inRange = IKHelper.IsInRange(dict[FullBodyBipedEffector.RightShoulder], leftHandPosition)
                        || IKHelper.IsInRange(dict[FullBodyBipedEffector.RightShoulder], rightHandPosition);
                    GUILayout.Label(string.Format("R Shoulder: {0}",
                        format(IKHelper.GetEffectorPosition(dict[FullBodyBipedEffector.RightShoulder]))),
                            inRange ? guiStyleLabelInRange : guiStyleLabelOutOfRange);
                    inRange = IKHelper.IsInRange(dict[FullBodyBipedEffector.RightThigh], leftHandPosition)
                        || IKHelper.IsInRange(dict[FullBodyBipedEffector.RightThigh], rightHandPosition);
                    GUILayout.Label(string.Format("R Thigh:    {0}",
                        format(IKHelper.GetEffectorPosition(dict[FullBodyBipedEffector.RightThigh]))),
                            inRange ? guiStyleLabelInRange : guiStyleLabelOutOfRange);
                    inRange = IKHelper.IsInRange(dict[FullBodyBipedEffector.RightFoot], leftHandPosition)
                        || IKHelper.IsInRange(dict[FullBodyBipedEffector.RightFoot], rightHandPosition);
                    GUILayout.Label(string.Format("R Foot:     {0}",
                        format(IKHelper.GetEffectorPosition(dict[FullBodyBipedEffector.RightFoot]))),
                            inRange ? guiStyleLabelInRange : guiStyleLabelOutOfRange);
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private string format(Vector3 v3) {
            return string.Format("x={0:0.00} y={1:0.00} z={2:0.00}", v3.x, v3.y, v3.z);
        }
    }
}
