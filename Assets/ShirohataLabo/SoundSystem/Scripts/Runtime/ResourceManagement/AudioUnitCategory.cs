using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SoundSystem {
    [CreateAssetMenu(fileName = nameof(AudioUnitCategory), menuName = nameof(SoundSystem) + "/" + nameof(AudioUnitCategory))]
    // 循環参照対策のため、SetParentを通さずにParentをセットするのは禁止
    public class AudioUnitCategory : ScriptableObject {
        [SerializeField] AudioUnitCategory _parent;
        public AudioUnitCategory Parent => _parent;

#if UNITY_EDITOR
        public void SetParentWithUndo(AudioUnitCategory newParent) {
            int depth = 1;
            AudioUnitCategory category = newParent;
            // 親を辿っていって自分が見つかったら循環参照のため代入しない
            // 本関数実行前から循環参照が発生していない限りは無限ループにはならないが、念のため探索の深さは100まで
            while (category != null && depth <= 100) {
                if (category == this) return;
                category = category.Parent;
                depth++;
            }
            Undo.RecordObject(this, $"Change {nameof(AudioUnitCategory)}");
            _parent = newParent;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}