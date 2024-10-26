using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    public class SoundManagementWindow : EditorWindow {
        [MenuItem(nameof(SoundSystem) + "/" + nameof(SoundManagementWindow))]
        static void Open() {
            GetWindow<SoundManagementWindow>();
        }
        
        [SerializeField] AudioClipView _audioClipView = new();
        [SerializeField] CustomClipView _customClipView = new();
        [SerializeField] SoundContainerView _soundContainerView = new();

        [SerializeField] bool _showAudioClipView = true;
        [SerializeField] bool _showcustomClipView = true;
        [SerializeField] bool _showSoundContainerView = true;

        LayoutSplitter _splitter1;
        LayoutSplitter _splitter2;

        void OnEnable() {
            titleContent = new GUIContent(nameof(SoundSystem), Skin.Instance.MainIcon);

            _audioClipView.OnEnable();
            _customClipView.OnEnable();
            _soundContainerView.OnEnable();

            _splitter1 = new(true, sessionStateKey: nameof(SoundManagementWindow) + "_splitter1") {
                ResizeHandleMouseAcceptRectOffset = new RectOffset(2, 2, 0, 0),
                HandleColor = new Color(0.1f, 0.1f, 0.1f),
                HandleWidth = 1,
            };
            _splitter2 = new(true, sessionStateKey: nameof(SoundManagementWindow) + "_splitter2") {
                ResizeHandleMouseAcceptRectOffset = new RectOffset(2, 2, 0, 0),
                HandleColor = new Color(0.1f, 0.1f, 0.1f),
                HandleWidth = 1,
            };

            OnProjectChanged();

            EditorApplication.projectChanged += OnProjectChanged;
        }

        void OnDisable() {
            _customClipView.OnDisable();
            _soundContainerView.OnDisable();

            EditorApplication.projectChanged -= OnProjectChanged;
        }

        void OnGUI() {
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar)) {
                _showAudioClipView = GUILayout.Toggle(_showAudioClipView, new GUIContent(Icons.AudioClipIcon), EditorStyles.toolbarButton, GUILayout.Width(32));
                _showcustomClipView = GUILayout.Toggle(_showcustomClipView, new GUIContent(Skin.Instance.CustomClipIcon), EditorStyles.toolbarButton, GUILayout.Width(32));
                _showSoundContainerView = GUILayout.Toggle(_showSoundContainerView, new GUIContent(Skin.Instance.SoundContainerIcon), EditorStyles.toolbarButton, GUILayout.Width(32));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Command", EditorStyles.toolbarButton)) {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Generate SoundKey Class"), false, () => ScriptGenerator.GenerateSoundKeyScript());
                    menu.ShowAsContext();
                }
            }
            using (new GUILayout.HorizontalScope()) {
                bool useSplitter1 = _showAudioClipView && _showcustomClipView;
                bool useSplitter2 = (_showcustomClipView || _showAudioClipView) && _showSoundContainerView;
                if (useSplitter2) _splitter2.Begin();
                if (useSplitter1) _splitter1.Begin();
                if (_showAudioClipView) _audioClipView.OnGUI();
                if (useSplitter1) _splitter1.Split();
                if (_showcustomClipView) _customClipView.OnGUI();
                if (useSplitter1) _splitter1.End();
                if (useSplitter2) _splitter2.Split();
                if (_showSoundContainerView) _soundContainerView.OnGUI();
                if (useSplitter2) _splitter2.End();

            }
        }

        void OnProjectChanged() {
            _audioClipView.Reload();
            _customClipView.Reload();
            _soundContainerView.Reload(); 
        }
    }
}