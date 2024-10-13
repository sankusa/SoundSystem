using System;
using UnityEngine;

namespace SoundSystem {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ShowOnlyChildAttribute : PropertyAttribute {
        public ShowOnlyChildAttribute() {}
    }
}