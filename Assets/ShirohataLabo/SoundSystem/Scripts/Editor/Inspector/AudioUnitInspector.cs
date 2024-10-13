using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomEditor(typeof(AudioUnit))]
    public class AudioUnitInspector : Editor {
        readonly EditorSoundPlayer _editorSoundPlayer = new();

        void OnEnable() {
            _editorSoundPlayer.OnEnable();
            _editorSoundPlayer.Bind(target as AudioUnit);
        }

        void OnDisable() {
            _editorSoundPlayer.OnDisable();
        }
        
        public override void OnInspectorGUI() {
            serializedObject.Update();

            // メンバ取得
            var categoryProp = serializedObject.FindProperty("_category");
            var clipProp = serializedObject.FindProperty("_clip");
            var volumeOptionProp = serializedObject.FindProperty("_volumeOption");
            var constantVolumeProp = serializedObject.FindProperty("_constantVolume");
            var volumeCurveProp = serializedObject.FindProperty("_volumeCurve");
            var pitchProp = serializedObject.FindProperty("_pitch");
            var startProp = serializedObject.FindProperty("_start");
            var endProp = serializedObject.FindProperty("_end");

            AudioClip clip = (AudioClip)clipProp.objectReferenceValue;

            // EditorGUILayout.PropertyField(idProp);
            EditorGUILayout.PropertyField(categoryProp);
            EditorGUILayout.PropertyField(clipProp);
            // AudioClip変更時、各種値をリセット
            if ((AudioClip)clipProp.objectReferenceValue != clip) {
                clip = (AudioClip)clipProp.objectReferenceValue;
                // constantVolumeProp.floatValue = 1;
                if (clip == null) {
                    volumeCurveProp.animationCurveValue = new AnimationCurve();
                }
                else {
                    volumeCurveProp.animationCurveValue = new AnimationCurve(new Keyframe(0, 1), new Keyframe(clip.length, 1));
                }
                pitchProp.floatValue = 1;
                startProp.floatValue = 0;
                if (clip == null) {
                    endProp.floatValue = 0;
                }
                else {
                    endProp.floatValue = clip.length;
                }
            }

            // 音量
            EditorGUILayout.PropertyField(volumeOptionProp, new GUIContent("Volume"));
            EditorGUI.indentLevel++;
            if (volumeOptionProp.enumValueIndex == (int) AudioUnit.VolumeType.Constant) {
                EditorGUILayout.PropertyField(constantVolumeProp);
            }
            else if (volumeOptionProp.enumValueIndex == (int) AudioUnit.VolumeType.Curve) {
                EditorGUILayout.PropertyField(volumeCurveProp);
            }
            EditorGUI.indentLevel--;

            GUIUtil.Separator();

            EditorGUILayout.PropertyField(pitchProp);

            EditorGUILayout.LabelField("Play Range");
            EditorGUI.indentLevel++;
            startProp.floatValue = Mathf.Clamp(EditorGUILayout.FloatField("Start", startProp.floatValue), 0, endProp.floatValue);
            endProp.floatValue = Mathf.Clamp(EditorGUILayout.FloatField("End", endProp.floatValue), startProp.floatValue, clip == null ? 0 : clip.length);
            // PlayRange MinMaxSlider
            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUI.indentLevel--;

                EditorGUILayout.Space(12, false);

                EditorGUILayout.LabelField("0", GUILayout.Width(12));

                float start = startProp.floatValue;
                float end = endProp.floatValue;
                EditorGUILayout.MinMaxSlider(ref start, ref end, 0, clip == null ? 0 : clip.length);
                startProp.floatValue = start;
                endProp.floatValue = end;

                EditorGUILayout.LabelField(clip == null ? "0.0" : clip.length.ToString("0.0"), GUILayout.Width(32));

                EditorGUI.indentLevel++;
            }
            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
            
            _editorSoundPlayer.DrawGUILayout();
        }
    }
}