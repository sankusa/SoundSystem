using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomEditor(typeof(CustomClip))]
    public class CustomClipInspector : Editor {
        readonly EditorSoundPlayerGUI _editorSoundPlayer = new();

        void OnEnable() {
            _editorSoundPlayer.OnEnable();
            _editorSoundPlayer.Bind(target as CustomClip);
        }

        void OnDisable() {
            _editorSoundPlayer.OnDisable();
        }
        
        public override void OnInspectorGUI() {
            serializedObject.Update();

            // メンバ取得
            var audioClipProp = CustomClip.GetAudioClipProp(serializedObject);

            var volumeMultiplierProp = CustomClip.GetVolumeMultiplierProp(serializedObject);
            var volumeMultiplierCurveProp = CustomClip.GetVolumeMultiplierCurveProp(serializedObject);
            var pitchMultiplierProp = CustomClip.GetPitchMultiplierProp(serializedObject);

            var playRangeProp = CustomClip.GetPlayRangeProp(serializedObject);
            var playRange_EnableProp = playRangeProp.FindPropertyRelative("_enable");
            var playRange_FromSamplesProp = playRangeProp.FindPropertyRelative("_fromSamples");
            var playrange_ToSamplesProp = playRangeProp.FindPropertyRelative("_toSamples");

            var descriptionProp = serializedObject.FindProperty("_description");

            _editorSoundPlayer.DrawGUILayout();

            AudioClip clip = (AudioClip)audioClipProp.objectReferenceValue;

            EditorGUILayout.PropertyField(audioClipProp);
            // AudioClip変更時、各種値をリセット
            if ((AudioClip)audioClipProp.objectReferenceValue != clip) {
                clip = (AudioClip)audioClipProp.objectReferenceValue;
                CustomClip.ResetValueBasedOnAudioClip(serializedObject);
            }

            GUIUtil.Separator();

            EditorGUILayout.PropertyField(volumeMultiplierProp);
            EditorGUILayout.PropertyField(volumeMultiplierCurveProp);

            EditorGUILayout.PropertyField(pitchMultiplierProp);

            EditorGUILayout.LabelField("Play Range", EditorStyles.boldLabel);

            Rect playRangeEnableRect = GUILayoutUtility.GetLastRect();
            playRangeEnableRect.xMin = playRangeEnableRect.xMax - 18;
            EditorGUI.PropertyField(playRangeEnableRect, playRange_EnableProp, GUIContent.none);

            if (playRange_EnableProp.boolValue) {
                EditorGUI.indentLevel++;

                // PlayRange MinMaxSlider
                using (new EditorGUILayout.HorizontalScope()) {
                    EditorGUI.indentLevel--;

                    EditorGUILayout.Space(12, false);

                    EditorGUILayout.LabelField("0", GUILayout.Width(12));

                    int fromSamples = playRange_FromSamplesProp.intValue;
                    int toSamples = playrange_ToSamplesProp.intValue;
                    float fromTime = clip == null ? 0 : (float)fromSamples / clip.frequency;
                    float toTime = clip == null ? 0 : (float)toSamples / clip.frequency;

                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.MinMaxSlider(ref fromTime, ref toTime, 0, clip == null ? 0 : clip.length);
                    if (EditorGUI.EndChangeCheck()) {
                        playRange_FromSamplesProp.intValue = (int)(fromTime * clip.frequency);
                        playrange_ToSamplesProp.intValue = (int)(toTime * clip.frequency);
                    }

                    EditorGUILayout.LabelField(clip == null ? "0.0" : clip.length.ToString("0.0"), GUILayout.Width(36));

                    EditorGUI.indentLevel++;
                }

                using (new EditorGUILayout.HorizontalScope()) {
                    EditorGUILayout.LabelField("Time", GUILayout.Width(64));
                    float fromTime = clip == null ? 0 : (float)playRange_FromSamplesProp.intValue / clip.frequency;
                    float toTime = clip == null ? 0 : (float)playrange_ToSamplesProp.intValue / clip.frequency;
                    EditorGUI.BeginChangeCheck();
                    using (new LabelWidthScope(54)) {
                        float newFromTime = EditorGUILayout.FloatField("[ From", fromTime);
                        if (EditorGUI.EndChangeCheck()) {
                            playRange_FromSamplesProp.intValue = (int)(Mathf.Clamp(newFromTime, 0, toTime) * clip.frequency);
                        }
                    }
                    using (new IndentLevelScope(0)) {
                        using (new LabelWidthScope(28)) {
                            EditorGUI.BeginChangeCheck();
                            float newToTime = EditorGUILayout.FloatField("- To ", toTime);
                            if (EditorGUI.EndChangeCheck()) {
                                playrange_ToSamplesProp.intValue = (int)(Mathf.Clamp(newToTime, fromTime, clip == null ? 0 : clip.length) * clip.frequency);
                            }
                        }
                        EditorGUILayout.LabelField("]", GUILayout.Width(8));
                    }
                }

                using (new EditorGUILayout.HorizontalScope()) {
                    EditorGUILayout.LabelField("Samples", GUILayout.Width(64));
                    using (new LabelWidthScope(54)) {
                        int newFromSamples = EditorGUILayout.IntField("[ From", playRange_FromSamplesProp.intValue);
                        playRange_FromSamplesProp.intValue = Mathf.Clamp(newFromSamples, 0, playrange_ToSamplesProp.intValue);
                    }
                    using (new IndentLevelScope(0)) {
                        using (new LabelWidthScope(28)) {
                            int newToSamples = EditorGUILayout.IntField("- To ", playrange_ToSamplesProp.intValue);
                            playrange_ToSamplesProp.intValue = Mathf.Clamp(newToSamples, playRange_FromSamplesProp.intValue, clip == null ? 0 : clip.samples);
                        }
                        EditorGUILayout.LabelField("]", GUILayout.Width(8));
                    }
                }
                EditorGUI.indentLevel--;
            }

            GUIUtil.Separator();

            EditorGUILayout.PropertyField(descriptionProp);

            serializedObject.ApplyModifiedProperties();
        }
    }
}