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

        static Texture2D _plusIcon;
        public static Texture2D PlusIcon {
            get {
                if (_plusIcon == null) {
                    _plusIcon = (Texture2D)EditorGUIUtility.Load("d_Toolbar Plus");
                }
                return _plusIcon;
            }
        }

        static Texture2D _minusIcon;
        public static Texture2D MinusIcon {
            get {
                if (_minusIcon == null) {
                    _minusIcon = (Texture2D)EditorGUIUtility.Load("d_Toolbar Minus");
                }
                return _minusIcon;
            }
        }
        
        static Texture2D _playIcon;
        public static Texture2D PlayIcon {
            get {
                if (_playIcon == null) {
                    _playIcon = (Texture2D)EditorGUIUtility.IconContent("PlayButton").image;
                }
                return _playIcon;
            }
        }

        static Texture2D _pauseIcon;
        public static Texture2D PauseIcon {
            get {
                if (_pauseIcon == null) {
                    _pauseIcon = (Texture2D)EditorGUIUtility.IconContent("PauseButton").image;
                }
                return _pauseIcon;
            }
        }

        static Texture2D _repeatIcon;
        public static Texture2D RepeatIcon {
            get {
                if (_repeatIcon == null) {
                    _repeatIcon = (Texture2D)EditorGUIUtility.IconContent("preAudioLoopOff").image;
                }
                return _repeatIcon;
            }
        }

        static Texture2D _autoPlayIcon;
        public static Texture2D AutoPlayIcon {
            get {
                if (_autoPlayIcon == null) {
                    _autoPlayIcon = (Texture2D)EditorGUIUtility.IconContent("preAudioAutoPlayOff").image;
                }
                return _autoPlayIcon;
            }
        }
    }
}