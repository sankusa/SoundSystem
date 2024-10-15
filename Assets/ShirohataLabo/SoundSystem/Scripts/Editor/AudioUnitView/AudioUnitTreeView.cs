using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    [System.Serializable]
    public class AudioUnitTreeView : TreeView {
        const string DragDataKey = nameof(SoundSystem) + "_" + nameof(AudioUnitTreeView) + "_DragData";
        List<AudioUnit> _audioUnits;

        public AudioUnitTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader) {
            rowHeight = 16;
            // showAlternatingRowBackgrounds = true;
            // showBorder = true;
            // enableItemHovering = true;
            // depthIndentWidth = 18;
        }

        public void Reload(List<AudioUnit> audioUnits) {
            _audioUnits = audioUnits;
            Reload();
        }

        protected override TreeViewItem BuildRoot() {
            TreeViewItem root = new() {depth = -1};

            int id = 0;

            string folderPath = SoundSystemSetting.Instance.ManagedFolderRootPath;
            AudioUnitTreeViewItem_Folder folderItem = new AudioUnitTreeViewItem_Folder(++id, folderPath);
            root.AddChild(folderItem);

            BuildItemRecursive(folderItem, ref id);

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        void BuildItemRecursive(AudioUnitTreeViewItem_Folder parentFolderItem, ref int id) {
            string folderPath = parentFolderItem.FolderPath;
            foreach (string guid in AssetDatabase.FindAssets("", new string[] {folderPath})) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                // フォルダパス以降に'/'が入っていたら直下ではない
                if (assetPath.Substring(folderPath.Length + 1).IndexOf('/') != -1) continue;

                if (AssetDatabase.IsValidFolder(assetPath)) {
                    AudioUnitTreeViewItem_Folder folderItem = new AudioUnitTreeViewItem_Folder(++id, assetPath);
                    parentFolderItem.AddChild(folderItem);
                    BuildItemRecursive(folderItem, ref id);
                    continue;
                }
                
                AudioUnit audioUnit = AssetDatabase.LoadAssetAtPath<AudioUnit>(assetPath);
                if (audioUnit != null) {
                    parentFolderItem.AddChild(new AudioUnitTreeViewItem_AudioUnit(++id, audioUnit));
                }
            }
        }

        protected override void RowGUI(RowGUIArgs args) {
            Rect rect = new(args.rowRect) {xMin = args.rowRect.xMin + GetContentIndent(args.item)};
            if (args.item is AudioUnitTreeViewItem_AudioUnit unitItem) {
                Rect backRect = new Rect(rect) {xMin = rect.xMin - 16};
                GUI.Box(backRect, "", GUIStyles.AudioUnitRowBackground);

                string assetPath = AssetDatabase.GetAssetPath(unitItem.AudioUnit);
                Texture icon = AssetDatabase.GetCachedIcon(assetPath);
                EditorGUI.LabelField(rect, new GUIContent(unitItem.Label, icon));
            }
            else if(args.item is AudioUnitTreeViewItem_Folder folderItem) {
                Rect backRect = new Rect(rect) {xMin = rect.xMin - 16};
                GUI.Box(backRect, "", GUIStyles.FolderRowBackground);

                Texture icon = AssetDatabase.GetCachedIcon(folderItem.FolderPath);
                EditorGUI.LabelField(rect, new GUIContent(folderItem.FolderName, icon));
            }
            else {
                base.RowGUI(args);
            }
        }

        protected override void SingleClickedItem(int id) {
            TreeViewItem item = FindItem(id, rootItem);
            if (item is AudioUnitTreeViewItem_AudioUnit unitItem) {
                unitItem.OnSingleClick();
            }
            else if (item is AudioUnitTreeViewItem_Folder folderItem) {
                folderItem.OnSingleClick();
            }
        }

        protected override void DoubleClickedItem(int id) {
            TreeViewItem item = FindItem(id, rootItem);
            if (item is AudioUnitTreeViewItem_AudioUnit unitItem) {
                unitItem.OnDoubleClick();
            }
            else if (item is AudioUnitTreeViewItem_Folder folderItem) {
                folderItem.OnDoubleClick();
            }
        }

        protected override bool CanStartDrag(CanStartDragArgs args) => true;

        protected override bool CanMultiSelect(TreeViewItem item) => false;

        protected override void SetupDragAndDrop(SetupDragAndDropArgs args) {
            Object[] dragObjects = GetRows()
                .Where(item => args.draggedItemIDs.Contains(item.id))
                .Select(x => {
                    if (x is AudioUnitTreeViewItem_Folder folderItem) {
                        return folderItem.FolderAsset as Object;
                    }
                    if (x is AudioUnitTreeViewItem_AudioUnit unitItem) {
                        return unitItem.AudioUnit as Object;
                    }
                    return null;
                })
                .Where(x => x != null)
                .ToArray();
            DragAndDropUtil.SendObjects(dragObjects);
        }

        protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args) {
            if (args.performDrop) {
                Object[] droppedObjects = DragAndDrop.objectReferences;
                if (droppedObjects == null || droppedObjects.Length == 0) {
                    return DragAndDropVisualMode.None;
                }

                switch (args.dragAndDropPosition) {
                    case DragAndDropPosition.UponItem:
                        if (args.parentItem is AudioUnitTreeViewItem_Folder parentFolderItem) {
                            string folderPath = AssetDatabase.GetAssetPath(parentFolderItem.FolderAsset);
                            IEnumerable<AudioUnit> audioUnits = droppedObjects.OfType<AudioUnit>();
                            EditorUtil.MoveAssetsWithUndo(audioUnits, folderPath);

                            IEnumerable<DefaultAsset> folders = droppedObjects.OfType<DefaultAsset>().Where(x => AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(x)));
                            EditorUtil.MoveAssetsWithUndo(folders, folderPath);

                            IEnumerable<AudioClip> clips = droppedObjects.OfType<AudioClip>();
                            AudioUnitUtil.CreateAudioUnits(clips, folderPath);
                        }
                        if (args.parentItem is AudioUnitTreeViewItem_AudioUnit parentUnitItem) {
                            string audioUnitFolder = EditorUtil.GetFolderPath(AssetDatabase.GetAssetPath(parentUnitItem.AudioUnit));

                            IEnumerable<AudioUnit> audioUnits = droppedObjects.OfType<AudioUnit>();
                            EditorUtil.MoveAssetsWithUndo(audioUnits, audioUnitFolder);

                            IEnumerable<DefaultAsset> folders = droppedObjects.OfType<DefaultAsset>().Where(x => AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(x)));
                            EditorUtil.MoveAssetsWithUndo(folders, audioUnitFolder);

                            IEnumerable<AudioClip> clips = droppedObjects.OfType<AudioClip>();
                            AudioUnitUtil.CreateAudioUnits(clips, audioUnitFolder);
                        }
                        Reload();
                        break;
                    default:
                        return DragAndDropVisualMode.None;
                }
            }

            if (args.dragAndDropPosition == DragAndDropPosition.UponItem) {
                return DragAndDropVisualMode.Move;
            }
            else {
                return DragAndDropVisualMode.None;
            }
        }

        protected override bool CanBeParent(TreeViewItem item) {
            return true;
        }
    }
}