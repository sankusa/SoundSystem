using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomEditor(typeof(AudioUnit))]
    public class AudioUnitInspector : Editor {
        readonly EditorSoundPlayerGUI _editorSoundPlayer = new();

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
            var fromSamplesProp = serializedObject.FindProperty("_fromSamples");
            var toSamplesProp = serializedObject.FindProperty("_toSamples");

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
                fromSamplesProp.intValue = 0;
                if (clip == null) {
                    toSamplesProp.intValue = 0;
                }
                else {
                    toSamplesProp.intValue = clip.samples;
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

            // PlayRange MinMaxSlider
            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUI.indentLevel--;

                EditorGUILayout.Space(12, false);

                EditorGUILayout.LabelField("0", GUILayout.Width(12));

                int fromSamples = fromSamplesProp.intValue;
                int toSamples = toSamplesProp.intValue;
                float fromTime = clip == null ? 0 : (float)fromSamples / clip.frequency;
                float toTime = clip == null ? 0 : (float)toSamples / clip.frequency;

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.MinMaxSlider(ref fromTime, ref toTime, 0, clip == null ? 0 : clip.length);
                if (EditorGUI.EndChangeCheck()) {
                    fromSamplesProp.intValue = (int)(fromTime * clip.frequency);
                    toSamplesProp.intValue = (int)(toTime * clip.frequency);
                }

                EditorGUILayout.LabelField(clip == null ? "0.0" : clip.length.ToString("0.0"), GUILayout.Width(32));

                EditorGUI.indentLevel++;
            }

            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUILayout.LabelField("Time", GUILayout.Width(64));
                float fromTime = clip == null ? 0 : (float)fromSamplesProp.intValue / clip.frequency;
                float toTime = clip == null ? 0 : (float)toSamplesProp.intValue / clip.frequency;
                EditorGUI.BeginChangeCheck();
                using (new LabelWidthScope(54)) {
                    float newFromTime = EditorGUILayout.FloatField("[ From", fromTime);
                    if (EditorGUI.EndChangeCheck()) {
                        fromSamplesProp.intValue = (int)(Mathf.Clamp(newFromTime, 0, toTime) * clip.frequency);
                    }
                }
                using (new IndentLevelScope(0)) {
                    using (new LabelWidthScope(28)) {
                        EditorGUI.BeginChangeCheck();
                        float newToTime = EditorGUILayout.FloatField("- To ", toTime);
                        if (EditorGUI.EndChangeCheck()) {
                            toSamplesProp.intValue = (int)(Mathf.Clamp(newToTime, fromTime, clip == null ? 0 : clip.length) * clip.frequency);
                        }
                    }
                    EditorGUILayout.LabelField("]", GUILayout.Width(8));
                }
            }

            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUILayout.LabelField("Samples", GUILayout.Width(64));
                using (new LabelWidthScope(54)) {
                    int newFromSamples = EditorGUILayout.IntField("[ From", fromSamplesProp.intValue);
                    fromSamplesProp.intValue = Mathf.Clamp(newFromSamples, 0, toSamplesProp.intValue);
                }
                using (new IndentLevelScope(0)) {
                    using (new LabelWidthScope(28)) {
                        int newToSamples = EditorGUILayout.IntField("- To ", toSamplesProp.intValue);
                        toSamplesProp.intValue = Mathf.Clamp(newToSamples, fromSamplesProp.intValue, clip == null ? 0 : clip.samples);
                    }
                    EditorGUILayout.LabelField("]", GUILayout.Width(8));
                }
            }
            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
            
            _editorSoundPlayer.DrawGUILayout();
        }
    }
}