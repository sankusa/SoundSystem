using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SoundSystem {
    public abstract class ComponentAccessorBase<T> where T : Component {
        protected GameObject GameObject { get; }

        public T Component { get; private set; }
        public bool HasComponent { get; private set; }

        bool _destroying;

        public ComponentAccessorBase(GameObject gameObject) {
            GameObject = gameObject;
        }

        public void CreateFilterIfNull() {
            if (HasComponent == false) {
                // Destroyが実際にObjectを破棄するのはフレームの最後なので
                // Destroyを呼んだフレーム内でAddComponentしようとするとコンポーネントが重複する(重複不可の場合追加できない)
                // さらに、そのフレームの終わりに元のコンポーネントがDestroyされてしまう
                // 回避策として、Destroy呼ぶ場合はフレームの最後まで遅延させて、同フレーム内ならキャンセル可能にする
                if (_destroying) {
                    _destroying = false;
                }
                else {
                    Component = GameObject.AddComponent<T>();
                }
                SetDefault();
                Apply();
                HasComponent = true;
            }
        }

        public void ApplyIfChanged() {
            if (HasComponent) {
                ApplyIfChangedMain();
            }
            CopyToOld();
        }
        protected abstract void ApplyIfChangedMain();

        public void Reset() {
            if (HasComponent == false) return;
            SetDefault();
        }

        public void Clear() {
            DestroyComopnent();
            SetDefault();
            CopyToOld();
        }

        protected void Apply() {
            if (HasComponent) {
                ApplyMain();
            }
            CopyToOld();
        }
        protected abstract void ApplyMain();

        protected abstract void SetDefault();
        protected abstract void CopyToOld();

        void DestroyComopnent() {
            if (HasComponent == false) return;
#if UNITY_EDITOR
            if (EditorApplication.isPlaying == false) {
                Object.DestroyImmediate(Component);
            }
            else
#endif
            {
                // 他に使えるMonoBehaviourがないので
                SoundManager.Instance.StartCoroutine(DestroyOnEndOfFrameCoroutine());
            }

            HasComponent = false;
        }

        // Destroyを呼ぶタイミングをフレームの最後まで遅らせて、フレーム内ならキャンセル可能にする
        IEnumerator DestroyOnEndOfFrameCoroutine() {
            _destroying = true;
            yield return null;
            if (_destroying) {
                Object.Destroy(Component);
                _destroying = false;
            }
        }
    }
}