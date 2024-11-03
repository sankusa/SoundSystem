using UnityEngine;
using System;

namespace SoundSystem {
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class SoundKeyAttribute : PropertyAttribute {}
}