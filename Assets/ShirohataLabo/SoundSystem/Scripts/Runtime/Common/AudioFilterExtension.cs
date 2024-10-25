using UnityEngine;

namespace SoundSystem {
    public static class AudioFilterExtension {
        public static void DisableAndReset(this AudioHighPassFilter filter) {
            if (filter == null) return;
            filter.enabled = false;
            filter.cutoffFrequency = 5000f;
            filter.highpassResonanceQ = 1;
        }

        public static void DisableAndReset(this AudioLowPassFilter filter) {
            if (filter == null) return;
            filter.enabled = false;
            filter.cutoffFrequency = 5007.7f;
            filter.lowpassResonanceQ = 1;
        }

        public static void DisableAndReset(this AudioEchoFilter filter) {
            if (filter == null) return;
            filter.enabled = false;
            filter.delay = 500;
            filter.decayRatio = 0.5f;
            filter.dryMix = 1;
            filter.wetMix = 1;
        }

        public static void DisableAndReset(this AudioDistortionFilter filter) {
            if (filter == null) return;
            filter.enabled = false;
            filter.distortionLevel = 0.5f;
        }

        public static void DisableAndReset(this AudioReverbFilter filter) {
            if (filter == null) return;
            filter.enabled = false;
            filter.reverbPreset = AudioReverbPreset.Off;
        }

        public static void DisableAndReset(this AudioChorusFilter filter) {
            if (filter == null) return;
            filter.enabled = false;
            filter.dryMix = 0.5f;
            filter.wetMix1 = 0.5f;
            filter.wetMix2 = 0.5f;
            filter.wetMix3 = 0.5f;
            filter.delay = 40;
            filter.rate = 0.8f;
            filter.depth = 0.03f;
        }

        public static void DisableAndReset(this AudioReverbZone filter) {
            if (filter == null) return;
            filter.enabled = false;
            filter.minDistance = 10f;
            filter.maxDistance = 15f;
            filter.reverbPreset = AudioReverbPreset.Generic;
        }
    }
}