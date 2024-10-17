using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    [System.Serializable]
    public class AudioUnitTreeView : TreeView {
        public AudioUnitTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader) {
            rowHeight = 16;
            // showAlternatingRowBackgrounds = true;
            // showBorder = true;
            // enableItemHovering = true;
            // depthIndentWidth = 18;
        }

        protected override TreeViewItem BuildRoot() {
            TreeViewItem root = new() {depth = -1};

            int id = 0;

            string folderPath = SoundSystemSetting.Instance.AudioUnitFolderRoot;
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
            Rect backgroundRect = new(args.rowRect) {xMin = args.rowRect.xMin + GetContentIndent(args.item) - 16};

            if (args.item is AudioUnitTreeViewItem_Folder folderItem) {
                GUI.Box(backgroundRect, "", GUIStyles.FolderRowBackground);
                for (int i = 0; i < args.GetNumVisibleColumns(); i++) {
                    Rect cellRect = args.GetCellRect(i);
                    int columnIndex = args.GetColumn(i);
                    switch (columnIndex) {
                        case 0:
                            cellRect.xMin += GetContentIndent(args.item);
                            string assetPath = AssetDatabase.GetAssetPath(folderItem.FolderAsset);
                            Texture icon = AssetDatabase.GetCachedIcon(assetPath);

                            EditorGUI.LabelField(cellRect, new GUIContent(folderItem.FolderAsset.name, icon));
                            break;
                    }
                }
            }
            else if (args.item is AudioUnitTreeViewItem_AudioUnit unitItem) {
                GUI.Box(backgroundRect, "", GUIStyles.SoundRowBackground);
                for (int i = 0; i < args.GetNumVisibleColumns(); i++) {
                    Rect cellRect = args.GetCellRect(i);
                    int columnIndex = args.GetColumn(i);
                    switch (columnIndex) {
                        case 0:
                            cellRect.xMin += GetContentIndent(args.item);
                            string assetPath = AssetDatabase.GetAssetPath(unitItem.AudioUnit);
                            Texture icon = AssetDatabase.GetCachedIcon(assetPath);

                            EditorGUI.LabelField(cellRect, new GUIContent(unitItem.AudioUnit.name, icon));
                            break;
                    }
                }
            }
        }

        protected override bool CanBeParent(TreeViewItem item) {
            return true;
        }

        protected override void SingleClickedItem(int id) {
            TreeViewItem item = FindItem(id, rootItem);
            if (item is AudioUnitTreeViewItem_Folder folderItem) {
                folderItem.OnSingleClick();
            }
            else if (item is AudioUnitTreeViewItem_AudioUnit unitItem) {
                unitItem.OnSingleClick();
            } 
        }

        protected override void DoubleClickedItem(int id) {
            TreeViewItem item = FindItem(id, rootItem);
            if (item is AudioUnitTreeViewItem_Folder folderItem) {
                folderItem.OnDoubleClick();
            }
            else if (item is AudioUnitTreeViewItem_AudioUnit unitItem) {
                unitItem.OnDoubleClick();
            }
        }

        protected override bool CanStartDrag(CanStartDragArgs args) {
            // TreeViewを1ウィンドウに複数描画＋ドラッグ有効だと一部のSingleClickedItemが呼ばれない事象への対処
            SingleClickedItem(args.draggedItem.id);
            return true;
        }

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
                            HandleDroppedObjects(droppedObjects, folderPath);
                        }
                        if (args.parentItem is AudioUnitTreeViewItem_AudioUnit parentUnitItem) {
                            string audioUnitFolder = EditorUtil.GetFolderPath(AssetDatabase.GetAssetPath(parentUnitItem.AudioUnit));
                            HandleDroppedObjects(droppedObjects, audioUnitFolder);
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

        static void HandleDroppedObjects(Object[] droppedObjects, string folderPath) {
            IEnumerable<AudioUnit> audioUnits = droppedObjects.OfType<AudioUnit>();
            EditorUtil.MoveAssetsWithUndo(audioUnits, folderPath);

            IEnumerable<DefaultAsset> folders = droppedObjects.OfType<DefaultAsset>().Where(x => AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(x)));
            EditorUtil.MoveAssetsWithUndo(folders, folderPath);

            IEnumerable<AudioClip> clips = droppedObjects.OfType<AudioClip>();
            AudioUnitUtil.CreateAudioUnits(clips, folderPath);
        }
    }
}