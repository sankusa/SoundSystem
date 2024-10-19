using System;
using UnityEngine;

namespace SoundSystem {
    [Serializable]
    public class PlayRange {
        [SerializeField] bool _enable;
        public bool Enable => _enable;

        [SerializeField] int _fromSamples = 0;
        public int FromSamples => _fromSamples;

        [SerializeField] int _toSamples = int.MaxValue;
        public int ToSamples => _toSamples;
    }
}