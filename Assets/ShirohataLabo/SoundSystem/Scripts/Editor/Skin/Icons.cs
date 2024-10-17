using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public static class Icons {
        static Texture2D _refleshIcon;
        public static Texture2D RefleshIcon {
            get {
                if (_refleshIcon == null) {
                    _refleshIcon = (Texture2D)EditorGUIUtility.Load("d_Refresh");
                }
                return _refleshIcon;
            }
        }

        static Texture2D _commandIcon;
        public static Texture2D CommandIcon {
            get {
                if (_commandIcon == null) {
                    _commandIcon = (Texture2D)EditorGUIUtility.Load("d_more");
                }
                return _commandIcon;
            }
        }

        static Texture2D _okIcon;
        public static Texture2D OkIcon {
            get {
                if (_okIcon == null) {
                    _okIcon = (Texture2D)EditorGUIUtility.Load("d_P4_CheckOutRemote");
                }
                return _okIcon;
            }
        }

        static Texture2D _ngIcon;
        public static Texture2D NgIcon {
            get {
                if (_ngIcon == null) {
                    _ngIcon = (Texture2D)EditorGUIUtility.Load("d_P4_DeletedLocal");
                }
                return _ngIcon;
            }
        }
    }
}