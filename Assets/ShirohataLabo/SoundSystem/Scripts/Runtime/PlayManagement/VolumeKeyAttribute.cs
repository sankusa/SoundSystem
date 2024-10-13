using System;
using UnityEngine;

namespace SoundSystem {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class VolumeKeyAttribute : PropertyAttribute {
        public VolumeKeyAttribute() {}
    }
}