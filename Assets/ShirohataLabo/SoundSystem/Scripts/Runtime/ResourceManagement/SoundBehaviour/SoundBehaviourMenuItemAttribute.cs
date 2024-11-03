using System;

namespace SoundSystem {
    [AttributeUsage(AttributeTargets.Class)]
    public class SoundBehaviourMenuItemAttribute : Attribute {
        public string MenuName { get; }
        public int Priority { get; }

        public SoundBehaviourMenuItemAttribute(string menuName, int priority = 0) {
            MenuName = menuName;
            Priority = priority;
        }
    }
}