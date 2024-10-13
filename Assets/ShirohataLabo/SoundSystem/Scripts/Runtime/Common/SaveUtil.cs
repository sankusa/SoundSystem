using UnityEngine;

namespace SoundSystem {
    public static class SaveUtil {
        public static void SaveFloat(string key, float value) {
            PlayerPrefs.SetFloat(key, value);
        }
        public static float LoadFloat(string key) {
            return PlayerPrefs.GetFloat(key);
        }

        public static void SaveBool(string key, bool value) {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }
        public static bool LoadBool(string key) {
            return PlayerPrefs.GetInt(key) == 1;
        }

        public static bool HasKey(string key) {
            return PlayerPrefs.HasKey(key);
        }
    }
}