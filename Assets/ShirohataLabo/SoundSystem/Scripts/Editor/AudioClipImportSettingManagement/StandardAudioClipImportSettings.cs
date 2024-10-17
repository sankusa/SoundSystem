using System;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CreateAssetMenu(fileName = nameof(StandardAudioClipImportSettings), menuName = nameof(SoundSystem) + "/" + nameof(StandardAudioClipImportSettings))]
    public class StandardAudioClipImportSettings : ScriptableObject {
        [SerializeField] bool _enableForceToMono;
        [SerializeField] bool _forceToMono;

        [SerializeField] bool _enableNormalize;
        [SerializeField] bool _normalize;

        [SerializeField] bool _enableLoadInBackground;
        [SerializeField] bool _loadInBackground;

        [SerializeField] bool _enableAmbisonic;
        [SerializeField] bool _ambisonic;

        [Serializable]
        public class StandardAudioImporterSampleSettings {
            [SerializeField] bool _enableLoadType;
            [SerializeField] AudioClipLoadType _loadType;

            [SerializeField] bool _enablePreloadAudioData;
            [SerializeField] bool _preloadAudioData;

            [SerializeField] bool _enableCompressionFormat;
            [SerializeField] AudioCompressionFormat _compressionFormat;

            [SerializeField] bool _enableQuality;
            [SerializeField] float _quality;

            [SerializeField] bool _enableSampleRateSetting;
            [SerializeField] AudioSampleRateSetting _sampleRateSetting;

            [SerializeField] bool _enableSampleRateOverride;
            [SerializeField] uint _sampleRateOverride;

            public bool Check(AudioImporterSampleSettings settings) {
                if (_enableLoadType && settings.loadType != _loadType) return false;
                if (_enablePreloadAudioData && settings.preloadAudioData != _preloadAudioData) return false;
                if (_enableCompressionFormat && settings.compressionFormat != _compressionFormat) return false;
                if (_enableQuality && settings.quality != _quality) return false;
                if (_enableSampleRateSetting && settings.sampleRateSetting != _sampleRateSetting) return false;
                if (_enableSampleRateOverride && settings.sampleRateOverride != _sampleRateOverride) return false;
                return true;
            }

            public AudioImporterSampleSettings Apply(AudioImporterSampleSettings setting) {
                if (_enableLoadType) setting.loadType = _loadType;
                if (_enablePreloadAudioData) setting.preloadAudioData = _preloadAudioData;
                if (_enableCompressionFormat) setting.compressionFormat = _compressionFormat;
                if (_enableQuality) setting.quality = _quality;
                if (_enableSampleRateSetting) setting.sampleRateSetting = _sampleRateSetting;
                if (_enableSampleRateOverride) setting.sampleRateOverride = _sampleRateOverride;
                return setting;
            }
        }

        [SerializeField] StandardAudioImporterSampleSettings _defaultSampleSettings;

        public bool Check(AudioClip clip) {
            AudioImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(clip)) as AudioImporter;
            SerializedObject serializedObject = new SerializedObject(importer);

            if (_enableForceToMono && importer.forceToMono != _forceToMono) return false;
            if (_enableNormalize && serializedObject.FindProperty("m_Normalize").boolValue != _normalize) return false;
            if (_enableLoadInBackground && importer.loadInBackground != _loadInBackground) return false;
            if (_enableAmbisonic && importer.ambisonic != _ambisonic) return false;

            if (_defaultSampleSettings.Check(importer.defaultSampleSettings) == false) return false;

            return true;
        }

        public void Apply(AudioClip clip) {
            if (Check(clip)) return;
            AudioImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(clip)) as AudioImporter;
            ApplyToImporter(importer);
            importer.SaveAndReimport();
        }

        public void ApplyToImporter(AudioImporter importer) {
            SerializedObject serializedObject = new SerializedObject(importer);

            if (_enableForceToMono) importer.forceToMono = _forceToMono;
            if (_enableNormalize) {
                serializedObject.FindProperty("m_Normalize").boolValue = _normalize;
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
            if (_enableLoadInBackground) importer.loadInBackground = _loadInBackground;
            if (_enableAmbisonic) importer.ambisonic = _ambisonic;

            importer.defaultSampleSettings = _defaultSampleSettings.Apply(importer.defaultSampleSettings);
        }
    }
}