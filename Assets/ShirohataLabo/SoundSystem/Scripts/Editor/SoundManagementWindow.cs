using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    public class SoundManagementWindow : EditorWindow {
        [MenuItem(nameof(SoundSystem) + "/" + nameof(SoundManagementWindow))]
        static void Open() {
            GetWindow<SoundManagementWindow>().titleContent = new GUIContent(nameof(SoundSystem), Skin.Instance.MainIcon);
        }
        
        [SerializeField] AudioClipView _audioClipView = new();
        [SerializeField] AudioUnitView _audioUnitView = new();
        [SerializeField] SoundContainerView _soundContainerView = new();

        [SerializeField] bool _showAudioClipView = true;
        [SerializeField] bool _showAudioUnitView = true;
        [SerializeField] bool _showSoundContainerView = true;

        LayoutSplitter _splitter1;
        LayoutSplitter _splitter2;

        void OnEnable() {
            _audioClipView.OnEnable();
            _audioUnitView.OnEnable();
            _soundContainerView.OnEnable();

            _splitter1 = new(true, sessionStateKey: nameof(SoundManagementWindow) + "_splitter1") {
                ResizeHandleMouseAcceptRectOffset = new RectOffset(),
                HandleColor = Color.gray,
            };
            _splitter2 = new(true, sessionStateKey: nameof(SoundManagementWindow) + "_splitter2") {
                ResizeHandleMouseAcceptRectOffset = new RectOffset(),
                HandleColor = Color.gray,
            };

            OnProjectChanged();

            EditorApplication.projectChanged += OnProjectChanged;
        }

        void OnDisable() {
            _audioUnitView.OnDisable();
            _soundContainerView.OnDisable();

            EditorApplication.projectChanged -= OnProjectChanged;
        }

        void OnGUI() {
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar)) {
                _showAudioClipView = GUILayout.Toggle(_showAudioClipView, "AudioClip Tree", EditorStyles.toolbarButton);
                _showAudioUnitView = GUILayout.Toggle(_showAudioUnitView, "AudioUnit Tree", EditorStyles.toolbarButton);
                _showSoundContainerView = GUILayout.Toggle(_showSoundContainerView, "SoundContainer List", EditorStyles.toolbarButton);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Command", EditorStyles.toolbarButton)) {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Generate SoundKey Class"), false, () => ScriptGenerator.GenerateSoundKeyScript());
                    menu.ShowAsContext();
                }
            }
            using (new GUILayout.HorizontalScope()) {
                bool useSplitter1 = _showAudioClipView && _showAudioUnitView;
                bool useSplitter2 = (_showAudioUnitView || _showAudioClipView) && _showSoundContainerView;
                if (useSplitter2) _splitter2.Begin();
                if (useSplitter1) _splitter1.Begin();
                if (_showAudioClipView) _audioClipView.OnGUI();
                if (useSplitter1) _splitter1.Split();
                if (_showAudioUnitView) _audioUnitView.OnGUI();
                if (useSplitter1) _splitter1.End();
                if (useSplitter2) _splitter2.Split();
                if (_showSoundContainerView) _soundContainerView.OnGUI();
                if (useSplitter2) _splitter2.End();

            }
        }

        void OnProjectChanged() {
            _audioClipView.Reload();
            _audioUnitView.Reload();
            _soundContainerView.Reload(); 
        }
    }
}