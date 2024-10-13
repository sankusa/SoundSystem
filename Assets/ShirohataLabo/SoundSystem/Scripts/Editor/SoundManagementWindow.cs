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
        
        [SerializeField] AudioUnitView _audioUnitView = new();
        [SerializeField] SoundContainerView _soundView = new();

        [SerializeField] bool _showAudioUnitView = true;
        [SerializeField] bool _showSoundContainerView = true;

        LayoutSplitter _splitter;

        void OnEnable() {
            _audioUnitView.OnEnable();
            _soundView.OnEnable();

            _splitter = new(true, sessionStateKey: nameof(SoundManagementWindow) + "_splitter", handleColor: Color.gray);

            OnProjectChanged();

            EditorApplication.projectChanged += OnProjectChanged;
        }

        void OnDisable() {
            EditorApplication.projectChanged -= OnProjectChanged;
        }

        void OnGUI() {
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar)) {
                _showAudioUnitView = GUILayout.Toggle(_showAudioUnitView, "AudioUnit List", EditorStyles.toolbarButton);
                _showSoundContainerView = GUILayout.Toggle(_showSoundContainerView, "SoundContainer List", EditorStyles.toolbarButton);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Command", EditorStyles.toolbarButton)) {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Generate SoundKey Class"), false, () => ScriptGenerator.GenerateSoundKeyScript());
                    menu.ShowAsContext();
                }
            }
            using (new GUILayout.HorizontalScope()) {
                if (_showAudioUnitView && _showSoundContainerView) {
                    _splitter.Begin();
                }
                if (_showAudioUnitView) {
                    _audioUnitView.OnGUI();
                }
                if (_showAudioUnitView && _showSoundContainerView) {
                    _splitter.Split();
                }
                if (_showSoundContainerView) {
                    _soundView.OnGUI();
                }
                if (_showAudioUnitView && _showSoundContainerView) {
                    _splitter.End();
                }
            }
        }

        void OnProjectChanged() {
            _audioUnitView.Reload();
            _soundView.Reload(); 
        }
    }
}