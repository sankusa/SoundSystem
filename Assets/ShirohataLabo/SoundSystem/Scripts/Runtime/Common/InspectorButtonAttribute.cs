using System;
using UnityEngine;

namespace SoundSystem {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class InspectorButtonAttribute : PropertyAttribute {
        public InspectorButtonAttribute() {}
    }
}