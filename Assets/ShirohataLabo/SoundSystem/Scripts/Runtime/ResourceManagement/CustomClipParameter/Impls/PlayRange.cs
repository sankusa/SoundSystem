using System;
using UnityEngine;

namespace SoundSystem.CustomClipParameters {
    [Serializable]
    public class PlayRange : CustomClipParameter {
        [SerializeField] int _fromSamples = 0;
        public int FromSamples => _fromSamples;

        [SerializeField] int _toSamples = int.MaxValue;
        public int ToSamples => _toSamples;
    }
}