using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CreateAssetMenu(fileName = nameof(AudioUnit), menuName = nameof(SoundSystem) + "/" + nameof(AudioUnit))]
    public class AudioUnit : ScriptableObject {
        public enum VolumeType {
            Constant = 0,
            Curve = 1,
        }

        [SerializeField] AudioUnitCategory _category;
        public AudioUnitCategory Category => _category;

        [SerializeField] AudioClip _clip;
        public AudioClip Clip {
            get => _clip;
            set => _clip = value;
        }

        [SerializeField] VolumeType _volumeOption = VolumeType.Constant;
        public VolumeType VolumeOption => _volumeOption;

        [SerializeField, Range(0, 1)] float _constantVolume = 1f;
        public float ConstantVolume => _constantVolume;

        [SerializeField] AnimationCurve _volumeCurve = new();
        public AnimationCurve VolumeCurve => _volumeCurve;

        [SerializeField] private float _pitch = 1f;
        public float Pitch => _pitch;

        [SerializeField] private float _start = 0;
        public float Start => _start;

        [SerializeField] private float _end = float.MaxValue;
        public float End => _end;

        public float GetCurrentVolume(float time) {
            if (VolumeOption == VolumeType.Constant) {
                return _constantVolume;
            }
            else if (VolumeOption == VolumeType.Curve) {
                return _volumeCurve.Evaluate(time);
            }
            return 1;
        }

#if UNITY_EDITOR
        public void SetCategoryWithUndo(AudioUnitCategory category, bool withoutUndo = false) {
            if (withoutUndo) {
                _category = category;
            }
            else {
                Undo.RecordObject(this, $"Change {nameof(AudioUnit)}");
                _category = category;
                EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}