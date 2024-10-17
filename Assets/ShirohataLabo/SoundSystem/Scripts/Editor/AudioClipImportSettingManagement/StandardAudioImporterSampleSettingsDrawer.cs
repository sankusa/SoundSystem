using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(StandardAudioClipImportSettings.StandardAudioImporterSampleSettings))]
    public class StandardAudioImporterSampleSettingsDrawer : PropertyDrawer {
        bool _switchEnableLoadType;
        bool _switchEnablePreloadAudioData;
        bool _switchEnableCompressionFormat;
        bool _switchEnableQuality;
        bool _switchEnableSampleRateSetting;
        bool _switchEnableSampleRateOverride;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            SerializedProperty enableLoadTypeProp = property.FindPropertyRelative("_enableLoadType");
            SerializedProperty loadTypeProp = property.FindPropertyRelative("_loadType");

            SerializedProperty enablePreloadAudioDataProp = property.FindPropertyRelative("_enablePreloadAudioData");
            SerializedProperty preloadAudioDataProp = property.FindPropertyRelative("_preloadAudioData");

            SerializedProperty enableCompressionFormatProp = property.FindPropertyRelative("_enableCompressionFormat");
            SerializedProperty compressionFormatProp = property.FindPropertyRelative("_compressionFormat");

            SerializedProperty enableQualityProp = property.FindPropertyRelative("_enableQuality");
            SerializedProperty qualityProp = property.FindPropertyRelative("_quality");

            SerializedProperty enableSampleRateSettingProp = property.FindPropertyRelative("_enableSampleRateSetting");
            SerializedProperty sampleRateSettingProp = property.FindPropertyRelative("_sampleRateSetting");

            SerializedProperty enableSampleRateOverrideProp = property.FindPropertyRelative("_enableSampleRateOverride");
            SerializedProperty sampleRateOverrideProp = property.FindPropertyRelative("_sampleRateOverride");

            Rect labelRect = new Rect(position) {height = EditorGUIUtility.singleLineHeight};
            position.yMin += labelRect.height + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.LabelField(labelRect, label);
            Rect buttonRect = labelRect;
            buttonRect.xMin = buttonRect.xMax - 40;
            if (GUI.Button(buttonRect, "Item")) {
                GenericMenu menu = new();
                menu.AddItem(
                    new GUIContent("Load Type"),
                    enableLoadTypeProp.boolValue,
                    () => _switchEnableLoadType = true
                );
                menu.AddItem(
                    new GUIContent("Preload Audio Data"),
                    enablePreloadAudioDataProp.boolValue,
                    () => _switchEnablePreloadAudioData = true
                );
                menu.AddItem(
                    new GUIContent("Compression Format"),
                    enableCompressionFormatProp.boolValue,
                    () => _switchEnableCompressionFormat = true
                );
                menu.AddItem(
                    new GUIContent("Enable Quality"),
                    enableQualityProp.boolValue,
                    () => _switchEnableQuality = true
                );
                menu.AddItem(
                    new GUIContent("Sample Rate Setting"),
                    enableSampleRateSettingProp.boolValue,
                    () => _switchEnableSampleRateSetting = true
                );
                menu.AddItem(
                    new GUIContent("Sample Rate Override"),
                    enableSampleRateOverrideProp.boolValue,
                    () => _switchEnableSampleRateOverride = true
                );
                menu.ShowAsContext();
            }

            if (_switchEnableLoadType) {
                enableLoadTypeProp.boolValue = !enableLoadTypeProp.boolValue;
                _switchEnableLoadType = false;
            }
            if (_switchEnablePreloadAudioData) {
                enablePreloadAudioDataProp.boolValue = !enablePreloadAudioDataProp.boolValue;
                _switchEnablePreloadAudioData = false;
            }
            if (_switchEnableCompressionFormat) {
                enableCompressionFormatProp.boolValue = !enableCompressionFormatProp.boolValue;
                _switchEnableCompressionFormat = false;
            }
            if (_switchEnableQuality) {
                enableQualityProp.boolValue = !enableQualityProp.boolValue;
                _switchEnableQuality = false;
            }
            if (_switchEnableSampleRateSetting) {
                enableSampleRateSettingProp.boolValue = !enableSampleRateSettingProp.boolValue;
                _switchEnableSampleRateSetting = false;
            }
            if (_switchEnableSampleRateOverride) {
                enableSampleRateOverrideProp.boolValue = !enableSampleRateOverrideProp.boolValue;
                _switchEnableSampleRateOverride = false;
            }

            EditorGUI.indentLevel++;

            if (enableLoadTypeProp.boolValue) {
                Rect rect = new Rect(position) {height = EditorGUIUtility.singleLineHeight};
                position.yMin += rect.height + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.PropertyField(rect, loadTypeProp);
            }
            if (enablePreloadAudioDataProp.boolValue) {
                Rect rect = new Rect(position) {height = EditorGUIUtility.singleLineHeight};
                position.yMin += rect.height + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.PropertyField(rect, preloadAudioDataProp);
            }
            if (enableCompressionFormatProp.boolValue) {
                Rect rect = new Rect(position) {height = EditorGUIUtility.singleLineHeight};
                position.yMin += rect.height + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.PropertyField(rect, compressionFormatProp);
            }
            if (enableQualityProp.boolValue) {
                Rect rect = new Rect(position) {height = EditorGUIUtility.singleLineHeight};
                position.yMin += rect.height + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.PropertyField(rect, qualityProp);
            }
            if (enableSampleRateSettingProp.boolValue) {
                Rect rect = new Rect(position) {height = EditorGUIUtility.singleLineHeight};
                position.yMin += rect.height + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.PropertyField(rect, sampleRateSettingProp);
            }
            if (enableSampleRateOverrideProp.boolValue) {
                Rect rect = new Rect(position) {height = EditorGUIUtility.singleLineHeight};
                position.yMin += rect.height + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.PropertyField(rect, sampleRateOverrideProp);
            }
            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty enableLoadTypeProp = property.FindPropertyRelative("_enableLoadType");
            SerializedProperty enablePreloadAudioDataProp = property.FindPropertyRelative("_enablePreloadAudioData");
            SerializedProperty enableCompressionFormatProp = property.FindPropertyRelative("_enableCompressionFormat");
            SerializedProperty enableQualityProp = property.FindPropertyRelative("_enableQuality");
            SerializedProperty enableSampleRateSettingProp = property.FindPropertyRelative("_enableSampleRateSetting");
            SerializedProperty enableSampleRateOverrideProp = property.FindPropertyRelative("_enableSampleRateOverride");

            float height = EditorGUIUtility.singleLineHeight;
            if (enableLoadTypeProp.boolValue) height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (enablePreloadAudioDataProp.boolValue) height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (enableCompressionFormatProp.boolValue) height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (enableQualityProp.boolValue) height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (enableSampleRateSettingProp.boolValue) height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (enableSampleRateOverrideProp.boolValue) height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            return height;
        }
    }
}