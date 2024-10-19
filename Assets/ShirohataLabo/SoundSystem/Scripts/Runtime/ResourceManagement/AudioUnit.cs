using System;
using SoundSystem.AudioUnitEffects;
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