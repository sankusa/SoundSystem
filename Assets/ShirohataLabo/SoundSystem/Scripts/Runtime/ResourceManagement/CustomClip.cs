using SoundSystem.CustomClipParameters;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SoundSystem {
    [CreateAssetMenu(fileName = nameof(CustomClip), menuName = nameof(SoundSystem) + "/" + nameof(CustomClip))]
    public class CustomClip : ScriptableObject {
        public enum VolumeType {
            Constant = 0,
            Curve = 1,
        }

        [SerializeField] AudioClip _audioClip;
        public AudioClip AudioClip {
            get => _audioClip;
            set => _audioClip = value;
        }

        [SerializeField] VolumeMultiplier _volumeMultiplier;

        [SerializeField] VolumeMultiplierCurve _volumeMultiplierCurve;

        [SerializeField] PitchMultiplier _pitchMultiplier;

        [SerializeField] PlayRange _playRange;
        public PlayRange PlayRange => _playRange;

        public float GetVolumeMultiplier(float time) {
            float volume = 1;
            if (_volumeMultiplier.Enable) volume *= _volumeMultiplier.Value;
            if (_volumeMultiplierCurve.Enable) volume *= _volumeMultiplierCurve.Curve.Evaluate(time);
            return Mathf.Clamp01(volume);
        }

        public float GetPitchMultiplier() {
            return _pitchMultiplier.Enable ? _pitchMultiplier.Value : 1;
        }

#if UNITY_EDITOR
        public static SerializedProperty GetVolumeMultiplierProp(SerializedObject serializedObject) {
            return serializedObject.FindProperty(nameof(_volumeMultiplier));
        }

        public static SerializedProperty GetVolumeMultiplierCurveProp(SerializedObject serializedObject) {
            return serializedObject.FindProperty(nameof(_volumeMultiplierCurve));
        }

        public static SerializedProperty GetPitchMultiplierProp(SerializedObject serializedObject) {
            return serializedObject.FindProperty(nameof(_pitchMultiplier));
        }

        public static SerializedProperty GetPlayRangeProp(SerializedObject serializedObject) {
            return serializedObject.FindProperty(nameof(_playRange));
        }

        public static void ResetValueBasedOnAudioClip(SerializedObject serializedObject) {
            AudioClip clip = serializedObject.FindProperty(nameof(_audioClip)).objectReferenceValue as AudioClip;

            var volumeMultiplierProp = GetVolumeMultiplierProp(serializedObject);
            var volumeMultiplier_ValueProp = volumeMultiplierProp.FindPropertyRelative("_value");

            var volumeMultiplierCurveProp = GetVolumeMultiplierCurveProp(serializedObject);
            var volumeMultiplierCurve_CurveProp = volumeMultiplierCurveProp.FindPropertyRelative("_curve");

            var pitchMultiplierProp = GetPitchMultiplierProp(serializedObject);
            var pitchMultiplier_ValueProp = pitchMultiplierProp.FindPropertyRelative("_value");

            var playRangeProp = GetPlayRangeProp(serializedObject);
            var playRange_FromSamplesProp = playRangeProp.FindPropertyRelative("_fromSamples");
            var playrange_ToSamplesProp = playRangeProp.FindPropertyRelative("_toSamples");

            volumeMultiplier_ValueProp.floatValue = 1;
            if (clip == null) {
                volumeMultiplierCurve_CurveProp.animationCurveValue = new AnimationCurve();
            }
            else {
                volumeMultiplierCurve_CurveProp.animationCurveValue = new AnimationCurve(new Keyframe(0, 1), new Keyframe(clip.length, 1));
            }
            pitchMultiplier_ValueProp.floatValue = 1;
            playRange_FromSamplesProp.intValue = 0;
            if (clip == null) {
                playrange_ToSamplesProp.intValue = 0;
            }
            else {
                playrange_ToSamplesProp.intValue = clip.samples;
            }
        }
#endif
    }
}