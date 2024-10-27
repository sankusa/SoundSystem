using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class GUIStyles {
        static GUIStyle _richTextLabel;
        public static GUIStyle RichTextLabel {
            get {
                if (_richTextLabel == null) {
                    _richTextLabel = new GUIStyle(EditorStyles.label);
                    _richTextLabel.richText = true;
                }
                return _richTextLabel;
            }
        }

        static GUIStyle _simpleBox;
        public static GUIStyle SimpleBox {
            get {
                if (_simpleBox == null) {
                    _simpleBox = new GUIStyle("GroupBox");
                    _simpleBox.margin = new RectOffset();
                    _simpleBox.padding = new RectOffset(6, 6, 6, 6);
                }
                return _simpleBox;
            }
        }

        static GUIStyle _darkSimpleBox;
        public static GUIStyle DarkSimpleBox {
            get {
                if (_darkSimpleBox == null) {
                    _darkSimpleBox = new GUIStyle("GroupBox");
                    _darkSimpleBox.margin = new RectOffset();
                    _darkSimpleBox.padding = new RectOffset(6, 6, 6, 6);

                    Texture2D backgroundTexture = new(1, 1);
                    backgroundTexture.SetPixel(0, 0, new Color(0, 0, 0, 0.4f));
                    backgroundTexture.Apply();

                    _darkSimpleBox.normal.background = backgroundTexture;
                }
                return _darkSimpleBox;
            }
        }

        static GUIStyle _invisibleButton;
        public static GUIStyle InvisibleButton {
            get {
                if (_invisibleButton == null) {
                    _invisibleButton = new GUIStyle("RL FooterButton");
                    _invisibleButton.margin = new RectOffset();
                    _invisibleButton.padding = new RectOffset();
                }
                return _invisibleButton;
            }
        }

        static GUIStyle _darkToolbar;
        public static GUIStyle DarkToolbar {
            get {
                if (_darkToolbar == null) {
                    _darkToolbar = new GUIStyle("ContentToolbar"/*EditorStyles.toolbar*/);

                    Texture2D backgroundTexture = new(1, 1);
                    backgroundTexture.SetPixel(0, 0, new Color(0, 0, 0, 0.4f));
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
                    _captionLabel.normal.textColor = Colors.ThemeColor;
                }
                return _captionLabel;
            }
        }

        static GUIStyle _basicRowBackground;
        public static GUIStyle BasicRowBackground {
            get {
                if (_basicRowBackground == null) {
                    _basicRowBackground = new GUIStyle("OL box NoExpand");

                    Texture2D backgroundTexture = new(1, 1);
                    backgroundTexture.SetPixel(0, 0, new Color(0.05f, 0.05f, 0.05f, 0.2f));
                    backgroundTexture.Apply();

                    _basicRowBackground.normal.background = backgroundTexture;
                }
                return _basicRowBackground;
            }
        }

        static GUIStyle _folderRowBackground;
        public static GUIStyle FolderRowBackground {
            get {
                if (_folderRowBackground == null) {
                    _folderRowBackground = new GUIStyle("OL box NoExpand");

                    Texture2D backgroundTexture = new(1, 1);
                    backgroundTexture.SetPixel(0, 0, new Color(0, 0, 0, 0.4f));
                    backgroundTexture.Apply();

                    _folderRowBackground.normal.background = backgroundTexture;
                }
                return _folderRowBackground;
            }
        }

        static GUIStyle _soundContainerRowBackground;
        public static GUIStyle SoundContainerRowBackground {
            get {
                if (_soundContainerRowBackground == null) {
                    _soundContainerRowBackground = new GUIStyle("OL box NoExpand");

                    Texture2D backgroundTexture = new(1, 1);
                    backgroundTexture.SetPixel(0, 0, new Color(0, 0, 0, 0.4f));
                    backgroundTexture.Apply();

                    _soundContainerRowBackground.normal.background = backgroundTexture;
                }
                return _soundContainerRowBackground;
            }
        }

        static GUIStyle _soundRowBackground;
        public static GUIStyle SoundRowBackground {
            get {
                if (_soundRowBackground == null) {
                    _soundRowBackground = new GUIStyle("OL box NoExpand");

                    Texture2D backgroundTexture = new(1, 1);
                    backgroundTexture.SetPixel(0, 0, new Color(0.05f, 0.05f, 0.05f, 0.2f));
                    backgroundTexture.Apply();

                    _soundRowBackground.normal.background = backgroundTexture;
                }
                return _soundRowBackground;
            }
        }
    }
}