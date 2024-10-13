using UnityEditor;

namespace SoundSystem {
    [CustomEditor(typeof(AudioUnitCategory))]
    public class AudioUnitCategoryInspector : Editor {
        public override void OnInspectorGUI() {
            AudioUnitCategory category = target as AudioUnitCategory;

            EditorGUI.BeginChangeCheck();
            AudioUnitCategory newParent = 
                EditorGUILayout.ObjectField("Parent", category.Parent, typeof(AudioUnitCategory), false)
                as AudioUnitCategory;
            if (EditorGUI.EndChangeCheck()) {
                category.SetParentWithUndo(newParent);
            }
        }
    }
}