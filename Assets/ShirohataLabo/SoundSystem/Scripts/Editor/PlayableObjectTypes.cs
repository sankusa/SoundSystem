using System;
using UnityEngine;

namespace SoundSystem {
    public static class PlayableObjectTypes {
        static Type[] _types = new Type[] {typeof(AudioClip), typeof(CustomClip)};
        public static Type[] Types => _types;
    }
}