using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomEditor(typeof(StandardAudioClipImportSettings))]
    public class StandardAudioClipImportSettingsInspector : Editor {
        bool _switchEnableForceToMono;
        bool _switchEnableNormalize;
        bool _switchEnableLoadInBackground;
        bool _switchEnableAmbisonic;

        public override void OnInspectorGUI() {
            serializedObject.Update();

            SerializedProperty enableForceToMonoProp = serializedObject.FindProperty("_enableForceToMono");
            SerializedProperty forceToMonoProp = serializedObject.FindProperty("_forceToMono");

            SerializedProperty enableNormalizeProp = serializedObject.FindProperty("_enableNormalize");
            SerializedProperty normalizeProp = serializedObject.FindProperty("_normalize");

            SerializedProperty enableLoadInBackgroundProp = serializedObject.FindProperty("_enableLoadInBackground");
            SerializedProperty loadInBackgroundProp = serializedObject.FindProperty("_loadInBackground");

            SerializedProperty enableAmbisonicProp = serializedObject.FindProperty("_enableAmbisonic");
            SerializedProperty ambisonicProp = serializedObject.FindProperty("_ambisonic");

            if (GUILayout.Button("Item")) {
                GenericMenu menu = new();
                menu.AddItem(
                    new GUIContent("Force To Mono"),
                    enableForceToMonoProp.boolValue,
                    () => _switchEnableForceToMono = true
                );
                menu.AddItem(
                    new GUIContent("Normalize"),
                    enableNormalizeProp.boolValue,
                    () => _switchEnableNormalize = true
                );
                menu.AddItem(
                    new GUIContent("Load In Background"),
                    enableLoadInBackgroundProp.boolValue,
                    () => _switchEnableLoadInBackground = true
                );
                menu.AddItem(
                    new GUIContent("Ambisonic"),
                    enableAmbisonicProp.boolValue,
                    () => _switchEnableAmbisonic = true
                );
                menu.ShowAsContext();
            }

            if (_switchEnableForceToMono) {
                enableForceToMonoProp.boolValue = !enableForceToMonoProp.boolValue;
                _switchEnableForceToMono = false;
            }
            if (_switchEnableNormalize) {
                enableNormalizeProp.boolValue = !enableNormalizeProp.boolValue;
                _switchEnableNormalize = false;
            }
            if (_switchEnableLoadInBackground) {
                enableLoadInBackgroundProp.boolValue = !enableLoadInBackgroundProp.boolValue;
                _switchEnableLoadInBackground = false;
            }
            if (_switchEnableAmbisonic) {
                enableAmbisonicProp.boolValue = !enableAmbisonicProp.boolValue;
                _switchEnableAmbisonic = false;
            }

            if (enableForceToMonoProp.boolValue) {
                EditorGUILayout.PropertyField(forceToMonoProp);
            }
            if (enableNormalizeProp.boolValue) {
                EditorGUILayout.PropertyField(normalizeProp);
            }
            if (enableLoadInBackgroundProp.boolValue) {
                EditorGUILayout.PropertyField(loadInBackgroundProp);
            }
            if (enableAmbisonicProp.boolValue) {
                EditorGUILayout.PropertyField(ambisonicProp);
            }

            SerializedProperty defaultSampleSettingsProp = serializedObject.FindProperty("_defaultSampleSettings");
            EditorGUILayout.PropertyField(defaultSampleSettingsProp);

            serializedObject.ApplyModifiedProperties();
        }
    }
}