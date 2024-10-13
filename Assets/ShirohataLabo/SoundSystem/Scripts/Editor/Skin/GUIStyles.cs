using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class GUIStyles {
        static GUIStyle _darkToolbar;
        public static GUIStyle DarkToolbar {
            get {
                if (_darkToolbar == null) {
                    _darkToolbar = new GUIStyle("ContentToolbar"/*EditorStyles.toolbar*/);

                    Texture2D backgroundTexture = new(1, 1);
                    backgroundTexture.SetPixel(0, 0, new Color(0, 0, 0, 0.5f));
                    backgroundTexture.Apply();

                    _darkToolbar.normal.background = backgroundTexture;
                }
                return _darkToolbar;
            }
        }

        static GUIStyle _captionLabel;
        public static GUIStyle CaptionLabel {
            get {
                if (_captionLabel == null) {
                    _captionLabel = new GUIStyle(EditorStyles.label);
                    _captionLabel.normal.textColor = new Color(0.9647059f, 0.4901961f, 0.0627451f);
                }
                return _captionLabel;
            }
        }

        static GUIStyle _audioUnitRowBackground;
        public static GUIStyle AudioUnitRowBackground {
            get {
                if (_audioUnitRowBackground == null) {
                    _audioUnitRowBackground = new GUIStyle("OL box NoExpand");

                    Texture2D backgroundTexture = new(1, 1);
                    backgroundTexture.SetPixel(0, 0, new Color(0, 0, 0, 0.3f));
                    backgroundTexture.Apply();

                    _audioUnitRowBackground.normal.background = backgroundTexture;
                }
                return _audioUnitRowBackground;
            }
        }

        static GUIStyle _categoryRowBackground;
        public static GUIStyle CategoryRowBackground {
            get {
                if (_categoryRowBackground == null) {
                    _categoryRowBackground = new GUIStyle("OL box NoExpand");

                    Texture2D backgroundTexture = new(1, 1);
                    backgroundTexture.SetPixel(0, 0, new Color(0, 0, 0, 0.6f));
                    backgroundTexture.Apply();

                    _categoryRowBackground.normal.background = backgroundTexture;
                }
                return _categoryRowBackground;
            }
        }

        static GUIStyle _soundRowBackground;
        public static GUIStyle SoundRowBackground {
            get {
                if (_soundRowBackground == null) {
                    _soundRowBackground = new GUIStyle("OL box NoExpand");

                    Texture2D backgroundTexture = new(1, 1);
                    backgroundTexture.SetPixel(0, 0, new Color(0, 0, 0, 0.3f));
                    backgroundTexture.Apply();

                    _soundRowBackground.normal.background = backgroundTexture;
                }
                return _soundRowBackground;
            }
        }
    }
}