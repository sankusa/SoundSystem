using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CreateAssetMenu(fileName = nameof(SoundContainerPreloader), menuName = nameof(SoundSystem) + "/Develop/" + nameof(SoundContainerPreloader))]
    public class SoundContainerPreloader : ScriptableObject {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Load() {
            SoundContainerPreloader instance = Resources.Load<SoundContainerPreloader>(nameof(SoundContainerPreloader));
            if (instance == null) {
                Debug.LogError($"\"Resources/{nameof(SoundContainerPreloader)}\" not found");
                return;
            }

            SoundCache.Instance.AddContainers(instance._containers);
        }

        [SerializeField] List<SoundContainer> _containers = new();

#if UNITY_EDITOR
        static SoundContainerPreloader _instanceForEditor;
        public static SoundContainerPreloader InstanceForEditor {
            get {
                if (_instanceForEditor == null) {
                    _instanceForEditor = LoadForEditor();
                }
                return _instanceForEditor;
            }
        }

        static SoundContainerPreloader LoadForEditor() {
            string guid = AssetDatabase.FindAssets($"t:{typeof(SoundContainerPreloader)}").FirstOrDefault();
            if (guid == null) {
                Debug.LogError($"{nameof(SoundContainerPreloader)} must exist under Resources folder");
                return null;
            }
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<SoundContainerPreloader>(assetPath);
        }

        public bool ContainsForEditor(SoundContainer container) {
            return _containers.Contains(container);
        }

        public void UpdatePreloadStateForEditor(SoundContainer container, bool preload) {
            if (preload) {
                AddForEditor(container);
            }
            else {
                RemoveForEditor(container);
            }
        }

        public void AddForEditor(SoundContainer container) {
            if (ContainsForEditor(container)) return;
            Undo.RecordObject(this, $"Change {nameof(SoundContainerPreloader)}");
            RemoveNullForEditor();
            _containers.Add(container);
            EditorUtility.SetDirty(this);
        }

        public void RemoveForEditor(SoundContainer container) {
            if (ContainsForEditor(container) == false) return;
            Undo.RecordObject(this, $"Change {nameof(SoundContainerPreloader)}");
            RemoveNullForEditor();
            _containers.Remove(container);
            EditorUtility.SetDirty(this);
        }

        void RemoveNullForEditor() {
            for (int i = _containers.Count - 1; i >= 0; i--) {
                if (_containers[i] == null) {
                    _containers.RemoveAt(i);
                }
            }
        }
#endif
    }
}