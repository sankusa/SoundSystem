using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class SoundContainerListWindow : EditorWindow {
        [MenuItem("ShirohataLabo/" + nameof(SoundSystem) + "/" + nameof(SoundContainerListWindow))]
        static void Open() {
            GetWindow<SoundContainerListWindow>();
        }
        
        [SerializeField] SoundContainerView _soundContainerView = new();

        void OnEnable() {
            titleContent = new GUIContent("SoundContainer", Skin.Instance.MainIcon);
            _soundContainerView.OnEnable();

            OnProjectChanged();

            EditorApplication.projectChanged += OnProjectChanged;
        }

        void OnDisable() {
            _soundContainerView.OnDisable();

            EditorApplication.projectChanged -= OnProjectChanged;
        }

        void OnGUI() {
            _soundContainerView.OnGUI();
        }

        void OnProjectChanged() {
            _soundContainerView.Reload(); 
        }
    }
}