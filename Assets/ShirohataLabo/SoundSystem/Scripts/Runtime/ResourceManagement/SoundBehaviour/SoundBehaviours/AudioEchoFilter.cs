using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(AudioEchoFilter), 1002)]
    public class AudioEchoFilter : SoundBehaviour {
        [SerializeField] bool _enable = true;
        public bool Enable {
            get => _enable;
            set => _enable = value;
        }

        [SerializeField, Range(10, 5000)] float _delay = 500;
        public float Delay {
            get => _delay;
            set => _delay = value;
        }

        [SerializeField, Range(0, 1)] float _decayRatio = 0.5f; 
        public float DecayRatio {
            get => _decayRatio;
            set => _decayRatio = value;
        }

        [SerializeField, Range(0, 1)] float _dryMix = 1f;
        public float DryMix {
            get => _dryMix;
            set => _dryMix = value;
        }

        [SerializeField, Range(0, 1)] float _wetMix = 1f;
        public float WetMix {
            get => _wetMix;
            set => _wetMix = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            AudioEchoFilterAccessor accessor = GetOrCreateAudioEchoFilter(player);
            accessor.Enable = _enable;
            accessor.Delay = _delay;
            accessor.DryMix = _dryMix;
            accessor.WetMix = _wetMix;
        }
    }
}