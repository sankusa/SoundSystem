using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class SoundManagementWindow : EditorWindow {
        [MenuItem(nameof(SoundSystem) + "/" + nameof(SoundManagementWindow))]
        static void Open() {
            GetWindow<SoundManagementWindow>();
        }
        
        [SerializeField] ClipView _clipView = new();
        [SerializeField] SoundContainerView _soundContainerView = new();

        [SerializeField] bool _showClipView = true;
        [SerializeField] bool _showSoundContainerView = true;

        LayoutSplitter _splitter1;

        void OnEnable() {
            titleContent = new GUIContent(nameof(SoundSystem), Skin.Instance.MainIcon);

            _clipView.OnEnable();
            _soundContainerView.OnEnable();

            _splitter1 = new(true, sessionStateKey: nameof(SoundManagementWindow) + "_splitter1") {
                ResizeHandleMouseAcceptRectOffset = new RectOffset(2, 2, 0, 0),
                HandleColor = new Color(0.1f, 0.1f, 0.1f),
                HandleWidth = 1,
            };

            OnProjectChanged();

            EditorApplication.projectChanged += OnProjectChanged;
        }

        void OnDisable() {
            _clipView.OnDisable();
            _soundContainerView.OnDisable();

            EditorApplication.projectChanged -= OnProjectChanged;
        }

        void OnGUI() {
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar)) {
                _showClipView = GUILayout.Toggle(_showClipView, new GUIContent(Icons.FolderIcon), EditorStyles.toolbarButton, GUILayout.Width(32));
                _showSoundContainerView = GUILayout.Toggle(_showSoundContainerView, new GUIContent(Skin.Instance.SoundContainerIcon), EditorStyles.toolbarButton, GUILayout.Width(32));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Command", EditorStyles.toolbarButton)) {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Generate SoundKey Class"), false, () => ScriptGenerator.GenerateSoundKeyScript());
                    menu.ShowAsContext();
                }
            }
            using (new GUILayout.HorizontalScope()) {
                bool useSplitter1 = _showClipView && _showSoundContainerView;
                if (useSplitter1) _splitter1.Begin();
                if (_showClipView) _clipView.OnGUI();
                if (useSplitter1) _splitter1.Split();
                if (_showSoundContainerView) _soundContainerView.OnGUI();
                if (useSplitter1) _splitter1.End();
            }
        }

        void OnProjectChanged() {
            _clipView.Reload();
            _soundContainerView.Reload(); 
        }
    }
}